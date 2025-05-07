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
        ���� �ֱ�

        rigid.velocity(inputVec);
        �ӵ�����

        rigid.MovePosition(rigid.position + inputVec);
        ��ġ�̵� ü������ ���ٰ� ����������
        ������������ ��ġ�� �̵���Ű�� �޼��� => ������ġ�� �����ϴ� �Լ�
        inpuVec = �Է��� ���� ��� ���̴� ������(inputVec)�� ���ԵǸ�
        �Է��� �� ���� ���� -1 ~0 ~1������ �ִ�ġ�̰� Ű�� ������ ���ԵǸ�
        �� ���� �ٽ� 0 �� ������ ��Ӿ�򰡷� �̵��Ѵٴ� ������ �����ϱ�� �����.
        �� �Է� x 0.7 y 0.4->������->�ٽ� �Է� x 0.7 y 0.4 �� ��������
        .MovePosition��(inputVec)�� �ְԵǸ� �׳� ��� �Է��� ���� ��ġ�� �����ع�����.
        ��ġ���� ���̴� ������ �ƴϰ� �ȴ�.*/

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
        //magnitude�� ���Ϳ� ��������μ� ���⺤���� ���̸� ���� �� �ִ�
        //�� ��� ���⺤���� ���̸� ����� ĳ���Ͱ� ������ �� �� ������ ���� ����
        //��? ���⺤���� ���� 1�̱� ������ ������ �Է��ϴ� �������� �� ���ǹ��� ���
        //�Է��� ���ٸ� 0�̱� ������ else���� ����
        //�� �Է��ߴ��� ���ߴ��ĸ� �Ǻ��ع���
        {
            anim.SetBool("isMove", true);
            rigid.velocity = (direction * moveSpeed);
            //���ν�Ƽ�� �ʼ� �� ��ü�̱� ������ ��ŸŸ���� �����ָ� �ȵȴ�.
        }
        else
        {
            anim.SetBool("isMove", false);
            rigid.velocity = Vector2.zero;
        }

    }

    public void PlayerJump()
    {
        //Debug.Log("���� ����!");
        //rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        //Debug.Log("���� �ӵ�: " + rigid.velocity);
        //ForceMode2D�� ���������� Ȥ�� �ε巴�� ������� ���ϴ� 
        //�� ���� � ������� ������ ���ϴ� �ɼ�
        //.force�� ���������� ������ �� - ���������� �ۿ� (�����Ӹ��� ����)
        //.impulse�� ���������� Ƣ�� �� - �� �ѹ� ���������� �ۿ�
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
        //������ �����ð� ���� ������Ÿ�̸Ӹ� �̿��� ���� ���� ������Ÿ�̸��� �ð���
        //�����ð��� �ʰ��Ҷ� ���� �ڷ�ƾ���� �����Ӵ��� ������ ������ ��
        {
            frameTimer += Time.deltaTime;
            //������Ÿ�̸ӿ� �ð��� ����
            //��ŸŸ�� �����Ӱ� ������ ���̽ð�

            anim.SetBool("isJump", true);

            float t = frameTimer / jumpDuration;

            float yOffset = Mathf.Sin(t * Mathf.PI) * jumpHeight;
            //���� ��� ���ֱⰡ �ƴ϶� �� �ֱ⸦ ����Ͽ�(float t = frameTimer / jumpDuration;)
            //�������� ��� ������
            //t = frameTimer / jumpDuration�̰� ����� ������ ������ ����Ǹ�
            //���ϴ� �ð����� �Ǵµ� ����Ǵ� �ð��� ������Ÿ�̸ӷ� �����
            //0% 100%������ ����� �� �׸��� �װ��� ���̿� ���ؼ�
            //���� ��� �º��� ��� �ð�������� ����Ǵ� ���̱⿡
            //���� �����õ���� �ɰ��� ���̰� �װ��� �� �κ��� %�� �� �� �ִ�.
            Vector2 currentPos = transform.position;
            //���� ��ǥ�� �־��� �ڽ��� ���� ���� �÷��̾��� �Է¿� ����
            //���� ��ġ�� �����Ӵ����� ��� �����̵Ǿ
            jumpPos = new Vector2(currentPos.x, startPos.y + yOffset);
            //x��ǥ�� ������ǥ(�̵��ϴ� �Ǵ� ������ �ִ�) y��ǥ�� ���� ������ + �������θ��� ������ y��ǥ�� �̿�
            //���� ��ǥ�� �����.
            transform.position = jumpPos;
            //�ڷ�ƾ���� �����Ӵ����� �÷��̾ �� ��ǥ�� �̵��ϰ� �������.

            //�� ������ �̵��� �Ǵ� ����   transform.position = jumpPos; �� ����
            //���� ������ ó���ϱ� ���� �ʿ��� ���� �۾��� �������ڵ� ����

            yield return null; // ���� �����ӱ��� ��ٸ���
            //yield return �� �� �������� ���߰� ���� �����ӿ� �ٽ� �����̶�� ����
            //�ڷ�ƾ�Լ��� ������ �ɰ��� �� �� ����
            //��Ȯ�� ���ϸ� �Լ� ��ü�� �ɰ��� ��� ����
                //Debug.Log("A"); �̰� ���� ������
                //yield return null;  <- ���⼭ ��� ����
                //Debug.Log("B"); �׸��� �̰�
            //�� �Լ���ü�� �ɰ��⿡ �װ��� yield return���� ���� �����ӿ� �ٽ� ����
            //���Ѽ� �����Ӵ����� ���డ�� 
        }

        transform.position = jumpPos; // ��Ȯ�� ���� ��ġ ����
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
        //���⼭ �̹� �Է��� ���� ����ȭ�ؼ� ������ ��
        //�׷��⿡ �밢�� �̵��� ���� ����ó���� ���� �ʾƵ� ��

        //Vector2 moveWithDirectionAndSpeed = inputVec * moveSpeed * Time.deltaTime;
        //���ؿ� ���⺤�� �� �����ӹ����� moveSpeed(�̵��ӵ�)�� ���ؼ� 
        //������ ����� ũ�⸦ �� �Ը���� �����.
        // -> �ᱹ ���ν�Ƽ�� ���� ������ٵ��� �����ý����� �̿��Ͽ� �����̴� ��������
        // ���ν�Ƽ�� ������ �ϴ� ���� ������ִ� ���̴�.

        //rigid.velocity = moveWithDirectionAndSpeed;
        //velocity = ����x�ӵ� �� ���⼺�� �ӵ��� ũ�⸦ ��� ���� ����
        //������ ����,ũ�� ���͸� ���� �־��ָ� �̵� �ý���

    }

    public void OnLook(InputAction.CallbackContext context) //InputValue value)
    {
        Vector2 mousePos = context.ReadValue<Vector2>(); //value.Get<Vector2>();
        Vector3 lookDirection3D = new Vector3(mousePos.x, mousePos.y, 10f);
        lookDirection = mainCam.ScreenToWorldPoint(lookDirection3D);

    }

    public void OnJump(InputAction.CallbackContext context)
    //����Ƽ�Է¿��� ��ư����� �������� �� ���� ����Ű�� ���� �󸶳� ������ ����
    //���ʰ� �������� ���� ������ �Լ��� ��Ƽ� �����ִ� ���� CallbackContext
    {
        if (context.performed)
        //.performed - �Է��� ��Ȯ�� ����� ������ (���� ����)
        //context.started - �Է��� ���۵� �������� (���������)
        //context.canceled	- �Է��� ��ҵ� ���� (��ư�� ���� ��)
        //context.ReadValue<T>() - �Էµ� ���� �� �б� ����2 �÷� �� ��
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
            Debug.Log("��ȣ�ۿ� Ű ���");
            InteractionManager.Instance.CheckPlayerTryInteract();
        }
    }

}
