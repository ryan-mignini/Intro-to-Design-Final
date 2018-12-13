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
                    //Do not use pickup if player cannot benefit from it - i.e. when they are at max health
                    EntityHealth playerHealth = col.gameObject.GetComponent<EntityHealth>();
                    if (playerHealth.Health == playerHealth.MaxHealth)
                        return;

                    //Otherwise increase their HP
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
