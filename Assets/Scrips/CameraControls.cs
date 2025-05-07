using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public GameObject player;
    public Transform target;
    private float followSpeed = 0.9f;
    //ī�޶� �ӵ��� 0.1�� �ִϱ� ���ƽ�� ����� �����ϱ� ���� �ӵ�����

    private void Awake()
    {
        player = GameObject.Find("Player");
        target = player.transform;

    }

    void Start()
    {

    }


    void Update()
    {
        //FollowPlayer();
    }

    private void LateUpdate()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        Vector3 camPosition = new Vector3(
            transform.position.x, 
            transform.position.y, 
            -10);

        Vector3 targetPosition = new Vector3(
            target.position.x,
            target.position.y,
            -10);

        float t = followSpeed * Time.deltaTime;
        transform.position = Vector3.Lerp(camPosition, targetPosition, t);
        //������ �ε巴�� �ϱ� ���� ����ϴ� ��찡 �Ϲ����ε�
        //t�� 0.5�̷� ���� �׳� ������ a���� b���� ���� �׾��� �� 50�ۼ�Ʈ������ �����̴� ���� ������
        //ī�޶�� �÷��̾ ������ �� ���� �����δٸ� �׸��� �÷��̾ �����̴� ����
        //������Ʈ, �Ƚ� ������Ʈ�� ������ٵ� ���ؼ� �� �����Ӵ����� �����δٸ�
        //ī�޶�� ��û���� ���󰡴� 50����ä��� ���߰� ���󰡴� 50���� ä��� ���߰�
        //�̷� ����� �Ǿ������ ȭ���� �������� �Ҷ� ����� �� ó�� ����
        //�׷��� t���� �ۼ�Ʈ�� x ��ŸŸ���� �ϸ� �� ������ ���� �� ����
        //ī�޶� �ε巴�� �÷��̾��� ��ġ�� �÷��̾�� ������ �� ���� ������ 

        //t = ������ (��: 0.1)	�׻� 10%�� �̵� �� b�� ���� �� �������� �� ���� ����
        //t = speed * deltaTime   �ð� �������� �󸶳� �̵������� ��� �� �ڿ������� ����
        //�� ��������� **� �����ӿ����� ���ð� �������� ������ �ӵ���**�� ��

        //�׷��� ���� ��� �Լ�(Lerp)�� �ð��� �̵��ϰ� �Ǵ� �Ÿ���� �Լ�ó�� �ٲ�

    }

}
