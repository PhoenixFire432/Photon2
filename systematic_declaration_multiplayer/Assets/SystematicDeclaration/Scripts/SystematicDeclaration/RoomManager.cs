using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

namespace SysDec.MultiplayerGame
{
    public class RoomManager : MonoBehaviourPunCallbacks
    {
        #region Photon
        public void LeaveRoom()
        {
            Debug.Log("leave room");
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
            }
            else
            {
                Debug.LogError("failed to load arena: not master client");
            }
        }
        #endregion
    }
}