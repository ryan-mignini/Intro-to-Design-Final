using UnityEngine;

[System.Serializable]
public class TransitionData
{
    public float cameraSize;
    public Vector2 minBounds;
    public Vector2 maxBounds;
    public float gravityScale;
}

public class CameraTransitionScript : MonoBehaviour
{
    [SerializeField]
    private TransitionData[] transitions;

    private int currentSetting = 0;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            currentSetting = ++currentSetting % transitions.Length;

            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>().ChangeValues(transitions[currentSetting].minBounds,
                                                                                                        transitions[currentSetting].maxBounds,
                                                                                                        transitions[currentSetting].cameraSize);
            col.gameObject.GetComponent<Rigidbody2D>().gravityScale = transitions[currentSetting].gravityScale;
        }
    }
}
