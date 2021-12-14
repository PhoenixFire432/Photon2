using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace SysDec.MultiplayerGame
{
    public class CannonBehaviors : MonoBehaviour
    {
        public Transform ammo_fire_anchor;
        private Rigidbody projectile_rb;
        private PhotonView scripts;

        //private bool shot_stopped_moving;

        public bool last_shot_fired;
        public float min_velocity_threshold;

        private void Start()
        {
            last_shot_fired = false;
            scripts = GameObject.Find("Scripts").GetComponent<PhotonView>();
        }

        public void FireCannon(AmmoTemplate ammo)
        {
            // spawn ammo
            GameObject projectile = PhotonNetwork.Instantiate(ammo.ammo_prefab.name, ammo_fire_anchor.position, Quaternion.identity);
            
            // fire ammo
            projectile_rb = projectile.GetComponent<Rigidbody>();
            projectile_rb.isKinematic = false; 
            projectile_rb.useGravity = true;
            projectile_rb.AddForce(ammo_fire_anchor.transform.forward * ammo.ammo_fire_force * 100);

            // play sounds
            scripts.RPC("Play", RpcTarget.All, "Shoot");
            scripts.RPC("Play", RpcTarget.All, ammo.ammo_sound_name);

            if (last_shot_fired)
            {
                last_shot_fired = false;
            }
        }

        public void LastShotFired ()
        {
            last_shot_fired = true;
            Debug.Log("LAST SHOT HAS BEEN FIRED");
        }

        // voodoo -- this is the result of a ton of fiddling to kill bugs
        private void LateUpdate()
        {
            //if (projectile_rb != null) Debug.Log(projectile_rb.velocity.magnitude);
            if (last_shot_fired)
            {
                Debug.Log(projectile_rb.velocity.magnitude);
                if (projectile_rb.velocity.magnitude <= min_velocity_threshold)
                {
                    Debug.Log("builder victory");
                    this.gameObject.GetPhotonView().RPC("OutOfShots", RpcTarget.Others);
                    last_shot_fired = false;
                }
            }
        }
    }
}