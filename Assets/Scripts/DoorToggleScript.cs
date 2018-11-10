using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorToggleScript : MonoBehaviour
{
    [SerializeField]
    private GameObject leftDoor, rightDoor;
    [SerializeField]
    private float secondsUntilDoorCloses;
    [SerializeField]
    private bool currentlyUsingLeftDoor;

    private float leftDoorPos, leftDoorScale,
                rightDoorPos, rightDoorScale,
                doorProgress;

    void Start()
    {
        leftDoorPos = leftDoor.transform.position.y;
        leftDoorScale = leftDoor.transform.localScale.y;
        rightDoorPos = rightDoor.transform.position.y;
        rightDoorScale = rightDoor.transform.localScale.y;
    }

    // Update is called once per frame
    void Update ()
    {
        doorProgress = Mathf.Min(doorProgress + Time.deltaTime / secondsUntilDoorCloses, 1);
        leftDoor.transform.localScale = new Vector3(leftDoor.transform.localScale.x,
                                                                leftDoorScale * (currentlyUsingLeftDoor ? 1 - doorProgress : doorProgress),
                                                                leftDoor.transform.localScale.z);
        leftDoor.transform.position = new Vector3(leftDoor.transform.position.x,
                                                                leftDoorPos + (leftDoorScale - leftDoor.transform.localScale.y) / 2,
                                                                leftDoor.transform.position.z);
        rightDoor.transform.localScale = new Vector3(rightDoor.transform.localScale.x,
                                                                rightDoorScale * (currentlyUsingLeftDoor ? doorProgress : 1 - doorProgress),
                                                                rightDoor.transform.localScale.z);
        rightDoor.transform.position = new Vector3(rightDoor.transform.position.x,
                                                                rightDoorPos + (rightDoorScale - rightDoor.transform.localScale.y) / 2,
                                                                rightDoor.transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            currentlyUsingLeftDoor = !currentlyUsingLeftDoor;
            doorProgress = 0;
        }
    }
}
