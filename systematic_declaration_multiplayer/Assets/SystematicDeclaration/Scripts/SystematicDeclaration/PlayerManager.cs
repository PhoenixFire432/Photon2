using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace SysDec.MultiplayerGame
{
    public class PlayerManager : MonoBehaviourPunCallbacks
    {
        #region Fields
        [Header("Set In Editor")]
        [SerializeField]
        private GameObject player_prefab;
        [SerializeField]
        private Transform cannon_camera_anchor;
        [SerializeField]
        private Transform blocks_camera_anchor;

        [Header("Debug Values")]
        [SerializeField]
        private GameObject player;
        #endregion

        private void Start()
        {
            player = PhotonNetwork.Instantiate(player_prefab.name, Vector3.zero, Quaternion.identity, 0);
            player.GetComponent<Camera>().enabled = true;
            player.GetComponent<AudioListener>().enabled = true;
        }

        [PunRPC]
        public void CannonGoAhead()
        {
            PlayerCannonController pcc = player.GetComponent<PlayerCannonController>();
            if (pcc.enabled)
            {
                pcc.cannon_go_ahead = true;
            }
        }

        public void StopBlocks()
        {
            PlayerBlocksController pbc = player.GetComponent<PlayerBlocksController>();
            if (pbc.enabled)
            {
                pbc.can_move_blocks = false;
            }
        }

        #region Set Player Special Roles
        public void SetCannonPlayerToLocal()
        {
            // ensure that one player is not both blocks and cannon
            player.GetComponent<Player>().DisableBlocks();

            // ensure that only one player is the 'cannon player'
            player.GetComponent<Player>().EnableCannon();
            this.GetComponent<PhotonView>().RPC("SetCannonPlayerToRemote", RpcTarget.Others);

            // move player to proper position
            player.transform.position = cannon_camera_anchor.position;
        }

        [PunRPC]
        public void SetCannonPlayerToRemote()
        {
            player.GetComponent<Player>().DisableCannon();
        }

        public void SetBlocksPlayerToLocal ()
        {
            // ensure that one player is not both blocks and cannon
            player.GetComponent<Player>().DisableCannon();

            // ensure that only one player is the 'blocks' player
            player.GetComponent<Player>().EnableBlocks();
            this.GetComponent<PhotonView>().RPC("SetBlocksPlayerToRemote", RpcTarget.Others);

            // move blocks player to position
            player.transform.position = blocks_camera_anchor.position;
        }

        [PunRPC]
        public void SetBlocksPlayerToRemote()
        {
            player.GetComponent<Player>().DisableBlocks();
        }
        #endregion
    }
}
