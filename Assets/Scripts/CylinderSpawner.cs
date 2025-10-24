using Unity.Netcode;
using UnityEngine;

public class CylinderSpawner : NetworkBehaviour
{
    [SerializeField] NetworkObject _prefab;

    static CylinderSpawner _instance;
    public static CylinderSpawner Instance => _instance;

    public override void OnNetworkSpawn()
    {
        //ホストの時だけ
        if (IsHost)
        {
            GenerateCylinder();
        }

        //シングルトン処理
        if(_instance == null)
        {
            _instance = this;
        }
    }

    void GenerateCylinder()
    {
        for (int i = 0; i < 10; i++)
        {
            var cylinder = Instantiate(_prefab);
            int posX = Random.Range(0, 10) - 5;
            int posZ = Random.Range(0, 10) - 5;
            cylinder.transform.position = new Vector3(posX, 0, posZ);
            cylinder.transform.rotation = Quaternion.identity;
            cylinder.Spawn();
        }
    }
}
