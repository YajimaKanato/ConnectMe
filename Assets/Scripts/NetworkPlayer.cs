using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class NetworkPlayer : NetworkBehaviour
{
    [SerializeField] float _movePower = 10;
    [SerializeField] float _jumpPower = 10;
    [SerializeField] LayerMask _groundLayer;

    Rigidbody _rb;
    InputAction _moveAct;
    InputAction _jumpAct;

    Vector3 _direction;

    bool _isJump;
    bool _isJumping;

    #region 初期設定
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        _jumpAct.started += Jump;
    }

    private void OnDisable()
    {
        _jumpAct.started -= Jump;
    }

    /// <summary>
    /// 初期化関数
    /// </summary>
    void Init()
    {
        _rb = GetComponent<Rigidbody>();

        _moveAct = InputSystem.actions.FindAction("Move");
        _jumpAct = InputSystem.actions.FindAction("Jump");
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        //オーナーの時
        if (IsOwner)
        {
            PlayerMoveServerRpc(_moveAct.ReadValue<Vector2>(), _jumpAct.IsPressed());
        }

        //サーバーの時
        if (IsServer)
        {
            ServerUpdate();
        }
    }

    /// <summary>
    /// ジャンプを行う関数
    /// </summary>
    /// <param name="context"></param>
    void Jump(InputAction.CallbackContext context)
    {
        //_isJump = true;
    }

    /// <summary>
    /// 移動入力を設定する関数
    /// クライアントからサーバーにアクションを起こす
    /// </summary>
    [ServerRpc]
    void PlayerMoveServerRpc(Vector2 direction, bool isJump)
    {
        _direction = new Vector3(direction.x, 0, direction.y);
        _isJump = isJump;

        //if (_isJump)
        //{
        //    _rb.AddForce(transform.up * _jumpPower, ForceMode.Impulse);
        //}
    }

    void ServerUpdate()
    {
        _rb.AddForce(_direction * _movePower);
        if (_isJumping)
        {
            if (_isJump)
            {
                _rb.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            _isJumping = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            _isJumping = false;
        }
    }
}
