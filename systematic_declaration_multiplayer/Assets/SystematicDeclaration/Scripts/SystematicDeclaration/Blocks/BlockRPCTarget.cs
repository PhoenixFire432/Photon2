using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace SysDec.MultiplayerGame {
    public class BlockRPCTarget : MonoBehaviourPunCallbacks
    {
        Rigidbody rb;
        PhotonView local_player;

        public string block_sound_name;


        private void Start()
        {
            rb = gameObject.GetComponent<Rigidbody>();
            local_player = GameObject.Find("Scripts").GetComponent<PlayerManager>().player.GetComponent<PhotonView>();
        }

        private void Update() {
            Debug.Log(block_sound_name);
        }

        [PunRPC]
        public void TakeOver()
        {
            this.photonView.TransferOwnership(local_player.Owner);
        }

        [PunRPC]
        public void EnablePhysics()
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        [PunRPC]
        public void DisablePhysics()
        {
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }
        }
    }
}