using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using SystematicDeclaration.MultiplayerGame;
using UnityEngine;

namespace SysDec.MultiplayerGame
{
    public class BlockSounds : MonoBehaviour
    {
        PhotonView scripts;
        public string block_sound_name;

        public void Start()
        {
            scripts = GameObject.Find("Scripts").GetComponent<PhotonView>();
        }
        public void OnCollisionEnter(Collision collision)
        {
            if (collision == null) return;

            if (collision.gameObject.tag.Equals("Dinosaur"))
            {
                scripts.RPC("Play", RpcTarget.All, block_sound_name);
                scripts.RPC("Play", RpcTarget.All, "Dino Impact");
            }

            if (collision.gameObject.tag.Equals("TowerPiece"))
            {
                scripts.RPC("Play", RpcTarget.All, block_sound_name);
            }
        }
    }
}
