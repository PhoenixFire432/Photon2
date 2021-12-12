using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace SysDec.MultiplayerGame
{
    public class AmmoPreview : MonoBehaviourPunCallbacks
    {
        #region Fields
        public Transform preview_anchor;
        public Vector3 preview_scale;
        public Vector3 rotation_v3;
        private GameObject ammo_preview;
        private Rigidbody preview_rb;
        #endregion

        private void Update()
        {
            if (ammo_preview == null) return;
            ammo_preview.transform.Rotate(rotation_v3*Time.deltaTime);
        }

        public void PreviewAmmo (GameObject prefab)
        {
            PurgeAmmo();
            ammo_preview = PhotonNetwork.Instantiate(prefab.name, preview_anchor.position, Quaternion.identity, 0);
            ammo_preview.transform.localScale = preview_scale;
        }

        public void PurgeAmmo ()
        {
            if (ammo_preview == null) return;

            PhotonNetwork.Destroy(ammo_preview);
            ammo_preview = null;
        }
    }
}