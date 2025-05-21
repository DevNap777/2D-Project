using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    // �����̴� �ӵ�
    [SerializeField] private float _moveSpeed;
    // ����
    [SerializeField] private float _jumpPower;

    // �÷��̾ �ݴ� ������ �ٶ� ��, ī�޶� �̵��ϰ� �ϴ� ��
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    private Animator _animator;

    private Rigidbody2D _rigid;

    // �¿� ������ ���� SpriteRenderer
    private SpriteRenderer _spriteRenderer;
    private Vector2 InputVec;
    private float _inputX;

    // <���� ó��>
    // ������ �ߴ��� �Ǵ��ϱ� ���� bool Ÿ�� ����
    private bool _isJumped;
    // ���� ��Ҵ����� �Ǵ��ϱ� ���� bool Ÿ�� ����
    private bool _isGrounded;

    private CinemachineFramingTransposer _cinemachine;

    // �ִϸ����� �ؽ�ó��
    // Ʈ�������� �Ŵ°ͺ��� �ξ� ������
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
        // SurfaceEffect���� ������ũ�� ������ �� �ִ� �ڵ�
        if (_inputX == 0)
        {
            _animator.Play(IDLE_HASH);
            return;
        }
        // ���� ������ũ�� ������ ������
        // ���� inputX * moveSpeed�� x���� velocity�� ������ �ʿ䰡 ����.
        // �׷��⿡ �ڵ带 rigid.velocity.x + (inputX * moveSpeed) ����� ��.
        // �ٸ�, ����� ������ �������� ����.
        //rigid.velocity = new Vector2(rigid.velocity.x + (inputX * moveSpeed), rigid.velocity.y);

        _rigid.velocity = new Vector2(_inputX * _moveSpeed, _rigid.velocity.y);
        _animator.Play(WALK_HASH);

        // �÷��̾� ��������Ʈ �¿� �����ڵ�
        // if������ �ص� �ǰ�, _inputX�� �ٷ� �����ص� ��.

        // input ������ 0���� ����, ��, ������� flipX ���� ����
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
            Debug.Log("�浹");
            _isGrounded = true;
        }
    }
}
