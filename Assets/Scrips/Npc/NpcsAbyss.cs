using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcsAbyss : Npc
{
    string name = "������";
    string[] talkLog = 
    { 
        "����� ������ �������� �̿��Ϸ� �°հ�?",
        "�� �˰ڳ� ���� �������� ���� ����",
        "�׷����� ������ �غ� �Ǹ� ���ƿ��Գ�",
        "���� �Ҹ��� �ڳ� �̹� ������ ���Ƴ�"
    };

    bool isOpenDoor = false;

    [SerializeField] Door door;
    [SerializeField] Sprite sprite;

    public override void Interact()
    {
        UiManager.Instance.ShowNpcBasicUi(sprite, name, talkLog, isOpenDoor);
    }

    public override void NpcWork()
    {
        OpenTheDoor();
    }

    public void OpenTheDoor()
    {
        isOpenDoor = UiManager.Instance.SendNpcSign();
        Debug.Log(isOpenDoor + "   npc�� ���� ��ȣ");
        door.OpenDoor(isOpenDoor);

    }



}
