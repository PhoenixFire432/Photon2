using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace SysDec.MultiplayerGame
{
    public class Player : MonoBehaviour
    {
        GameManager gm;

        private void Start()
        {
            gm = GameObject.Find("Scripts").GetComponent<GameManager>();
        }

        public void EnableCannon ()
        {
            this.GetComponent<PlayerCannonController>().enabled = true;
            gm.EnableCannon();
        }

        public void DisableCannon ()
        {
            this.GetComponent<PlayerCannonController>().enabled = false;
            gm.DisableCannon();
        }

        public void EnableBlocks ()
        {
            this.GetComponent<PlayerBlocksController>().enabled = true;
            gm.EnableBlocks();
        }

        public void DisableBlocks ()
        {
            this.GetComponent<PlayerBlocksController>().enabled = false;
            gm.DisableBlocks();
        }
    }
}