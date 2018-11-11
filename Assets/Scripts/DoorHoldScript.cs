using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHoldScript : MonoBehaviour
{
    [SerializeField]
    private GameObject door;
    [SerializeField]
    private bool shouldDoorBeOpen = true;
    [SerializeField]
    private float secondsUntilDoorOpens;

    private float doorScale, doorPos;
    private float doorProgress = 0;

    void Start()
    {
        doorScale = door.transform.localScale.y;
        doorPos = door.transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        doorProgress = Mathf.Clamp(doorProgress+(shouldDoorBeOpen ? -1 : 1)*Time.deltaTime/secondsUntilDoorOpens, 0, 1);

        door.transform.localScale = new Vector3(door.transform.localScale.x, doorScale * doorProgress, door.transform.localScale.y);
        door.transform.localPosition = new Vector3(door.transform.localPosition.x, doorPos + (doorScale * (1 - doorProgress) / 2), door.transform.localPosition.z);
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            shouldDoorBeOpen = !shouldDoorBeOpen;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            shouldDoorBeOpen = !shouldDoorBeOpen;
        }
    }
}
