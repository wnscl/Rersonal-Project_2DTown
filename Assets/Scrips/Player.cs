using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Camera mainCam;

    private Rigidbody2D rigid;

    private SpriteRenderer sprite;

    private Animator anim;

    public GameObject scanObject;

    [Header("Direction")]
    public Vector2 direction = Vector2.zero;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Vector2 lookDirection = Vector2.zero;

    [Header("Stat")]
    [Range(0,100)][SerializeField]private int hp = 100;
    public int Hp
    {
        get => hp;
        set { hp = Mathf.Clamp(value, 0,100);}
    }
    public float MoveSpeed
    { get => moveSpeed; set { Mathf.Clamp(value, 0, 20); } }

    [Header("mode")]
    public float invincibilityTime = 1f;
    private float invincibilityTimer = 0f;


    protected void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
    }

    protected void Start()
    {
        mainCam = Camera.main;

    }

    protected void Update()
    {
        PlayerMove();
        PlayerLook();
        Jumping();

        if (invincibilityTimer > 0f)
        {
            invincibilityTimer -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        /*rigid.AddForce(inputVec);
        힘을 주기

        rigid.velocity(inputVec);
        속도제어

        rigid.MovePosition(rigid.position + inputVec);
        위치이동 체스말을 땟다가 내려놓듯이
        무브포지션은 위치를 이동시키는 메서드 => 절대위치를 설정하는 함수
        inpuVec = 입력을 통해 얻는 값이다 하지만(inputVec)만 놓게되면
        입력할 때 마다 값이 -1 ~0 ~1까지가 최대치이고 키를 눌렀다 때게되면
        그 값이 다시 0 이 됨으로 계속어딘가로 이동한다는 개념을 구현하기는 힘들다.
        즉 입력 x 0.7 y 0.4->움직임->다시 입력 x 0.7 y 0.4 의 과정에서
        .MovePosition은(inputVec)를 넣게되면 그냥 즉시 입력한 값에 위치를 설정해버린다.
        위치값이 쌓이는 구조가 아니게 된다.*/

    }
    public void TakeDamage(int damage)
    {
        if (invincibilityTimer > 0f)
        {
            return;
        }
        else
        {
            hp -= damage;
            invincibilityTimer = invincibilityTime;

            UiManager.Instance.UpdateHpBar(this);
        }

        if (Hp <= 0)
        {
            TrapManager.Instance.GameOver();
        }
    }


    public void PlayerMove()
    {
        if (direction.magnitude > 0.01f)
        //magnitude를 벡터에 사용함으로서 방향벡터의 길이를 얻을 수 있다
        //이 경우 방향벡터의 길이를 사용해 캐릭터가 움직일 때 안 움직일 때를 구분
        //왜? 방향벡터의 값은 1이기 때문에 방향을 입력하는 시점에서 이 조건문이 사용
        //입력이 없다면 0이기 때문에 else문이 사용됨
        //즉 입력했느냐 안했느냐를 판별해버림
        {
            anim.SetBool("isMove", true);
            rigid.velocity = (direction * moveSpeed);
            //벨로시티는 초속 그 자체이기 때문에 델타타임을 곱해주면 안된다.
        }
        else
        {
            anim.SetBool("isMove", false);
            rigid.velocity = Vector2.zero;
        }

    }

    public void PlayerJump()
    {
        //Debug.Log("점프 시작!");
        //rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        //Debug.Log("현재 속도: " + rigid.velocity);
        //ForceMode2D는 순간적으로 혹은 부드럽게 계속인지 정하는 
        //즉 힘을 어떤 방식으로 가할지 정하는 옵션
        //.force는 지속적으로 누르는 힘 - 지속적으로 작용 (프레임마다 쌓임)
        //.impulse는 순간적으로 튀는 힘 - 딱 한번 순간적으로 작용
        if (!anim.GetBool("isJump"))
        {
            StartCoroutine(Jumping());
        }
    }


    private IEnumerator Jumping()
    {
        //isJump = true;

        float jumpHeight = 1.2f;
        float jumpDuration = 0.5f;
        float frameTimer = 0f;

        Vector2 startPos = transform.position;
        Vector2 jumpPos = Vector2.zero;

        while (frameTimer < jumpDuration)
        //정해준 점프시간 동안 프레임타이머를 이용해 내가 만든 프레임타이머의 시간이
        //점프시간을 초과할때 까지 코루틴으로 프레임단위 연출을 만들어내는 것
        {
            frameTimer += Time.deltaTime;
            //프레임타이머에 시간을 축적
            //델타타임 프레임과 프레임 사이시간

            anim.SetBool("isJump", true);

            float t = frameTimer / jumpDuration;

            float yOffset = Mathf.Sin(t * Mathf.PI) * jumpHeight;
            //사인 곡선의 한주기가 아니라 반 주기를 사용하여(float t = frameTimer / jumpDuration;)
            //언덕모양의 곡선을 만들어내고
            //t = frameTimer / jumpDuration이건 진행률 어차피 점프가 진행되면
            //원하는 시간동안 되는데 진행되는 시간을 프레임타이머로 만들어
            //0% 100%까지를 계산한 것 그리고 그것을 파이와 곱해서
            //사인 곡선이 좌부터 우로 시계방향으로 진행되는 것이기에
            //원을 수백수천개로 쪼개서 파이가 그것의 각 부분을 %로 볼 수 있다.
            Vector2 currentPos = transform.position;
            //현재 좌표를 넣어줄 박스를 만든 다음 플레이어의 입력에 따라
            //현재 위치가 프레임단위로 계속 저장이되어서
            jumpPos = new Vector2(currentPos.x, startPos.y + yOffset);
            //x좌표는 현재좌표(이동하는 또는 가만히 있는) y좌표는 점프 시작점 + 사인으로만든 곡선모양의 y좌표를 이용
            //점프 좌표를 만든다.
            transform.position = jumpPos;
            //코루틴으로 프레임단위로 플레이어가 그 좌표에 이동하게 만들었다.

            //즉 실제로 이동이 되는 것은   transform.position = jumpPos; 이 구문
            //위의 구문을 처리하기 위해 필요한 사전 작업은 나머지코드 전부

            yield return null; // 다음 프레임까지 기다리기
            //yield return 은 이 지점에서 멈추고 다음 프레임에 다시 시작이라는 구문
            //코루틴함수는 실행을 쪼개서 할 수 잇음
            //정확히 말하면 함수 자체를 쪼개서 사용 가능
                //Debug.Log("A"); 이거 먼저 나오고
                //yield return null;  <- 여기서 잠시 멈춤
                //Debug.Log("B"); 그리고 이거
            //즉 함수자체를 쪼개기에 그것을 yield return으로 다음 프레임에 다시 실행
            //시켜서 프레임단위로 실행가능 
        }

        transform.position = jumpPos; // 정확히 착지 위치 보정
        anim.SetBool("isJump", false);
    }


    public void PlayerLook()
    {
        if (transform.position.x < lookDirection.x)
        {
            sprite.flipX = false;
        }
        else
        {
            sprite.flipX = true;
        }
    }

    public void OnMove(InputAction.CallbackContext context) //InputValue value)
    {
        direction = context.ReadValue<Vector2>();
        //direction = value.Get<Vector2>();
        //여기서 이미 입력한 값을 정규화해서 가지고 옴
        //그렇기에 대각선 이동에 관한 예외처리를 하지 않아도 됨

        //Vector2 moveWithDirectionAndSpeed = inputVec * moveSpeed * Time.deltaTime;
        //구해온 방향벡터 즉 움직임방향을 moveSpeed(이동속도)와 곱해서 
        //벡터의 방향과 크기를 내 입맛대로 만든다.
        // -> 결국 벨로시티를 통해 리지드바디의 물리시스템을 이용하여 움직이는 것임으로
        // 벨로시티가 가져야 하는 값을 만들어주는 것이다.

        //rigid.velocity = moveWithDirectionAndSpeed;
        //velocity = 방향x속도 즉 방향성과 속도의 크기를 모두 갖는 벡터
        //가져온 방향,크기 벡터를 집어 넣어주면 이동 시스템

    }

    public void OnLook(InputAction.CallbackContext context) //InputValue value)
    {
        Vector2 mousePos = context.ReadValue<Vector2>(); //value.Get<Vector2>();
        Vector3 lookDirection3D = new Vector3(mousePos.x, mousePos.y, 10f);
        lookDirection = mainCam.ScreenToWorldPoint(lookDirection3D);

    }

    public void OnJump(InputAction.CallbackContext context)
    //유니티입력에서 버튼방식을 선택했을 때 내가 누른키가 뭐고 얼마나 누르고 땟고
    //몇초간 눌럿는지 같은 정보를 함수로 담아서 보내주는 것이 CallbackContext
    {
        if (context.performed)
        //.performed - 입력이 정확히 실행된 시점에 (누른 순간)
        //context.started - 입력이 시작된 순간부터 (누르기시작)
        //context.canceled	- 입력이 취소된 순간 (버튼을 땟을 때)
        //context.ReadValue<T>() - 입력된 실제 값 읽기 벡터2 플롯 불 등
        {
            PlayerJump();
        }
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.TryGetComponent<Npc>(out var npc))
        {
            scanObject = target.gameObject;
            InteractionManager.Instance.CheckInteractTarget(scanObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        scanObject = null;
        UiManager.Instance.HideNpcUi();
    }

    public void OnInteractionStart(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("상호작용 키 사용");
            InteractionManager.Instance.CheckPlayerTryInteract();
        }
    }

}
