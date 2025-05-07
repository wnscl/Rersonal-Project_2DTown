using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public GameObject player;
    public Transform target;
    private float followSpeed = 0.9f;
    //카메라 속도를 0.1로 주니까 드라마틱한 장면을 연출하기 좋은 속도였으

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
        //레프는 부드럽게 하기 위해 사용하는 경우가 일반적인데
        //t에 0.5이런 값을 그냥 넣으면 a부터 b까지 선을 그었을 때 50퍼센트까지만 움직이는 것이 맞지만
        //카메라는 플레이어가 움직일 때 따라 움직인다면 그리고 플레이어가 움직이는 것이
        //업데이트, 픽스 업데이트에 리지드바디를 통해서 즉 프레임단위로 움직인다면
        //카메라는 엄청빨리 따라가다 50프로채우고 멈추고 따라가다 50프로 채우고 멈추고
        //이런 방식이 되어버려서 화면이 눈아프게 뚝뚝 끊기는 것 처럼 보임
        //그러나 t값에 퍼센트값 x 델타타임을 하면 그 현상을 없앨 수 있음
        //카메라가 부드럽게 플레이어의 위치를 플레이어에게 도달할 때 까지 추적함 

        //t = 고정값 (예: 0.1)	항상 10%씩 이동 → b를 절대 못 따라잡음 → 끊겨 보임
        //t = speed * deltaTime   시간 기준으로 얼마나 이동할지를 계산 → 자연스럽게 도달
        //→ 결과적으로 **어떤 프레임에서도 “시간 기준으로 일정한 속도”**가 됨

        //그래서 비율 기반 함수(Lerp)가 시간당 이동하게 되는 거리기반 함수처럼 바뀜

    }

}
