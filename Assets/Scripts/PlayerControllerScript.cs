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
    public bool CanGlide;
    [SerializeField]
    private bool[] weaponsOwned;

    private Rigidbody2D charControl;
    private EntityHealth hp;
    private float facingDirection = 1;
    private float secondsSinceLastShot = 999;
    private int weaponSelected = -1;
    private bool isInvincible = false;

    private Animator charAnim;

    // Use this for initialization
    void Start()
    {
        charControl = GetComponent<Rigidbody2D>();
        charAnim = GetComponent<Animator>();
        hp = GetComponent<EntityHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        //Movement
        charControl.velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, charControl.velocity.y);
        if (Input.GetButton("Horizontal"))
        {
            facingDirection = Mathf.Sign(Input.GetAxisRaw("Horizontal"));
            gameObject.transform.localScale = new Vector3(Mathf.Abs(gameObject.transform.localScale.x)*facingDirection, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        }
        
        //Jumping
        if (Input.GetButtonDown("Jump"))
        {
            if (groundCheckTrigger.OverlapCollider(new ContactFilter2D(), new Collider2D[2]) > 1)
            {
                charControl.AddForce(new Vector2(0, jumpForce));
            }
        }

        //Shooting
        secondsSinceLastShot += Time.deltaTime;
        if (Input.GetButtonDown("Fire"))
        {
            switch (weaponSelected)
            {
                case 0: //Pistol
                    secondsSinceLastShot = 0;
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
                default:
                    secondsSinceLastShot = 0;
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
                        FireBullet(new Vector3(12.5f * facingDirection, (Random.value-0.5f)*0.8f, 0), 5);
                    }
                    break;
            }
        }

        //Weapon Cycling
        if(Input.GetButtonDown("Change Weapon"))
        {
            if (weaponsOwned.Length > 0)
            {
                //Cycling selection var to index of next owned weapon
                for (int i = 0; i < weaponsOwned.Length; i++)
                {
                    weaponSelected = ++weaponSelected % weaponsOwned.Length;
                    if(weaponsOwned[weaponSelected])
                    {
                        break;
                    }
                }

                //Testing to see that we actually own any weapons, and reflecting UI to match
                if(weaponsOwned[weaponSelected])
                {
                    gunSelectionImg.rectTransform.anchoredPosition = new Vector3(100 * weaponSelected - 260, 0, 0);
                }
                else
                {
                    weaponSelected = -1;
                    gunSelectionImg.rectTransform.anchoredPosition = new Vector3(100, 0, 0);
                }
            }
        }

        //Animation
        charAnim.SetBool("isMoving", Input.GetAxisRaw("Horizontal") != 0);
        charAnim.SetBool("isFiring", secondsSinceLastShot < .5f);
    }

    void LateUpdate()
    {
        if(Input.GetButton("Jump") && CanGlide)
        {
            charControl.velocity = new Vector2(charControl.velocity.x, Mathf.Max(charControl.velocity.y, -0.25f));
        }
        switch(weaponSelected)
        {
            case 1:
                gunSelectionImg.fillAmount = secondsSinceLastShot / 1f;
                break;
            case 2:
                gunSelectionImg.fillAmount = secondsSinceLastShot / 0.15f;
                break;
            default:
                gunSelectionImg.fillAmount = 1;
                break;
        }
    }

    void FireBullet(Vector3 direction, float damage)
    {
        BulletScript newBullet = GameObject.Instantiate(bulletType, transform.position + new Vector3(facingDirection*0.3f, 0.25f, 1), transform.rotation).GetComponent<BulletScript>();
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

    public void AddWeapon(int weaponIndex)
    {
        if (weaponIndex < 0 || weaponIndex >= weaponsOwned.Length)
            return;

        weaponsOwned[weaponIndex] = true;
        switch(weaponIndex)
        {
            case 0:
                GameObject.Find("Pistol Icon").GetComponent<Image>().enabled = true;
                break;
            case 1:
                GameObject.Find("Shotgun Icon").GetComponent<Image>().enabled = true;
                break;
            case 2:
                GameObject.Find("Machine Gun Icon").GetComponent<Image>().enabled = true;
                break;
        }

        //Automatically equipping new weapon
        weaponSelected = weaponIndex;
        gunSelectionImg.rectTransform.anchoredPosition = new Vector3(100 * weaponSelected - 260, 0, 0);
    }
}