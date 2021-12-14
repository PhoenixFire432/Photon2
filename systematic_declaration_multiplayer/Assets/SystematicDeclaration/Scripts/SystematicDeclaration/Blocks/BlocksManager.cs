using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace SysDec.MultiplayerGame
{
    public class BlocksManager : MonoBehaviourPunCallbacks
    {
        #region Fields
        [Header("Blocks")]
        public BlockTemplate[] blocks;

        [SerializeField]
        private int starting_points_blocks;
        [SerializeField]
        private float min_velocity_threshold;

        [Header("Label Texts")]
        public string points_remaining_label_text;
        public string button_lead_label_text;
        public string button_middle_label_text;
        public string button_tail_label_text;

        [Header("References")]
        public Text points_remaining_text;
        public Transform block_spawn_location;

        // private fields
        private int current_points_blocks;
        private bool cannon_out_of_shots = false;
        public List<GameObject> spawned_blocks;
        #endregion

        private void OnEnable()
        {
            // initialize values
            current_points_blocks = starting_points_blocks;
            points_remaining_text.text = points_remaining_label_text + current_points_blocks;
            spawned_blocks = new List<GameObject>();

            // set ui texts
            foreach (BlockTemplate b in blocks)
            {
                b.block_button.gameObject.GetComponentInChildren<Text>().text 
                    = button_lead_label_text + b.block_name 
                    + button_middle_label_text + b.block_cost 
                    + button_tail_label_text;
            }
        }

        private void Update()
        {
            if (cannon_out_of_shots)
            {
                bool game_over = true;
                foreach (GameObject b in spawned_blocks)
                {
                    if (b.GetComponent<Rigidbody>().velocity.magnitude >= min_velocity_threshold)
                    {
                        game_over = false;
                    }
                }
                if (game_over)
                {
                    this.photonView.RPC("BuilderWins", RpcTarget.AllBuffered);
                    cannon_out_of_shots = false;
                }
            }
        }

        public void PurchaseBlock (BlockTemplate b)
        {
            if (current_points_blocks < b.block_cost)
            {
                Debug.Log("Not enough funds (blocks)"); // Todo: make a notification on the screen for this
                return;
            }

            current_points_blocks -= b.block_cost;
            points_remaining_text.text = points_remaining_label_text + current_points_blocks;
            
            GameObject new_block = PhotonNetwork.Instantiate(b.block_prefab.name, block_spawn_location.position, Quaternion.identity);
            new_block.GetComponent<BlockSounds>().block_sound_name = b.block_sound_name;
            spawned_blocks.Add(new_block);
        }

        [PunRPC]
        public void OutOfShots()
        {
            cannon_out_of_shots = true;
        }

   
    }
}