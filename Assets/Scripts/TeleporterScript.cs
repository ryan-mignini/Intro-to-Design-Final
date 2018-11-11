using System.Collections;
using UnityEngine;

public class TeleporterScript : MonoBehaviour
{
    [SerializeField]
    private Vector2 teleportDestination;
    [SerializeField]
    private float secondsBeforeTeleport;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            StartCoroutine(TeleportObject(col.gameObject));
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            StopAllCoroutines();
        }
    }
    
    private IEnumerator TeleportObject(GameObject thingToTeleport)
    {
        yield return new WaitForSeconds(secondsBeforeTeleport);
        thingToTeleport.transform.position = teleportDestination;
    }
}
