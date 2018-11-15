using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLevelOnTouchScript : MonoBehaviour
{
    [SerializeField]
    private int levelIndex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(levelIndex);
        }
    }
}
