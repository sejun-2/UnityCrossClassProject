using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour, IDamageable
{
    enum State { Patrol, Wait, Chase, Attack, Dead }
    State _currentState = State.Patrol;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _patrolRange;
    [SerializeField, Tooltip("추격시 이속 증가배율")] private float _chaseSpeedMultiplier = 1.1f;
    [SerializeField, Tooltip("대기 시간")] private float _waitTime;
    [SerializeField, Tooltip("플레이어 감지 범위")] private float _detectionDistance;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _health;
    [SerializeField] private float _damage;
    [SerializeField] private float _attackCooldown;

    public Transform player;
    public LayerMask obstacleMask;
    public Animator animator;

    private Vector3 _spawnPos;
    private Vector3 _targetPos;
    private Vector3 _direction = Vector3.right;
    private float _waitTimer = 0f;
    private float _attackTimer = 0f;

    void Start()
    {
        _spawnPos = transform.position;
        _targetPos = _spawnPos + _direction * _patrolRange;
    }

    void Update()
    {
        if (_currentState != State.Dead)
            DetectPlayer();

        switch (_currentState)
        {
            case State.Patrol:
                Patrol();
                break;
            case State.Wait:
                Wait();
                break;
            case State.Chase:
                Chase();
                break;
            case State.Attack:
                Attack();
                break;
        }
    }

    void Patrol()
    {
        MoveTowards(_targetPos, _moveSpeed);

        if (IsObstacleAhead())//장애물이 있다면 멈추기
        {
            _currentState = State.Wait;
            _waitTimer = _waitTime;
            return;
        }

        if (Vector3.Distance(transform.position, _targetPos) < 0.5f)//목표 지점에 가까워지면 멈추기
        {
            _currentState = State.Wait;
            _waitTimer = _waitTime;
        }
    }

    void Wait()
    {
        _waitTimer -= Time.deltaTime;
        if (_waitTimer <= 0f)
        {
            _direction *= -1;
            _targetPos = _spawnPos + _direction * _patrolRange;
            _currentState = State.Patrol;
        }
    }

    void Chase()
    {
        if (Mathf.Abs(player.position.y - transform.position.y) > 2f)
        {
            _currentState = State.Patrol;
            _targetPos = _spawnPos + _direction * _patrolRange;
            Debug.Log("플레이어가 Y축 벗어남. 추격 중단");
            return;
        }

        if (Vector3.Distance(transform.position, player.position) <= _attackRange)//공격 사정거리 안에 들어오면 공격
        {
            _currentState = State.Attack;
            _attackTimer = 0f;
            return;
        }

        MoveTowards(player.position, _moveSpeed * _chaseSpeedMultiplier);
    }

    void Attack()
    {
        _attackTimer -= Time.deltaTime;

        if (Vector3.Distance(transform.position, player.position) > _attackRange)
        {
            _currentState = State.Chase;
            return;
        }

        if (_attackTimer <= 0f)
        {
            Debug.Log("좀비가 공격");
            //animator.SetTrigger("Attack");
            Manager.Player.Stats.CurHp.Value -= (int)_damage;
            //Manager.Player.TakeDamage(_damage);
            _attackTimer = _attackCooldown;
        }
    }

    void DetectPlayer()
    {
        Vector3 rayOrigin = transform.position;//지금은 캡슐이라 그대론데 프리팹 바뀌면 위치 높이 조정해야할 수 있음
        Vector3 rayDir = transform.forward * Mathf.Sign(_direction.z); // 이동 방향 기준 레이 쏨

        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, rayDir, out hit, _detectionDistance, ~0))
        {
            if (hit.collider.CompareTag("Player"))
            {
                if (_currentState == State.Patrol || _currentState == State.Wait)
                {
                    _currentState = State.Chase;
                    Debug.Log("좀비: 앞에 플레이어 있음! 추격 시작");
                }
            }
        }

        // Debug용 레이 표시
        Debug.DrawRay(rayOrigin, rayDir * _detectionDistance, Color.red);
    }

    void MoveTowards(Vector3 target, float speed)
    {
        Vector3 moveDir = (target - transform.position).normalized;

        // 수평 방향만 추출 (Y값 0)
        Vector3 flatDir = new Vector3(moveDir.x, 0f, moveDir.z).normalized;

        // 위치 이동 (y축은 그대로)
        transform.position += flatDir * speed * Time.deltaTime;

        // 수평 방향으로만 회전
        if (flatDir != Vector3.zero)
            transform.forward = flatDir;
    }

    bool IsObstacleAhead()//앞에 장애물이 있는지 레이로 확인
    {
        return Physics.Raycast(transform.position, _direction, 1f, obstacleMask);
    }

    public void TakeDamage(float amount)
    {
        if (_currentState == State.Dead) return;

        _health -= amount;
        if (_health <= 0)
        {
            _currentState = State.Dead;
            animator.SetTrigger("Die");
            this.enabled = false;
        }
    }
}
