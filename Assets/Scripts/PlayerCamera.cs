using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>ÉçÅ[ÉJÉãÇ»ë∂ç›</summary>
public class PlayerCamera : MonoBehaviour
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
        if (_player)
        {
            var rot = _cameraAct.ReadValue<Vector2>();
            _defaultRot.x -= rot.y;
            _defaultRot.y += rot.x;
            transform.rotation = Quaternion.Euler(_defaultRot);
            transform.position = _player.position;
        }
    }
}
