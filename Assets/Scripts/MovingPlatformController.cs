using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
    [SerializeField]
    private List<Vector3> positions;
    [SerializeField]
    private float secondsPerPosition;

    private float elapsedSeconds;

    // Update is called once per frame
    void FixedUpdate()
    {
        elapsedSeconds = (elapsedSeconds + Time.deltaTime) % (secondsPerPosition * positions.Count);

        int mostRecentPosIndex = (int)(elapsedSeconds / secondsPerPosition);

        transform.position = Vector3.Lerp(positions[mostRecentPosIndex],
                                            positions[(mostRecentPosIndex + 1)%positions.Count],
                                            (elapsedSeconds - mostRecentPosIndex * secondsPerPosition) / secondsPerPosition);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.parent = transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.parent = null;
        }
    }
}
