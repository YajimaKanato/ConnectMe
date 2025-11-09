using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    Transform _player;

    public Transform Player
    {
        get { return _player; }
        set { if (!_player) _player = value; }
    }
}
