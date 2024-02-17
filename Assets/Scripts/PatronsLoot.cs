using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatronsLoot : MonoBehaviour
{
    private GameObject Hero;
    public string patronsType;
    public int patronsAmount;
    private void Start()
    {
        Hero = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            if(patronsType == "shotgun")
                Hero.GetComponent<CharacterControl>().shotgunPatrons += patronsAmount;
            else if(patronsType == "revolver")
                Hero.GetComponent<CharacterControl>().revolverPatrons += patronsAmount;
            Destroy(gameObject);
        }
    }
}
