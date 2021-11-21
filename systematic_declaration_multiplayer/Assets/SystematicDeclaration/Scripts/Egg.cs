using System.Collections;
using System.Collections.Generic;
using SystematicDeclaration.MultiplayerGame;
using UnityEngine;

public class Egg : MonoBehaviour
{
    private GameManager gm;

    public void Start()
    {
        gm = GameObject.FindWithTag("Game Manager").GetComponent<GameManager>();
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Dinosaur"))
        {
            gm.EggHit();
        }
    }
}
