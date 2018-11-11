using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
enum PickupType
{
    Health,
    Weapon,
    Ability
}

public class PickupScript : MonoBehaviour
{
    [SerializeField]
    private PickupType type;
    [SerializeField]
    private int indexOrPotency;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            switch(type)
            {
                case PickupType.Health:
                    col.gameObject.GetComponent<PlayerControllerScript>().HurtPlayer(-1 * indexOrPotency);
                    break;
                case PickupType.Weapon:
                    col.gameObject.GetComponent<PlayerControllerScript>().AddWeapon(indexOrPotency);
                    break;
                case PickupType.Ability:
                    //TODO: Change this if/when more than one ability is added to the game
                    col.gameObject.GetComponent<PlayerControllerScript>().CanGlide = true;
                    break;
            }
            Destroy(gameObject);
        }
    }
}
