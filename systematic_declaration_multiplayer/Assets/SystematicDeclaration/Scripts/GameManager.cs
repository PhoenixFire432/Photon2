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

        #region Public Variables
        public GameObject playerPrefab;
        public PlayerManager player;
        #endregion

        #region Photon Callbacks
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.LogFormat("player {0} entered room", newPlayer.NickName);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.LogFormat("player {0} left room", otherPlayer.NickName);
        }
        #endregion

        #region Public Methods
        public void Start(){}

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }
        #endregion

        #region Private Methods
        void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("trying to load arena : FAILED -- is not the master client");
            }
            Debug.LogFormat("loading level. {0} players", PhotonNetwork.CurrentRoom.PlayerCount);
            PhotonNetwork.LoadLevel("Main");
        }
        #endregion
    }
}