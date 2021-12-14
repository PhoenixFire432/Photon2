using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace SysDec.MultiplayerGame
{
    public class AmmoManager : MonoBehaviour
    {
        #region Fields
        [Header("Ammo")]
        public AmmoTemplate[] ammo;

        [Header("Values and Costs")]
        [SerializeField]
        private int starting_points_cannon;

        [Header("Label Texts")]
        public string points_remaining_label_text;
        public string button_lead_label_text;
        public string button_middle_label_text;
        public string button_tail_label_text;

        [Header("References")]
        public Text points_remaining_text;

        // private fields
        private int current_points_cannon;
        private AmmoTemplate currently_loaded_ammo;
        private bool last_shot_loaded = false;
        private AmmoPreview ap;
        private CannonBehaviors cb;

        [Header("Debugging")]
        public int lowest_ammo_cost;
        #endregion

        private void OnEnable()
        {
            // initialize values
            current_points_cannon = starting_points_cannon;
            ap = GameObject.Find("Scripts").GetComponent<AmmoPreview>();
            cb = GameObject.Find("Scripts").GetComponent<CannonBehaviors>();

            // finds the lowest ammo cost of all available ammo
            // doesn't work if every ammo costs more than the starting points (unlikely)
            lowest_ammo_cost = starting_points_cannon;
            foreach (AmmoTemplate a in ammo)
            {
                if (a.ammo_cost < lowest_ammo_cost)
                {
                    lowest_ammo_cost = a.ammo_cost;
                }
            }

            // set ui texts
            points_remaining_text.text = points_remaining_label_text + current_points_cannon;
            foreach (AmmoTemplate a in ammo)
            {
                // it looks weird but this is the best way I could think of to make these consistent
                a.ammo_button.gameObject.GetComponentInChildren<Text>().text 
                    = button_lead_label_text + a.ammo_name 
                    + button_middle_label_text + a.ammo_cost 
                    + button_tail_label_text;
            }
        }

        private void OnDisable()
        {
            currently_loaded_ammo = null;
            ap.PurgeAmmo();
        }

        private void Update()
        {
            // let the blocks player know that cannon is out of shots
            if (current_points_cannon < lowest_ammo_cost)
            {
                Debug.Log("AmmoManager -- Last Shot has been loaded");
                last_shot_loaded = true;

                // prevent this from being called a ton of times;
                lowest_ammo_cost = current_points_cannon;
            }
        }

        public void PurchaseAmmo (AmmoTemplate a)
        {
            if (current_points_cannon < a.ammo_cost)
            {
                Debug.Log("Not enough funds (ammo)"); //TODO make a notification on the screen for this
                return;
            }

            if (currently_loaded_ammo != null)
            {
                // refund unshot unit
                current_points_cannon += currently_loaded_ammo.ammo_cost;
            }

            current_points_cannon -= a.ammo_cost;
            points_remaining_text.text = points_remaining_label_text + current_points_cannon;
            currently_loaded_ammo = a;

            ap.PreviewAmmo(a.ammo_prefab);
        }

        /*
         * removes ammo preview and sets up for subsequent shots
         * returns: ammo template of ammo just fired or null
         */
        public AmmoTemplate AmmoFired ()
        {
            if (currently_loaded_ammo == null) return null;

            AmmoTemplate retval = currently_loaded_ammo;
            ap.PurgeAmmo();
            currently_loaded_ammo = null;

            if (last_shot_loaded)
            {
                //This starts the coroutine;
                StartCoroutine(delay());
                last_shot_loaded = false;
            }

            return retval;
        }

        private IEnumerator delay()
        {
            //Once the condition is met this will wait for one second
            //after that second has passed it will THEN run everything underneath the return
            yield return new WaitForSeconds(1.0f);
            Debug.Log("AmmoManager -- Last shot fired");
            cb.LastShotFired();
        }

        public bool AmmoReadyToFire ()
        {
            return !(currently_loaded_ammo==null);
        }
    }
}