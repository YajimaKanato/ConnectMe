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

    #region �����ݒ�
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
    /// �������֐�
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
        //�I�[�i�[�̎�
        if (IsOwner)
        {
            PlayerMoveServerRpc(_moveAct.ReadValue<Vector2>(), _jumpAct.IsPressed());
        }

        //�T�[�o�[�̎�
        if (IsServer)
        {
            ServerUpdate();
        }
    }

    /// <summary>
    /// �W�����v���s���֐�
    /// </summary>
    /// <param name="context"></param>
    void Jump(InputAction.CallbackContext context)
    {
        //_isJump = true;
    }

    /// <summary>
    /// �ړ����͂�ݒ肷��֐�
    /// �N���C�A���g����T�[�o�[�ɃA�N�V�������N����
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
