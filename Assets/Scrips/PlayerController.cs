using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // �����̴� �ӵ�
    [SerializeField] private float moveSpeed;
    // ����
    [SerializeField] private float jumpPower;

    private Rigidbody2D rigid;
    private Vector2 InputVec;
    private float inputX;

    // <���� ó��>
    // ������ �ߴ��� �Ǵ��ϱ� ���� bool Ÿ�� ����
    private bool isJumped;
    // ���� ��Ҵ����� �Ǵ��ϱ� ���� bool Ÿ�� ����
    private bool isGrounded;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>(); 
    }

    private void Update()
    {
        PlayerInput();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJumped = true;
        }
    }

    private void FixedUpdate()
    {
        PlayerMove();

        if (isJumped && isGrounded)
        {
            PlayerJump();
        }
    }

    private void PlayerInput()
    {
        inputX = Input.GetAxis("Horizontal");
    }

    private void PlayerMove()
    {
        rigid.velocity = new Vector2(inputX * moveSpeed, rigid.velocity.y);
    }

    private void PlayerJump()
    {
        rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        isJumped = false;
        isGrounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("�浹");
            isGrounded = true;
        }
    }
}
