using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Timeline;
using UnityEngine;

public enum TrapType
{
    SmallSize = 0,
    NormalSize = 1,
    LargeSize
}

public class Trap : MonoBehaviour
{
    public TrapType type;

    public int trapDamage;
    public float animationDuration = 1f;
    private float frameTimer = 0f;
    private bool isPlayerInTriggerRange = false;

    Player player;

    public void TrapSetting()
    {
        switch (type)
        {
            case TrapType.SmallSize:
                transform.localScale = new Vector3(1f,1f, 1f);
                break;

            case TrapType.NormalSize:
                transform.localScale = new Vector3(1.5f, 1.5f, 1f);
                break;

            case TrapType.LargeSize:
                transform.localScale = new Vector3(2f, 2f, 1f);
                break;
        }
    }


    private void Update()
    {
        frameTimer += Time.deltaTime;

        int currentFrame = Mathf.FloorToInt((frameTimer / animationDuration) * 12);
    
        if (currentFrame >= 8 && currentFrame <= 12)
        {
            if (isPlayerInRange())
            {
                DamageToPlayer();
            }
            
        }
        if (frameTimer >= animationDuration)
        {
            frameTimer = 0f;
        }
    
    }

    public void DamageToPlayer()
    {
        if (player != null)
        {
            player.TakeDamage(trapDamage);
        }
    }

    private bool isPlayerInRange()
    {
        return isPlayerInTriggerRange;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.GetComponent<Player>();
            isPlayerInTriggerRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInTriggerRange = false;
            player = null;
        }
    }

}
