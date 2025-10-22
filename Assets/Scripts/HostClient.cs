using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;


public class HostClient : MonoBehaviour
{
    /// <summary>
    /// �z�X�g�ɂȂ�֐�
    /// </summary>
    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    /// <summary>
    /// �N���C�A���g�ɂȂ�֐�
    /// </summary>
    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }
}
