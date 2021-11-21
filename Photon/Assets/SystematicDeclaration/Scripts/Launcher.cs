using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace SystematicDeclaration.MultiplayerGame
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields
        [Tooltip("maximum players per room. when full, new players cannot join the room")]
        [SerializeField]
        private byte maxPlayersPerRoom = 3;
        #endregion

        #region Public Fields
        [Tooltip("UI panel to let the user connect")]
        [SerializeField]
        private GameObject controlPanel;
        [Tooltip("UI label to inform user of connection progress")]
        [SerializeField]
        private GameObject progressLabel;
        #endregion

        #region Private Fields
        // client's version number
        string gameVersion = "1";
        // current process
        bool isConnecting;
        #endregion

        #region MonoBehavior CallBacks
        void Awake ()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        void Start ()
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
        }
        #endregion

        #region Public Methods
        public void Connect ()
        {
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);

            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            } else
            {
                isConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }
        #endregion

        #region MonoBehaviorPunCallbacks Callbacks
        public override void OnConnectedToMaster()
        {
            //base.OnConnectedToMaster();
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
            if (isConnecting)
            {
                PhotonNetwork.JoinRandomRoom();
                isConnecting = false;
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);

            isConnecting = false;

            //base.OnDisconnected(cause);
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN OnJoinRandomFailed -- creating new room");
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("PUN OnJoinedRoom called -- now client is in a room");
            PhotonNetwork.LoadLevel("Main");
        }
        #endregion
    }
}