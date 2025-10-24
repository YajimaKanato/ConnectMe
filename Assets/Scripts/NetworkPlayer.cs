using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class NetworkPlayer : NetworkBehaviour
{
    [SerializeField] float _moveSpeed = 10;
    [SerializeField] float _jumpPower = 10;
    [SerializeField] float _gravityScale = 5;

    Rigidbody _rb;
    InputAction _moveAct;
    InputAction _jumpAct;
    InputAction _connectAct;

    Vector3 _direction;

    bool _isJump;
    bool _isJumping;
    bool _isConnect;

    #region 初期設定
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Init();
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    /// <summary>
    /// 初期化関数
    /// </summary>
    void Init()
    {
        _rb = GetComponent<Rigidbody>();

        _moveAct = InputSystem.actions.FindAction("Move");
        _jumpAct = InputSystem.actions.FindAction("Jump");
        _connectAct = InputSystem.actions.FindAction("Connect");

        if (tag != TagManager.PlayerTag)
        {
            tag = TagManager.PlayerTag;
        }
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        //接続した際に生成されたこのオブジェクトのみを操作できるようにする
        if (IsOwner)
        {
            PlayerMoveServerRpc(_moveAct.ReadValue<Vector2>(), _jumpAct.triggered);
            StretchConnectorServerRpc(_connectAct.triggered);
        }

        //サーバーの時
        if (IsServer)
        {
            ServerUpdate();
        }
    }

    private void FixedUpdate()
    {
        if (IsServer)
        {
            ServerFixedUpdate();
        }
    }

    #region サーバー
    /// <summary>
    /// 移動入力を設定する関数
    /// クライアントからサーバーにアクションを起こす
    /// </summary>
    [ServerRpc]
    void PlayerMoveServerRpc(Vector2 direction, bool isJump)
    {
        _direction = new Vector3(direction.x, 0, direction.y).normalized;
        _isJump = isJump;
    }

    /// <summary>
    /// コネクトアクションを検知する関数
    /// </summary>
    [ServerRpc]
    void StretchConnectorServerRpc(bool isConnect)
    {
        _isConnect = isConnect;
    }

    /// <summary>
    /// 瞬間操作
    /// サーバーからアクションを受け取る
    /// </summary>
    void ServerUpdate()
    {
        if (!_isJumping)
        {
            if (_isJump)
            {
                var vel = _rb.linearVelocity;
                vel.y = 0;
                _rb.linearVelocity = vel;
                _rb.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
            }
        }
        if (_direction.magnitude != 0)
        {
            transform.forward = _direction;
        }

        if (_isConnect)
        {

        }
    }

    /// <summary>
    /// 継続操作
    /// サーバーからアクションを受け取る
    /// </summary>
    void ServerFixedUpdate()
    {
        if (_isJumping)
        {
            _rb.linearVelocity -= Vector3.up * 9.8f * _gravityScale * Time.deltaTime;
        }

        _rb.linearVelocity = _direction * _moveSpeed + Vector3.up * _rb.linearVelocity.y;
    }
    #endregion

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            _isJumping = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            _isJumping = true;
        }
    }
}
