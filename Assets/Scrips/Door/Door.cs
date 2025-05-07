using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Door : MonoBehaviour
{

    [SerializeField] private SpriteRenderer DoorRenderer;
    [SerializeField] private Collider2D Collider;


    public void OpenDoor(bool isOpen)
    {
        if (isOpen)
        {
            DoorRenderer.enabled = !isOpen;
            Collider.isTrigger = isOpen;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        DoorRenderer.enabled = true;
        Collider.isTrigger = false;
    }
}
