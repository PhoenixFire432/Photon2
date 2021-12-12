using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace SysDec.MultiplayerGame
{
    public class CannonBehaviors : MonoBehaviour
    {
        public Transform ammo_fire_anchor;

        public void FireCannon(AmmoTemplate ammo)
        {
            GameObject projectile = PhotonNetwork.Instantiate(ammo.ammo_prefab.name, ammo_fire_anchor.position, Quaternion.identity);
            Rigidbody projectile_rb = projectile.GetComponent<Rigidbody>();
            projectile_rb.isKinematic = false;
            projectile_rb.useGravity = true;
            projectile_rb.AddForce(ammo_fire_anchor.transform.forward * ammo.ammo_fire_force * 100);
        }
    }
}