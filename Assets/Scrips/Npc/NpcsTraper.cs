using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcsTraper : Npc
{
    string name = "함정이";
    string[] talkLog =
    {
        "현재 투기장은 임시 휴업이라네 하지만 함정 생존 게임은 준비되어있지" 
        + "자네 도전해 볼 생각이 있나?",
        "자네의 용기를 높이 사도록하지 7초 후 게임은 시작된다네",
        "???...자네 간이 개미보다도 작구만",
        "무슨 소린가 자넨 이미 선택을 마쳤네"
    };

    bool isGameStart = false;
    bool isGameStop = false;

    [SerializeField] Door door;
    [SerializeField] Sprite sprite;
    [SerializeField] SpriteRenderer spriteRen;
    [SerializeField] Collider2D col;

    public override void Interact()
    {
        UiManager.Instance.ShowNpcBasicUi(sprite, name, talkLog, isGameStart);
    }

    public override void NpcWork()
    {
        if (!isGameStart && !isGameStop)
        {
            ReadyForTrapGame();
        }
    }

    public void NpcHide()
    {
        spriteRen.enabled = false;
        col.enabled = false;
    }
    public void NpcApper()
    {
        spriteRen.enabled = true;
        col.enabled = true;
    }

    public void ReadyForTrapGame()
    {
        StartCoroutine(StartTrapGame());
    }
    public IEnumerator StartTrapGame()
    {
        yield return new WaitForSeconds(2f);
        NpcHide();
        yield return new WaitForSeconds(5f);
        TrapManager.Instance.GameStart();
    }


    public void OpenTheDoor()
    {
        isGameStart = UiManager.Instance.SendNpcSign();
        Debug.Log(isGameStart + "   npc가 받은 신호");
        door.OpenDoor(isGameStart);

    }

}
