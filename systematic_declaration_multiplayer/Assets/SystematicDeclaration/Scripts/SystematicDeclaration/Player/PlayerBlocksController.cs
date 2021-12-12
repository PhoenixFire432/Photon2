using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SysDec.MultiplayerGame
{
    public class PlayerBlocksController : MonoBehaviour
    {
        #region Fields
        public float speed;
        public float sensitivity;
        [Range(0,1f)]
        public float view_angle;

        // private fields
        private Vector2 input;
        private Vector3 direction;
        private const string mouse_x = "Mouse X";
        private const string mouse_y = "Mouse Y";
        private float rot_x = 0;
        private float rot_y = 0;
        private float block_rot_x = 0;
        private float block_rot_y = 0;
        private BlocksManager bm;
        private GameObject held_block;
        private bool rotating_block = false;

        [Header("Debug Values")]
        //public Quaternion last_camera_angle;
        //public Quaternion current_camera_angle;
        public float x = 0;
        public float y = 0;
        #endregion

        #region Methods
        #region Monobehaviour Callbacks
        private void Start()
        {
            bm = GameObject.Find("Scripts").GetComponent<BlocksManager>();
        }

        private void Update()
        {
            // get mouse input
            x = Input.GetAxis(mouse_x);
            y = Input.GetAxis(mouse_y);

            // translate
            input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            direction = new Vector3(input.x, 0, input.y);
            this.gameObject.transform.Translate(direction * speed * Time.deltaTime);

            // rotate
            if (!rotating_block)
            {
                rot_x += x * sensitivity;
                rot_y -= y * sensitivity;
                transform.eulerAngles = new Vector3(rot_y, rot_x, 0);
            }
            else
            {
                block_rot_x += x * sensitivity;
                block_rot_y -= y * sensitivity;
                if (held_block != null) held_block.transform.eulerAngles = new Vector3(block_rot_y, block_rot_x, 0);
            }

            // register grab button press
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameObject facing_block = GetBlockFacing();
                if (facing_block != null) GrabBlock(facing_block);
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                if (held_block != null) ReleaseBlock(held_block);
            }

            // toggle block rotating vs camera rotating
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                rotating_block = true;
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                rotating_block = false;
                block_rot_x = block_rot_y = 0;
            }
        }
        #endregion

        void GrabBlock (GameObject block)
        {
            if (block is null)
            {
                throw new ArgumentNullException(nameof(block));
            }

            held_block = block;

            Rigidbody block_rb = block.GetComponent<Rigidbody>();
            block_rb.isKinematic = true;
            block_rb.useGravity = false;

            block.transform.SetParent(this.gameObject.transform, true);
        }

        void ReleaseBlock (GameObject block)
        {
            Rigidbody block_rb = block.GetComponent<Rigidbody>();
            block_rb.isKinematic = false;
            block_rb.useGravity = true;

            block.transform.parent = null;
            held_block = null;
        }

        /*
         * returns the first block game object that the player is looking at
         * returns null if none found
         * change view-angle strictness w/ 'view_angle' var
         */
        private GameObject GetBlockFacing ()
        {
            GameObject retval = null;

            Vector3 forward = transform.TransformDirection(Vector3.forward).normalized;
            Vector3 toOther = Vector3.zero;

            foreach (GameObject g in bm.spawned_blocks)
            {
                toOther = g.transform.position - transform.position;
                float dot = Vector3.Dot(forward, toOther.normalized);

                if (dot >= view_angle)
                {
                    Debug.Log("found block " + g.name + ", dot = " + dot);
                    retval = g;
                }
            }

            return retval;
        }
        #endregion
    }
}