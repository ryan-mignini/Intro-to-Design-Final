using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float dmgAmount;
    public Vector2 speed;
    public List<string> tagsToIgnore;

    void Start()
    {
        Destroy(gameObject, 3);
    }

    // Update is called once per frame
    void Update ()
    {
        transform.position += (Vector3)speed * Time.deltaTime;
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if(tagsToIgnore.Contains(col.gameObject.tag) || col.isTrigger)
        {
            return;
        }
        
        if(col.CompareTag("Player"))
        {
            col.gameObject.GetComponent<PlayerControllerScript>().HurtPlayer(dmgAmount);
        }
        else
        {
            EntityHealth health = col.gameObject.GetComponent<EntityHealth>();
            if (health != null)
            {
                health.ChangeHealth(-1 * dmgAmount);
            }
        }
        
        Destroy(gameObject);
    }
}
