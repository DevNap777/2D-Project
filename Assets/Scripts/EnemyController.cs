using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private LayerMask _groundLayer;

    private Rigidbody2D _rigid;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Vector2 _patrolVec;
    private bool _isWaited;

    private readonly int IDLE_HASH = Animator.StringToHash("EnemyIdle");
    private readonly int PATROL_HASH = Animator.StringToHash("EnemyPatrol");

    private void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _patrolVec = Vector2.left;
    }

    private void Update()
    {
        Patrol();
    }

    private void Patrol()
    {
        // 레이케스트를 이용하여 그라운드가 아닐때 다시 돌아오게 만듦
        Vector2 rayOrigin = transform.position + new Vector3(_patrolVec.x, 0);
        Debug.DrawRay(rayOrigin, Vector2.down * 3f, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 3f, _groundLayer);

        if (hit.collider == null)
        {
            // 돌아가는 로직
            StartCoroutine(CoTurnBack());
        }
    }

    private IEnumerator CoTurnBack()
    {
        _spriteRenderer.flipX = !_spriteRenderer.flipX;

        if (_spriteRenderer.flipX)
        {
            _patrolVec = Vector2.right;
        }
        else
        {
            _patrolVec = Vector2.left;
        }

        _animator.Play(IDLE_HASH);
        _isWaited = true;

        _rigid.velocity = Vector2.zero;

        yield return new WaitForSeconds(2f);

        _animator.Play(PATROL_HASH);
        _isWaited = false;
    }

    private void FixedUpdate()
    {
        if (_isWaited == false)
        {
            _rigid.velocity = _patrolVec * _moveSpeed;
        }
    }
}
