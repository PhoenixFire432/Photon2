using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BlockRPCTarget : MonoBehaviourPunCallbacks
{
    Rigidbody rb;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    [PunRPC]
    public void EnablePhysics ()
    {
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    [PunRPC]
    public void DisablePhysics ()
    {
        rb.isKinematic = true;
        rb.useGravity = false;
    }
}
