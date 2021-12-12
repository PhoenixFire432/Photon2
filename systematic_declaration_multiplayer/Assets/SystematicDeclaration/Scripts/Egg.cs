using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using SystematicDeclaration.MultiplayerGame;
using UnityEngine;

public class Egg : MonoBehaviour
{
    PhotonView gm;

    public void Start()
    {
        gm = GameObject.Find("Game Manager").GetComponent<PhotonView>();
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Dinosaur"))
        {
            gm.RPC("EggHit", RpcTarget.AllBuffered);
        }
    }
}
