using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using SystematicDeclaration.MultiplayerGame;
using UnityEngine;

namespace SysDec.MultiplayerGame
{
    public class Egg : MonoBehaviour
    {
        GameManager gm;

        public void Start()
        {
            gm = GameObject.Find("Scripts").GetComponent<GameManager>();
        }
        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag.Equals("Dinosaur"))
            {
                gm.EggHit();//.RPC("EggHit", RpcTarget.AllBuffered);
            }
        }
    }
}
