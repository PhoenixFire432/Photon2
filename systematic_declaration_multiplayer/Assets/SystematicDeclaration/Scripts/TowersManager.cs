using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TowersManager : MonoBehaviour
{
    [Header("Set in Inspector")]
    public Vector3 castlePos;

    [Header("Set Dynamically")]
    public GameObject castle;

    void Start()
    {
        //if (castle != null)
        //{
        //    PhotonNetwork.Destroy(castle);
        //}

        ////instantiate new castle
        //PhotonNetwork.Instantiate("towerTester", new Vector3(5, 0, 7), Quaternion.identity, 0);
        //castle.transform.position = castlePos;
    }

    public void SpawnTower(){
        Debug.Log("I want to spwan a tower. PLS");
        PhotonNetwork.Instantiate("preMadeTower", new Vector3(-1, 12, 14), Quaternion.identity, 0);
    }
}