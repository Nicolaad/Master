using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class OwnershipManager : NetworkBehaviour
{

    public static OwnershipManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }


    public void RequestOwnershipOfObject(GameObject gameObject)
    {
        Debug.Log("Requested ownership!");
        RequestOwnershipServerRpc(gameObject);

    }

    [ServerRpc(RequireOwnership = false)]
    public void RequestOwnershipServerRpc(NetworkObjectReference objectReference, ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        NetworkObject networkObject = null;
        objectReference.TryGet(out networkObject);
        networkObject.ChangeOwnership(clientId);
    }

    [ServerRpc]
    public void RemoveOwnershipServerRpc(NetworkObjectReference objectReference)
    {
        NetworkObject networkObject = null;
        objectReference.TryGet(out networkObject);
        networkObject.RemoveOwnership();
    }




}
