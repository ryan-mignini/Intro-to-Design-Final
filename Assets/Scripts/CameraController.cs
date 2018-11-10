using UnityEngine;
using System;

public class CameraController : MonoBehaviour
{
    private Camera cam;
    private GameObject target;
    private Rigidbody2D targetPhysics;
    private float targetSize;

    [SerializeField]
    private float smoothAmount;

    [SerializeField]
    private float leadDistance;

    [SerializeField]
    private bool bounds;

    [SerializeField]
    private Vector2 minCameraPos;
    [SerializeField]
    private Vector2 maxCameraPos;


    private void Start()
    {
        cam = GetComponent<Camera>();
        targetSize = cam.orthographicSize;
        target = GameObject.FindGameObjectWithTag("Player");
        targetPhysics = target.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 targetPosition = new Vector3(Mathf.Clamp(target.transform.position.x + Math.Sign(targetPhysics.velocity.x/*target.transform.localScale.x*/) * leadDistance, minCameraPos.x, maxCameraPos.x),
                                    Mathf.Clamp(target.transform.position.y, minCameraPos.y, maxCameraPos.y),
                                    -10f);
        
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothAmount * Time.deltaTime);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, smoothAmount * Time.deltaTime);
    }

    public void ChangeValues(Vector2 minBound, Vector2 maxBound, float scale)
    {
        minCameraPos = minBound;
        maxCameraPos = maxBound;
        targetSize = scale;
    }
}
