using Unity.Netcode;
using UnityEngine;

public class Cylinder : NetworkBehaviour
{
    float _delta;
    private void Update()
    {
        if (IsServer)
        {
            _delta += Time.deltaTime;
            if (_delta > 1) _delta = 0;
            transform.position = new Vector3(transform.position.x, Mathf.Sin(2 * Mathf.PI * _delta), transform.position.z);
        }
    }
}
