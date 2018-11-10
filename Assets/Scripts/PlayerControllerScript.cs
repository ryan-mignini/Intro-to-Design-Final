using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerScript : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed, jumpForce;
    [SerializeField]
    private GameObject bulletType;
    [SerializeField]
    private Collider2D groundCheckTrigger;
    [SerializeField]
    private Image hpBar, gunSelectionImg;
    [SerializeField]
    private bool canGlide;

    private Rigidbody2D charControl;
    private EntityHealth hp;
    private float facingDirection = 1;
    private float secondsSinceLastShot = 0;
    private int weaponSelected = 0;
    private bool isInvincible = false;

    // Use this for initialization
    void Start()
    {
        charControl = GetComponent<Rigidbody2D>();
        hp = GetComponent<EntityHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        charControl.velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, charControl.velocity.y);
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            facingDirection = Mathf.Sign(Input.GetAxisRaw("Horizontal"));
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (groundCheckTrigger.OverlapCollider(new ContactFilter2D(), new Collider2D[2]) > 1)
            {
                charControl.AddForce(new Vector2(0, jumpForce));
            }
        }

        secondsSinceLastShot += Time.deltaTime;
        if (Input.GetButtonDown("Fire"))
        {
            switch (weaponSelected)
            {
                case 0: //Pistol
                    FireBullet(new Vector3(15 * facingDirection, 0, 0), 10);
                    break;
                case 1: //Shotgun
                    if (secondsSinceLastShot >= 1)
                    {
                        secondsSinceLastShot = 0;
                        FireBullet(new Vector3(10 * facingDirection, .125f, 0), 5);
                        FireBullet(new Vector3(10 * facingDirection, -.125f, 0), 5);
                        FireBullet(new Vector3(9.9f * facingDirection, .375f, 0), 5);
                        FireBullet(new Vector3(9.9f * facingDirection, -.375f, 0), 5);
                    }
                    break;
            }
        }
        if(Input.GetButton("Fire"))
        {
            switch(weaponSelected)
            {
                case 2:
                    if(secondsSinceLastShot >= 0.15)
                    {
                        secondsSinceLastShot = 0;
                        FireBullet(new Vector3(12.5f * facingDirection, (Random.value-0.5f)*0.8f, 0), 3);
                    }
                    break;
            }
        }

        if(Input.GetButtonDown("Change Weapon"))
        {
            weaponSelected = ++weaponSelected % 3;
            gunSelectionImg.rectTransform.anchoredPosition = new Vector3(100*weaponSelected-260, 0, 0);
        }
    }

    void LateUpdate()
    {
        if(Input.GetButton("Jump") && canGlide)
        {
            charControl.velocity = new Vector2(charControl.velocity.x, Mathf.Max(charControl.velocity.y, -0.25f));
        }
    }

    void FireBullet(Vector3 direction, float damage)
    {
        BulletScript newBullet = GameObject.Instantiate(bulletType, transform.position + new Vector3(0, 0, 1), transform.rotation).GetComponent<BulletScript>();
        newBullet.speed = direction;
        newBullet.dmgAmount = damage;
        newBullet.tagsToIgnore.Add(gameObject.tag);
    }

    public void HurtPlayer(float amount)
    {
        if (isInvincible)
            return;

        hp.ChangeHealth(-1 * amount);

        //Update HP bar fill (100%-0% based on percentage of max health) and color (green at 100%, yellow at 50%, red at 0%)
        hpBar.fillAmount = hp.Health / hp.MaxHealth;
        hpBar.color = new Color((1 - Mathf.Max(0f, hpBar.fillAmount * 2 - 1)) * 0.8f, Mathf.Min(1f, hpBar.fillAmount * 2) * 0.8f, 0);
    }
}