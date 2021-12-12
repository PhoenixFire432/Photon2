using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

namespace SystematicDeclaration.MultiplayerGame
{
    public class GameManager : MonoBehaviourPunCallbacks
    {

        #region Fields
        public GameObject dinosaurs_win_canvas;
        private PlayerManager pm;
        #endregion

        #region Unity Callbacks
        private void Start()
        {
            pm = GameObject.Find("Local Player Manager").GetComponent<PlayerManager>();

            dinosaurs_win_canvas.SetActive(false);
            pm.CreatePlayer();
        }
        #endregion

        #region Photon

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        void LoadArena()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel("Main");
            } else
            {
                Debug.LogError("failed to load arena: not master client");
            }            
        }
        #endregion

        #region Public Methods
        [PunRPC]
        public void EggHit ()
        {
            Debug.Log("egg hit by player's ammunition (dinosaur)");
            dinosaurs_win_canvas.SetActive(true);
            FindObjectOfType<AudioManager>().Play("Victory");
        }
        #endregion
    }
}