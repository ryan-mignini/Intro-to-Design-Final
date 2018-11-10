using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayerOnContact : MonoBehaviour
{
    [SerializeField]
    private float dmgAmount;

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.CompareTag("Player"))
            return;

        col.gameObject.GetComponent<PlayerControllerScript>().HurtPlayer(dmgAmount);
    }

    void OnTriggerEnter2d(Collider2D col)
    {
        if (!col.CompareTag("Player"))
            return;

        col.gameObject.GetComponent<PlayerControllerScript>().HurtPlayer(dmgAmount);
    }
}
