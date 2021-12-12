using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Photon.Pun;
using System.Collections;
using PhotonHashTable = ExitGames.Client.Photon.Hashtable;

namespace SystematicDeclaration.MultiplayerGame
{
    public class PlayerManager : MonoBehaviourPunCallbacks
    {

        #region Fields
        #region Public Fields
        #region Preferences
        [Header("Values")]
        public float force_value = 10;
        #endregion

        #region References
        [Header("References")]
        [SerializeField]
        private GameObject camera_prefab;
        //[SerializeField]
        //private Transform default_camera_anchor;

        [Header("Rail Gun")]
        //[SerializeField]
        //private Transform cannon_body;
        //[SerializeField]
        //private Transform cannon_barrel;
        //[SerializeField]
        //private Transform cannon_camera_anchor;
        [SerializeField]
        private Transform ammo_preview_anchor;
        [SerializeField]
        private Transform ammo_fire_anchor;

        [Header("Ammunition")]
        [SerializeField]
        private GameObject[] ammo;
        #endregion
        #endregion

        #region Private Fields
        private GameObject player_ref;
        private PhotonView player_pv;
        private GameObject ammo_preview_ref;

        private int current_ammo_value = 0;
        #endregion
        #endregion

        #region MonoBehavior Callbacks
        private void Start()
        {
            Debug.Log(photonView.IsMine);
            SetupAmmoPreview();
        }

        private void Update()
        {
            if (player_pv == null) return;
            //ProcessInputs();
        }
        #endregion
    
        #region Private Functions
        private void FireCannon()
        {
            Debug.Log("fire button hit");
            GameObject projectile = PhotonNetwork.Instantiate(ammo[current_ammo_value].name, ammo_fire_anchor.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody>().AddForce(ammo_fire_anchor.transform.forward * force_value * 100);
            FindObjectOfType<AudioManager>().Play("Shoot");
            FindObjectOfType<AudioManager>().Play("DinoLaunchSound");
        }

        private void SetupAmmoPreview ()
        {
            ammo_preview_ref = Instantiate(ammo[current_ammo_value], ammo_preview_anchor);
            ammo_preview_ref.GetComponent<Rigidbody>().isKinematic = true;
            ammo_preview_ref.transform.localScale = new Vector3(.6f, .6f, .6f);
        }
        #endregion

        #region Special Player Roles
        public void PlayAsSpectator()
        {
            // disable all special controls
            //cannon_player = false;
            //blocks_player = false;
        }

        public void PlayAsCannon()
        {
            //player_pv.RPC("EnableCannon", RpcTarget.AllBuffered);
            //player_pv.RPC("DisableCannon", RpcTarget.AllViaServer);
            //player_pv.ViewID
            //player_ref.GetComponent<LocalPlayer>().EnableCannon();
            player_pv.RPC("DisableCannon", RpcTarget.Others);
            Debug.Log("play as cannon");
        }

        public void PlayAsBlocks()
        {
            Debug.Log("play as blocks");
        }
        #endregion

        public void CreatePlayer()
        {
            if (camera_prefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else
            {
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                player_ref = PhotonNetwork.Instantiate(this.camera_prefab.name, new Vector3(0f, 15f, 0f), Quaternion.identity, 0);
                player_pv = player_ref.GetComponent<PhotonView>();

                Debug.Log("player_pv mine? " + player_pv.IsMine);

                player_ref.GetComponent<Camera>().enabled = true;
                player_ref.GetComponent<AudioListener>().enabled = true;
            }
        }
    }
}