using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class NetworkPlayerCamera : NetworkBehaviour
{
    InputAction _cameraAct;
    Vector2 _rot;

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
        _rot = rotation;
    }

    void ServerUpdate()
    {
        transform.rotation = transform.rotation * Quaternion.Euler(_rot);
    }
    #endregion
}
