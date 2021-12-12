using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

namespace SysDec.MultiplayerGame
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        #region Fields

        [Header("References")]
        public GameObject dinosaurs_win_panel;
        public GameObject builder_wins_panel;
        public GameObject role_selection_panel;
        public GameObject cannon_panel;
        public GameObject blocks_panel;
        #endregion

        // ---

        #region MonoBehavior Callbacks
        private void Start()
        {
            // disable UI that isn't in use
            dinosaurs_win_panel.SetActive(false);
            builder_wins_panel.SetActive(false);
            cannon_panel.SetActive(false);
            blocks_panel.SetActive(false);
        }
        #endregion

        // ---

        #region Public Methods
        [PunRPC]
        public void EggHit()
        {
            Debug.Log("egg hit by player's ammunition (dinosaur)");
            dinosaurs_win_panel.SetActive(true);
            FindObjectOfType<AudioManager>().Play("Victory");
        }

        [PunRPC]
        public void BuilderWins()
        {
            Debug.Log("out of ammo!");
            builder_wins_panel.SetActive(true);
        }

        public void EnableCannon ()
        {
            role_selection_panel.SetActive(false);
            cannon_panel.SetActive(true);
            this.gameObject.GetComponent<AmmoManager>().enabled = true;
        }

        [PunRPC]
        public void DisableCannon ()
        {
            role_selection_panel.SetActive(true);
            cannon_panel.SetActive(false);
            this.gameObject.GetComponent<AmmoManager>().enabled = false;
        }

        public void EnableBlocks ()
        {
            role_selection_panel.SetActive(false);
            blocks_panel.SetActive(true);
            this.gameObject.GetComponent<BlocksManager>().enabled = true;
        }

        [PunRPC]
        public void DisableBlocks ()
        {
            role_selection_panel.SetActive(true);
            blocks_panel.SetActive(false);
            this.gameObject.GetComponent<BlocksManager>().enabled = false;
        }
        #endregion
    }
}