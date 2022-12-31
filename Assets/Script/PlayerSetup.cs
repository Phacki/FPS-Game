using UnityEngine;
using Unity.Netcode;
[RequireComponent(typeof(PlayerManager))]
public class PlayerSetup : NetworkBehaviour
{

    private NetworkVariable<int> randomNumber = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    [SerializeField]
    Behaviour[] partsToDisable;

    [SerializeField]
    string remoteLayerName = "RemotePlayer";

    private void Start()
    {
        //Disables all Components of other players i.e HUD and Camera
        if (!IsOwner)
        {
            DisableParts();
            IssueRemoteLayer();
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        string networkID = GetComponent<NetworkObject>().NetworkObjectId.ToString();
        PlayerManager user = GetComponent<PlayerManager>();
        GameManager.CreateUniqueUser(networkID, user);
    }

    void DisableParts()
    {
        for (int i = 0; i < partsToDisable.Length; i++)
        {
            partsToDisable[i].enabled = false;
        }
    }

    void IssueRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    private void OnDisable()
    {
        GameManager.DeleteUniqueUser(transform.name);
    }
}
