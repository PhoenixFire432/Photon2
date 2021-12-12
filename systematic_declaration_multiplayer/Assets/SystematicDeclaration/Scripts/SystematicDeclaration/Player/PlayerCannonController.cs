using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace SysDec.MultiplayerGame
{
    public class PlayerCannonController : MonoBehaviour
    {
        #region Fields
        #region Values
        private bool cannon_player = false;
        private bool blocks_player = false;

        private float look_x, look_y, move_x, move_y = 0;
        private Vector3 rot_input = Vector3.forward;
        #endregion

        #region References
        public Transform cannon_body;
        public Transform cannon_barrel;
        public Transform cannon_camera_anchor;
        public Transform default_camera_anchor;
        public Photon.Realtime.Player local_player;

        private CannonBehaviors cb;
        private AmmoManager am;
        #endregion

        #region Editor
        [Header("Mouse Settings")]
        public float sensitivity_y = 1f;
        public float sensitivity_x = 1f;
        public float max_look_angle = 90f;

        [Header("Gun Settings")]
        [SerializeField]
        private float gun_angle_offset = 80;

        [Header("Inputs")]
        private string input_look_hori = "Mouse X";
        private string input_look_vert = "Mouse Y";
        private string input_move_hori = "Horizontal";
        private string input_move_vert = "Vertical";
        #endregion
        #endregion

        #region Methods
        // Update is called once per frame
        void Update()
        {
            // process inputs
            look_x = Input.GetAxis(input_look_hori);
            look_y = Input.GetAxis(input_look_vert);
            move_x = Input.GetAxis(input_move_hori);
            move_y = Input.GetAxis(input_move_vert);

            // register fire button + act
            if (Input.GetKeyDown("space"))
            {
                Debug.Log("space pressed");
                if (am.AmmoReadyToFire()) cb.FireCannon(am.AmmoFired());
            }

            // calculate cannon rotation
            rot_input.y += look_x * sensitivity_x * 100f * Time.deltaTime;
            rot_input.x += look_y * sensitivity_y * 100f * Time.deltaTime;
            rot_input.x = Mathf.Clamp(rot_input.x, 80f, max_look_angle + gun_angle_offset);

            // rotate cannon
            cannon_body.localRotation = Quaternion.Euler(new Vector3(cannon_body.localRotation.x, rot_input.y, cannon_body.localRotation.y));
            cannon_barrel.localRotation = Quaternion.Euler(new Vector3(-rot_input.x, cannon_barrel.localRotation.x, cannon_barrel.localRotation.y));
        }

        private void OnEnable()
        {
            //  initialize values         
            this.local_player = this.gameObject.GetComponent<PhotonView>().Owner;
            this.cannon_barrel = GameObject.Find("Barrel").transform;
            this.cannon_body = GameObject.Find("Railgun").transform;
            this.cannon_camera_anchor = GameObject.Find("cannon_camera_anchor").transform;
            this.default_camera_anchor = GameObject.Find("default_camera_anchor").transform;
            this.cb = GameObject.Find("Scripts").GetComponent<CannonBehaviors>();
            this.am = GameObject.Find("Scripts").GetComponent<AmmoManager>();

            //  transfer ownership of the gun-- this allows the photonplayer to update it
            cannon_barrel.gameObject.GetComponent<PhotonView>().TransferOwnership(local_player);
            cannon_body.gameObject.GetComponent<PhotonView>().TransferOwnership(local_player);

            Debug.Log(this.gameObject.GetComponent<PhotonView>().Owner.NickName + " cannon enabled"); ;
        }
        #endregion
    }
}