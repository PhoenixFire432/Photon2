using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SysDec.MultiplayerGame
{
    /*
     * Provides metadata to ammomanager about one type of ammo, as well as which button purchases it
     */
    public class AmmoTemplate : MonoBehaviour
    {
        public string ammo_name;
        public int ammo_cost;
        public float ammo_fire_force;
        public GameObject ammo_prefab;
        public Button ammo_button;
        public string ammo_sound_name;

        private void Start()
        {
            // setup button to call purchase ammo function passing the ammo template as a parameter
            AmmoManager am = GameObject.Find("Scripts").GetComponent<AmmoManager>();
            ammo_button.onClick.AddListener(() =>
            {
                am.PurchaseAmmo(this);
            });
        }
    }
}