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

        #region Set in Editor Fields

        #region Preferences
        [Header("Mouse Settings")]
        public float sensitivity_y = 1f;
        public float sensitivity_x = 1f;
        public float max_look_angle = 90f;
        [Header("Values")]
        public float force_value = 10;
        #endregion

        #region References
        [Header("References")]
        [SerializeField]
        private GameObject camera_prefab;

        [Header("Rail Gun")]
        [SerializeField]
        private float gun_angle_offset = 0;
        [SerializeField]
        private Transform cannon_body;
        [SerializeField]
        private Transform cannon_barrel;
        [SerializeField]
        private Transform cannon_camera_anchor;
        [SerializeField]
        private Transform ammo_preview_anchor;
        [SerializeField]
        private Transform ammo_fire_anchor;

        [Header("Ammunition")]
        [SerializeField]
        private GameObject[] ammo;
        #endregion

        #endregion

        #region Internal-Use Fields

        #region References
        private GameObject player_ref;
        private PhotonView player_pv;

        private GameObject ammo_preview_ref;
        #endregion

        #region Values
        private bool cannon_player = false;
        private bool blocks_player = false;

        private bool ready_to_fire = true;
        #endregion

        #region Inputs
        [Header("Inputs")]
        private string input_look_hori = "Mouse X";
        private string input_look_vert = "Mouse Y";
        private string input_move_hori = "Horizontal";
        private string input_move_vert = "Vertical";
        #endregion

        #region Instantiations
        private int current_ammo_value = 0;

        private float 
            look_x, look_y, 
            move_x, move_y = 0;

        private Vector3 rot_input = Vector3.forward;

        //private PhotonHashTable properties;
        #endregion

        #endregion

        #endregion

        #region MonoBehavior Callbacks
        private void Start()
        {
            if (camera_prefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else
            {
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                player_ref = PhotonNetwork.Instantiate(this.camera_prefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
                player_pv = player_ref.GetComponent<PhotonView>();

                // setup ammo preview
                ammo_preview_ref = Instantiate(ammo[current_ammo_value], ammo_preview_anchor);
                ammo_preview_ref.GetComponent<Rigidbody>().isKinematic = true;
                ammo_preview_ref.transform.localScale = new Vector3(.6f, .6f, .6f);


                //setup custom properties
                //properties = new PhotonHashTable();
                //properties.Add("cannon_player_exists", false);
                //properties.Add("blocks_player_exists", false);
                //PhotonNetwork.SetPlayerCustomProperties(properties);
            }
        }

        private void Update()
        {
            if (!player_pv.IsMine)
            {
                return;
            }

            ProcessInputs();
        }
        #endregion

        #region InputProcessing
        private void ProcessInputs ()
        {
            look_x = Input.GetAxis(input_look_hori);
            look_y = Input.GetAxis(input_look_vert);
            move_x = Input.GetAxis(input_move_hori);
            move_y = Input.GetAxis(input_move_vert);

            if (cannon_player && ready_to_fire) ProcessInputsCannon();
            //else if (blocks_player) ProcessInputsBlocks();
        }

        private void ProcessInputsCannon()
        {
            // register fire button + act
            if (Input.GetKeyDown("space"))
            {
                FireCannon();
            }

            // calculate cannon rotation
            rot_input.y += look_x * sensitivity_x * 100f * Time.deltaTime;
            rot_input.x += look_y * sensitivity_y * 100f * Time.deltaTime;
            rot_input.x = Mathf.Clamp(rot_input.x, 80f, max_look_angle+gun_angle_offset);

            // rotate cannon
            cannon_body.localRotation = Quaternion.Euler(new Vector3(cannon_body.localRotation.x, rot_input.y, cannon_body.localRotation.y));
            cannon_barrel.localRotation = Quaternion.Euler(new Vector3(-rot_input.x, cannon_barrel.localRotation.x, cannon_barrel.localRotation.y));
        }
        #endregion

        #region Private Functions
        private void FireCannon ()
        {
            Debug.Log("fire button hit");
            GameObject projectile = PhotonNetwork.Instantiate(ammo[current_ammo_value].name, ammo_fire_anchor.position, Quaternion.identity);
            //Debug.Log("fire_anchor\n eul: " + ammo_fire_anchor.eulerAngles + "|| loc: " + ammo_fire_anchor.localEulerAngles + "\nbarrel\n eul: " + cannon_barrel.eulerAngles + "loc: " + cannon_barrel.localEulerAngles);
            projectile.GetComponent<Rigidbody>().AddForce(ammo_fire_anchor.transform.forward * force_value * 100);
        }
        #endregion

        #region Special Player Roles
        public void PlayAsSpectator ()
        {
            // disable all special controls
            cannon_player = false;
            blocks_player = false;
        }

        public void PlayAsCannon ()
        {
            blocks_player = false;
            // ensure there isn't already a cannon player
            // move camera
            player_ref.transform.position = cannon_camera_anchor.position;
            // enable cannon controls
            cannon_player = true;
            Debug.Log("play as cannon");
        }

        public void PlayAsBlocks ()
        {
            cannon_player = false;
            // ensure there isn't already a blocks player
            // move camera
            // enable blocks controls
            blocks_player = true;
            Debug.Log("play as blocks");
        }
    }
    #endregion
}