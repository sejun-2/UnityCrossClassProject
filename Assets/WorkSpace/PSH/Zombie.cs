using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public enum State { Patrol, Wait, Chase, Attack, Dead, TakeDamage }
    private State _currentState = State.Patrol;
    public State CurrentState
    {
        get { return _currentState; }
    }

    [SerializeField] private float _moveSpeed = 2;
    [SerializeField] private float _patrolRange = 4;
    [SerializeField, Tooltip("추격시 이속 증가배율")] private float _chaseSpeedMultiplier = 1.1f;
    [SerializeField, Tooltip("대기 시간")] private float _waitTime = 3;
    [SerializeField, Tooltip("플레이어 감지 범위")] private float _detectionDistance = 5;
    [SerializeField] private float _attackRange = 2;
    [SerializeField] private float _health = 100;
    public float Health { get => _health; set { _health = value; } }
    [SerializeField] private float _damage = 10;
    [SerializeField] private float _attackCooldown = 2;

    public Transform _playerTransform;
    public LayerMask obstacleMask;
    public Animator animator;
    private ZombieAttack _attack;

    private Vector3 _spawnPos;
    private Vector3 _targetPos;
    private Vector3 _direction = Vector3.right;
    private float _waitTimer = 0f;
    private float _attackTimer = 0f;

    void Start()
    {
        _spawnPos = transform.position;
        _targetPos = _spawnPos + _direction * _patrolRange;
        _attack = GetComponent<ZombieAttack>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            _playerTransform = playerObj.transform;
        }
        else
        {
            Debug.LogError("Player 오브젝트를 찾을 수 없습니다. 태그 확인 필요.");
        }
        animator = GetComponentInChildren<Animator>();
        animator.SetInteger("MovingPattren", 0);//0순찰 1대기 2추격
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_targetPos, 1);
    }

    void Update()
    {
        if (_currentState == State.Patrol || _currentState == State.Wait)
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
            StateChange(State.Wait);
            _waitTimer = _waitTime;
            return;
        }

        if (Vector3.Distance(transform.position, _targetPos) < 0.5f)//목표 지점에 가까워지면 멈추기
        {
            StateChange(State.Wait);
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
            StateChange(State.Patrol);
        }
    }

    void Chase()
    {
        if (Mathf.Abs(_playerTransform.position.y - transform.position.y) > 2f)
        {
            StateChange(State.Patrol);
            _targetPos = _spawnPos + _direction * _patrolRange;
            Debug.Log("플레이어가 Y축 벗어남. 추격 중단");
            return;
        }

        if (Vector3.Distance(transform.position, _playerTransform.position) <= _attackRange)//공격 사정거리 안에 들어오면 공격
        {
            StateChange(State.Attack);
            _attackTimer = 0f;
            return;
        }

        if (Manager.Player.Stats.isHiding)
        {
            StateChange(State.Patrol);
        }

        Vector3 rayOrigin = transform.position + Vector3.up; // 눈높이 조정

        // 앞 방향 레이
        Vector3 forwardDir = transform.forward * Mathf.Sign(_direction.z);
        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, forwardDir, out hit, _detectionDistance * 2, ~0))
        {
            if (!hit.collider.CompareTag("Player"))
            {
                StateChange(State.Wait);
                _waitTimer = _waitTime;
                Debug.Log("좀비: 추격 종료");
            }
        }


        MoveTowards(_playerTransform.position, _moveSpeed * _chaseSpeedMultiplier);
    }

    void Attack()
    {
    }

    void DetectPlayer()
    {
        Vector3 rayOrigin = transform.position + Vector3.up; // 눈높이 조정

        // 앞 방향 레이
        Vector3 forwardDir = transform.forward * Mathf.Sign(_direction.z);
        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, forwardDir, out hit, _detectionDistance, ~0))
        {
            if (hit.collider.CompareTag("Player"))
            {
                StateChange(State.Chase);
                Debug.Log("좀비: 앞에 플레이어 있음! 추격 시작");
            }
        }

        // 뒤 방향 레이 (사정거리 절반)
        Vector3 backwardDir = -transform.forward * Mathf.Sign(_direction.z);

        if (Physics.Raycast(rayOrigin, backwardDir, out hit, _detectionDistance * 0.5f, ~0))
        {
            if (hit.collider.CompareTag("Player"))
            {
                StateChange(State.Chase);
                Debug.Log("좀비: 앞에 플레이어 있음! 추격 시작");
            }
        }

        // Debug용 레이 표시
        Debug.DrawRay(rayOrigin, forwardDir * _detectionDistance, Color.red);
        Debug.DrawRay(rayOrigin, backwardDir * (_detectionDistance * 0.5f), Color.blue);
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


    private IEnumerator DieAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
        // 또는 Destroy(gameObject);
    }

    public void StateChange(State state)
    {
        Debug.Log(state);

        switch (state)
        {
            case State.Patrol:
                _currentState = State.Patrol;
                Flip();
                animator.SetInteger("MovingPattren", 0);
                break;
            case State.Wait:
                _currentState = State.Wait;
                animator.SetInteger("MovingPattren", 1);
                break;
            case State.Chase:
                _currentState = State.Chase;
                animator.SetInteger("MovingPattren", 2);
                break;
            case State.Attack:
                _currentState = State.Attack;
                _attack.Attack();
                break;
            case State.Dead:
                _currentState = State.Dead;
                StartCoroutine(DieAfterDelay());
                animator.SetBool("IsDead", true);
                break;
            case State.TakeDamage:
                _currentState = State.TakeDamage;
                break;
            default:
                break;
        }
    }
    public void Flip()//매쉬 한 쪽으로 치우치게 하기 위해 방향 바꿀때마다 이거 호출
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
