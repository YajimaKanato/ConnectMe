using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;


public class HostClient : MonoBehaviour
{
    /// <summary>
    /// ホストになる関数
    /// </summary>
    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    /// <summary>
    /// クライアントになる関数
    /// </summary>
    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }
}
