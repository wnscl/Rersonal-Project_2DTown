using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; private set; }
    private GameObject scanObject;
    private Npc currentNpc;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 중복 방지
            return;
        }

        Instance = this;
    }

    public void CheckInteractTarget(GameObject target)
    {
        scanObject = target;
    }

    public void CheckPlayerTryInteract()
    {
        if(scanObject == null) return;

        if (scanObject != null)
        {
            currentNpc = scanObject.GetComponent<Npc>();
            currentNpc.Interact();
        }
    }

    public void DoYourWork()
    {
        if (scanObject == null) return;

        if (scanObject != null)
        {
            currentNpc.NpcWork();
        }
    }


}
