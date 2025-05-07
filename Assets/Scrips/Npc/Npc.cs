using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public abstract class Npc : MonoBehaviour
{


    public abstract void Interact();
    //추상화의 이유 모든 npc는 상호작용이 가능

    public virtual void NpcWork()
    {

    }
    //하지만 모든 npc가 고유한 일을 하지는 않음

    //중요한 공통점은 인터페이스로 묶어줘도 좋다.
}
