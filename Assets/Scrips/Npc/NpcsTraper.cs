using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcsTraper : Npc
{
    string name = "������";
    string[] talkLog =
    {
        "���� �������� �ӽ� �޾��̶�� ������ ���� ���� ������ �غ�Ǿ�����" 
        + "�ڳ� ������ �� ������ �ֳ�?",
        "�ڳ��� ��⸦ ���� �絵������ 7�� �� ������ ���۵ȴٳ�",
        "???...�ڳ� ���� ���̺��ٵ� �۱���",
        "���� �Ҹ��� �ڳ� �̹� ������ ���Ƴ�"
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
        Debug.Log(isGameStart + "   npc�� ���� ��ȣ");
        door.OpenDoor(isGameStart);

    }

}
