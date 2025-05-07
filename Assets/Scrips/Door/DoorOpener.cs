using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    public Door door;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        door.OpenDoor(true);
    }


}
