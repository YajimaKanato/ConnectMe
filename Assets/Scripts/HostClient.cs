using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;


public class HostClient : MonoBehaviour
{
    [SerializeField] Vector3[] _positions;
    [SerializeField] int _maxConnectedClientsCount = 4;

    /// <summary>
    /// ホストになる関数
    /// </summary>
    public void StartHost()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;
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

    /// <summary>
    /// 接続承認応答処理を行う関数
    /// </summary>
    /// <param name="request"></param>
    /// <param name="response"></param>
    void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        //trueからfalseになると接続承認応答が処理される
        //承認応答は接続可能かどうかのこと
        response.Pending = true;

        //以下に承認手順を書く

        //接続最大人数評価
        if (NetworkManager.Singleton.ConnectedClients.Count >= _maxConnectedClientsCount)
        {
            //すでに接続人数が最大値に達していたら
            //接続を許可しない
            response.Approved = false;
            //接続承認応答を処理
            response.Pending = false;
            return;
        }

        //接続成功クライアントに向けた処理

        //接続を許可
        response.Approved = true;

        //DefaultPlayerObjectに設定されたオブジェクトを生成するかどうか
        response.CreatePlayerObject = true;

        //生成するプレハブのハッシュ値
        //nullの場合NetworkManagerに登録したプレハブ
        response.PlayerPrefabHash = null;

        //DefaultPlayerObjectをスポーンする位置
        //nullの場合Vector3.zero
        response.Position = _positions[NetworkManager.Singleton.ConnectedClients.Count % _maxConnectedClientsCount];

        //DefaultPlayerObjectの回転
        //nullの場合Quaternion.identity
        response.Rotation = Quaternion.identity;

        //接続承認応答を処理
        response.Pending = false;
    }
}
