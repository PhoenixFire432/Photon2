using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SysDec.MultiplayerGame {
    public class BlockTemplate : MonoBehaviour
    {
        public string block_name;
        public int block_cost;
        public GameObject block_prefab;
        public Button block_button;
        public string block_sound_name;

        private void Start()
        {
            // setup button to purchase a block from the blocks manager
            BlocksManager bm = GameObject.Find("Scripts").GetComponent<BlocksManager>();
            block_button.onClick.AddListener(() =>
            {
                bm.PurchaseBlock(this);
            });
        }
    }
}