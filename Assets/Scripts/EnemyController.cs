using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float dmgOnTouch;
    [SerializeField]
    private float secondsPerBulletFired = 1;
    [SerializeField]
    private GameObject bulletType;

    private float speedMultiplier;

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
        speedMultiplier = 1;
    }

    void Update()
    {
        charControl.velocity = new Vector3(movementSpeed*speedMultiplier, 0, 0);
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
        Debug.Log(gameObject.name + " IS TOUCHING " + col.gameObject.name);
        movementSpeed *= -1;
        gameObject.transform.localScale = new Vector3(Mathf.Abs(gameObject.transform.localScale.x) * Mathf.Sign(movementSpeed), gameObject.transform.localScale.y, gameObject.transform.localScale.z);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            speedMultiplier = 0.1f;
            StartCoroutine(ShootAtPlayer());
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            speedMultiplier = 1;
            StopAllCoroutines();
        }
    }

    void FixedUpdate ()
    {
        sprite.color = new Color(baseColor.r * hp.Health / hp.MaxHealth, baseColor.g * hp.Health / hp.MaxHealth, baseColor.b * hp.Health / hp.MaxHealth);
	}

    private IEnumerator ShootAtPlayer()
    {
        while (true) //Exit condition is stopping the coroutine - done in OnTriggerExit2D
        {
            yield return new WaitForSeconds(secondsPerBulletFired);

            BulletScript newBullet = GameObject.Instantiate(bulletType, transform.position + new Vector3(0.275f * transform.localScale.x, -.52f, 1), transform.rotation).GetComponent<BulletScript>();
            newBullet.speed = (GameObject.FindGameObjectWithTag("Player").transform.localPosition - (transform.position + new Vector3(0.275f * transform.localScale.x, -.52f, 1))).normalized * 8;
            newBullet.dmgAmount = dmgOnTouch;
            newBullet.tagsToIgnore.Add(gameObject.tag);
        }
    }
}
