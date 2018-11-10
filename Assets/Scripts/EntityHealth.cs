using UnityEngine;
using UnityEngine.SceneManagement;

public class EntityHealth : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;

    public float Health { get; private set; }
    public float MaxHealth
    {
        get { return maxHealth; }
    }
    
    void Start()
    {
        Health = maxHealth;
    }

    public void SetHealth(float val)
    {
        Health = val;
    }

    public void ChangeHealth(float delta)
    {
        Health = Mathf.Clamp(Health + delta, 0f, maxHealth);

        if(Health == 0)
        {
            if (gameObject.CompareTag("Player"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
