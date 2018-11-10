using UnityEngine;

[System.Serializable]
public class TransitionData
{
    public float cameraSize;
    public Vector2 minBounds;
    public Vector2 maxBounds;
    public float gravityScale;
}

public class AreaTransitionScript : MonoBehaviour
{
    [SerializeField]
    private TransitionData leftData;
    [SerializeField]
    private TransitionData rightData;

    [SerializeField]
    private bool currentlyUsingLeftValues;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            ChangeValues(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>(),
                        col.gameObject.GetComponent<Rigidbody2D>(),
                        (currentlyUsingLeftValues ? rightData : leftData));
            currentlyUsingLeftValues = !currentlyUsingLeftValues;
        }
    }

    void ChangeValues(CameraController cam, Rigidbody2D playerPhysics, TransitionData newVals)
    {
        cam.ChangeValues(newVals.minBounds, newVals.maxBounds, newVals.cameraSize);
        playerPhysics.gravityScale = newVals.gravityScale;
    }
}
