using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public abstract class Npc : MonoBehaviour
{


    public abstract void Interact();
    //�߻�ȭ�� ���� ��� npc�� ��ȣ�ۿ��� ����

    public virtual void NpcWork()
    {

    }
    //������ ��� npc�� ������ ���� ������ ����

    //�߿��� �������� �������̽��� �����൵ ����.
}
