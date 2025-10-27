using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class NetworkPlayerCamera : NetworkBehaviour
{
    [SerializeField] float _cameraSensitivity = 0.5f;
    [SerializeField] Transform _player;
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
        transform.SetParent(null);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner)
        {
            CameraControlServerRpc(_cameraAct.ReadValue<Vector2>());
        }

        if (IsServer)
        {
            ServerUpdate();
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
