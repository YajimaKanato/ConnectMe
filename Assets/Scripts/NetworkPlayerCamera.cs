using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.InputSystem;

public class NetworkPlayerCamera : MonoBehaviour
{
    [SerializeField] float _cameraSensitivity = 0.5f;
    Transform _player;
    public Transform Player
    {
        get { return _player; }
        set { if (!_player) _player = value; }
    }
    Vector3 _defaultRot;
    InputAction _cameraAct;

    private void Awake()
    {
        _defaultRot = transform.eulerAngles;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Init();
    }

    void Init()
    {
        _cameraAct = InputSystem.actions.FindAction("Look");
    }

    // Update is called once per frame
    void Update()
    {
        //if (IsOwner)
        {
            //CameraControlServerRpc(_cameraAct.ReadValue<Vector2>());
            //ServerUpdate();
            var rot = _cameraAct.ReadValue<Vector2>();
            _defaultRot.x -= rot.y;
            _defaultRot.y += rot.x;
            transform.rotation = Quaternion.Euler(_defaultRot);
            transform.position = _player.position;
        }
    }

    #region サーバー
    [ServerRpc]
    void CameraControlServerRpc(Vector2 rotation)
    {
        _defaultRot.x -= rotation.y;
        _defaultRot.y += rotation.x;
    }

    void ServerUpdate()
    {
        transform.rotation = Quaternion.Euler(_defaultRot);

        transform.position = _player.position;
    }
    #endregion
}
