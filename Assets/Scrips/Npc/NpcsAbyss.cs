using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcsAbyss : Npc
{
    string name = "나락이";
    string[] talkLog = 
    { 
        "어서오게 젊은이 투기장을 이용하러 온겐가?",
        "잘 알겠네 문이 열렸으니 들어가도 좋아",
        "그렇구만 언제든 준비가 되면 돌아오게나",
        "무슨 소린가 자넨 이미 선택을 마쳤네"
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
        Debug.Log(isOpenDoor + "   npc가 받은 신호");
        door.OpenDoor(isOpenDoor);

    }



}
