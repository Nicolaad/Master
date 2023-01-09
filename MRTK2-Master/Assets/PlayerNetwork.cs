using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{

    private readonly NetworkVariable<Vector3> _netPos = new NetworkVariable<Vector3>(writePerm: NetworkVariableWritePermission.Owner);
     private readonly NetworkVariable<Quaternion> _netRotation = new NetworkVariable<Quaternion>(writePerm: NetworkVariableWritePermission.Owner);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner) {
            _netPos.Value = transform.position;
            _netRotation.Value = transform.rotation;
        }
        else {
            transform.position = _netPos.Value;
            transform.rotation = _netRotation.Value;
        }
    }
}
