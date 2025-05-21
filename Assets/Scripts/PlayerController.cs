using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    // 움직이는 속도
    [SerializeField] private float _moveSpeed;
    // 점프
    [SerializeField] private float _jumpPower;

    // 플레이어가 반대 방향을 바라볼 때, 카메라도 이동하게 하는 법
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    private Animator _animator;

    private Rigidbody2D _rigid;

    // 좌우 변경을 위한 SpriteRenderer
    private SpriteRenderer _spriteRenderer;
    private Vector2 InputVec;
    private float _inputX;

    // <예외 처리>
    // 점프를 했는지 판단하기 위한 bool 타입 변수
    private bool _isJumped;
    // 땅에 닿았는지를 판단하기 위한 bool 타입 변수
    private bool _isGrounded;

    private CinemachineFramingTransposer _cinemachine;

    // 애니메이터 해싱처리
    // 트랜지션을 거는것보다 훨씬 유리함
    private readonly int IDLE_HASH = Animator.StringToHash("PlayerIdle");
    private readonly int WALK_HASH = Animator.StringToHash("PlayerWalk");
    private readonly int JUMP_HASH = Animator.StringToHash("PlayerJump");

    private void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _cinemachine =_virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    private void Update()
    {
        PlayerInput();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isJumped = true;
        }
    }

    private void FixedUpdate()
    {
        PlayerMove();

        if (_isJumped && _isGrounded)
        {
            PlayerJump();
        }
    }

    private void PlayerInput()
    {
        _inputX = Input.GetAxis("Horizontal");
    }

    private void PlayerMove()
    {
        // SurfaceEffect에서 무빙워크를 구현할 수 있는 코드
        if (_inputX == 0)
        {
            _animator.Play(IDLE_HASH);
            return;
        }
        // 실제 무빙워크의 느낌을 내려면
        // 기존 inputX * moveSpeed를 x값의 velocity와 더해줄 필요가 있음.
        // 그렇기에 코드를 rigid.velocity.x + (inputX * moveSpeed) 해줘야 함.
        // 다만, 현재는 무한정 빨라져서 날라감.
        //rigid.velocity = new Vector2(rigid.velocity.x + (inputX * moveSpeed), rigid.velocity.y);

        _rigid.velocity = new Vector2(_inputX * _moveSpeed, _rigid.velocity.y);
        _animator.Play(WALK_HASH);

        // 플레이어 스프라이트 좌우 변경코드
        // if문으로 해도 되고, _inputX에 바로 대입해도 됨.

        // input 방향이 0보다 작은, 즉, 음수라면 flipX 방향 변경
        if (_inputX < 0)
        {
            _spriteRenderer.flipX = true;
            _cinemachine.m_TrackedObjectOffset = new Vector3(-10, 0, 0);
        }
        else
        {
            _spriteRenderer.flipX = false;
            _cinemachine.m_TrackedObjectOffset = new Vector3(10, 0, 0);
        }
        // _spriteRenderer.flipX = _inputX < 0;
    }

    private void PlayerJump()
    {
        _rigid.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
        _animator.Play(JUMP_HASH);
        _isJumped = false;
        _isGrounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("충돌");
            _isGrounded = true;
        }
    }
}
