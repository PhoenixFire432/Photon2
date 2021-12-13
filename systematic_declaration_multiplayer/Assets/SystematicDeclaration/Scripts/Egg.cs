using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using SystematicDeclaration.MultiplayerGame;
using UnityEngine;

namespace SysDec.MultiplayerGame
{
    public class Egg : MonoBehaviour
    {
        PhotonView gm;

        public void Start()
        {
            gm = GameObject.Find("Scripts").GetComponent<PhotonView>();
        }
        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag.Equals("Dinosaur"))
            {
                gm.RPC("EggHit", RpcTarget.AllBuffered);
            }
        }
    }
}
