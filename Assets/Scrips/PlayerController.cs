using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 움직이는 속도
    [SerializeField] private float moveSpeed;
    // 점프
    [SerializeField] private float jumpPower;

    private Rigidbody2D rigid;
    private Vector2 InputVec;
    private float inputX;

    // <예외 처리>
    // 점프를 했는지 판단하기 위한 bool 타입 변수
    private bool isJumped;
    // 땅에 닿았는지를 판단하기 위한 bool 타입 변수
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
            Debug.Log("충돌");
            isGrounded = true;
        }
    }
}
