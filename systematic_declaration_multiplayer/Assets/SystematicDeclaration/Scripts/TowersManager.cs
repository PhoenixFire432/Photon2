using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TowersManager : MonoBehaviour
{
    int towerLimit = 0;
    [Header("Set Dynamically")]
    public GameObject castle;

    public void SpawnTower(){

        if(towerLimit == 0){
            
            Debug.Log("I want to spwan a tower. PLS");
            PhotonNetwork.Instantiate("preMadeTower", new Vector3(-1, 12, 14), Quaternion.identity, 0);
            towerLimit++;
        }
        else
        {
            Debug.Log("You already have a tower!");
            //PhotonNetwork.Destroy(castle);
        }
    }
}