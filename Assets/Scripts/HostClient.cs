using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;


public class HostClient : MonoBehaviour
{
    [SerializeField] Vector3[] _positions;
    [SerializeField] int _maxConnectedClientsCount = 4;

    /// <summary>
    /// �z�X�g�ɂȂ�֐�
    /// </summary>
    public void StartHost()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;
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

    /// <summary>
    /// �ڑ����F�����������s���֐�
    /// </summary>
    /// <param name="request"></param>
    /// <param name="response"></param>
    void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        //true����false�ɂȂ�Ɛڑ����F���������������
        //���F�����͐ڑ��\���ǂ����̂���
        response.Pending = true;

        //�ȉ��ɏ��F�菇������

        //�ڑ��ő�l���]��
        if (NetworkManager.Singleton.ConnectedClients.Count >= _maxConnectedClientsCount)
        {
            //���łɐڑ��l�����ő�l�ɒB���Ă�����
            //�ڑ��������Ȃ�
            response.Approved = false;
            //�ڑ����F����������
            response.Pending = false;
            return;
        }

        //�ڑ������N���C�A���g�Ɍ���������

        //�ڑ�������
        response.Approved = true;

        //DefaultPlayerObject�ɐݒ肳�ꂽ�I�u�W�F�N�g�𐶐����邩�ǂ���
        response.CreatePlayerObject = true;

        //��������v���n�u�̃n�b�V���l
        //null�̏ꍇNetworkManager�ɓo�^�����v���n�u
        response.PlayerPrefabHash = null;

        //DefaultPlayerObject���X�|�[������ʒu
        //null�̏ꍇVector3.zero
        response.Position = _positions[NetworkManager.Singleton.ConnectedClients.Count % _maxConnectedClientsCount];

        //DefaultPlayerObject�̉�]
        //null�̏ꍇQuaternion.identity
        response.Rotation = Quaternion.identity;

        //�ڑ����F����������
        response.Pending = false;
    }
}
