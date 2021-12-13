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
        private bool last_shot_signal = false;
        public bool last_shot_fired = false;
        public float min_velocity_threshold;

        public void FireCannon(AmmoTemplate ammo)
        {
            GameObject projectile = PhotonNetwork.Instantiate(ammo.ammo_prefab.name, ammo_fire_anchor.position, Quaternion.identity);
            
            projectile_rb = projectile.GetComponent<Rigidbody>();
            projectile_rb.isKinematic = false;
            projectile_rb.useGravity = true;
            projectile_rb.AddForce(ammo_fire_anchor.transform.forward * ammo.ammo_fire_force * 100);

            FindObjectOfType<AudioManager>().Play("Shoot");
            FindObjectOfType<AudioManager>().Play(ammo.ammo_sound_name);

            if(last_shot_signal) last_shot_fired = true;
        }

        public void LastShotFired ()
        {
            last_shot_signal = true;
        }

        private void Update()
        {
            // if the last shot has been fired (and has stopped moving)
            // tell the other player to start checking if the game is over
            if (last_shot_fired)
            {
                Debug.Log("velocity: " + projectile_rb.velocity.magnitude + " " + "threshold: " + min_velocity_threshold);
                if (projectile_rb.velocity.magnitude <= min_velocity_threshold)
                {
                    this.gameObject.GetPhotonView().RPC("OutOfShots", RpcTarget.Others);
                }
            }
        }
    }
}