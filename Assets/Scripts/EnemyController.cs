using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float dmgOnTouch;

    private SpriteRenderer sprite;
    private Color baseColor;
    private EntityHealth hp;
    private Rigidbody2D charControl;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        baseColor = sprite.color;
        hp = GetComponent<EntityHealth>();
        charControl = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        charControl.velocity = new Vector3(movementSpeed, 0, 0);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        switch(col.gameObject.tag)
        {
            case "Floor":
                return;
            case "Player":
                col.gameObject.GetComponent<PlayerControllerScript>().HurtPlayer(dmgOnTouch);
                break;
        }

        movementSpeed *= -1;
        gameObject.transform.localScale = new Vector3(Mathf.Abs(gameObject.transform.localScale.x) * Mathf.Sign(movementSpeed), gameObject.transform.localScale.y, gameObject.transform.localScale.z);
    }

    void FixedUpdate ()
    {
        sprite.color = new Color(baseColor.r * hp.Health / hp.MaxHealth, baseColor.g * hp.Health / hp.MaxHealth, baseColor.b * hp.Health / hp.MaxHealth);
	}
}
