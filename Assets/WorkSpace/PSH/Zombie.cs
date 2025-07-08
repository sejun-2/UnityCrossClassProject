using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public enum State { Patrol, Wait, Chase, Attack, Dead, TakeDamage }
    private State _currentState = State.Patrol;
    public State CurrentState
    {
        get { return _currentState; }
    }

    [SerializeField] AudioClip _zombieDetectSound;
    private bool _isDetected;

    [SerializeField] AudioClip _zombieStepSound;
    private bool _isStep;

    [SerializeField] private float _moveSpeed = 2;
    [SerializeField] private float _patrolRange = 4;
    [SerializeField, Tooltip("�߰ݽ� �̼� ��������")] private float _chaseSpeedMultiplier = 1.1f;
    [SerializeField, Tooltip("��� �ð�")] private float _waitTime = 3;
    [SerializeField, Tooltip("�÷��̾� ���� ����")] private float _detectionDistance = 5;
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
            Debug.LogError("Player ������Ʈ�� ã�� �� �����ϴ�. �±� Ȯ�� �ʿ�.");
        }
        animator = GetComponentInChildren<Animator>();
        animator.SetInteger("MovingPattren", 0);//0���� 1��� 2�߰�
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

        if (IsObstacleAhead())//��ֹ��� �ִٸ� ���߱�
        {
            StateChange(State.Wait);
            _waitTimer = _waitTime;
            return;
        }

        if (Vector3.Distance(transform.position, _targetPos) < 0.5f)//��ǥ ������ ��������� ���߱�
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
            Debug.Log("�÷��̾ Y�� ���. �߰� �ߴ�");
            return;
        }

        if (Vector3.Distance(transform.position, _playerTransform.position) <= _attackRange)//���� �����Ÿ� �ȿ� ������ ����
        {
            StateChange(State.Attack);
            _attackTimer = 0f;
            return;
        }

        if (Manager.Player.Stats.IsHiding)
        {
            StateChange(State.Patrol);
        }

        Vector3 rayOrigin = transform.position + Vector3.up; // ������ ����

        // �� ���� ����
        Vector3 forwardDir = transform.forward * Mathf.Sign(_direction.z);
        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, forwardDir, out hit, _detectionDistance * 2, ~0))
        {
            if (!hit.collider.CompareTag("Player"))
            {
                StateChange(State.Wait);
                _waitTimer = _waitTime;
                Debug.Log("����: �߰� ����");
            }
        }


        MoveTowards(_playerTransform.position, _moveSpeed * _chaseSpeedMultiplier);
    }

    void Attack()
    {
    }

    void DetectPlayer()
    {
        Vector3 rayOrigin = transform.position + Vector3.up; // ������ ����

        // �� ���� ����
        Vector3 forwardDir = transform.forward * Mathf.Sign(_direction.z);
        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, forwardDir, out hit, 1f, ~0))
        {
            if (!hit.collider.CompareTag("Player"))
            {
                _waitTimer = _waitTime;
                StateChange(State.Wait);
            }
        }

        if (Physics.Raycast(rayOrigin, forwardDir, out hit, _detectionDistance, ~0))
        {
            if (hit.collider.CompareTag("Player"))
            {
                StateChange(State.Chase);
                Debug.Log("����: �տ� �÷��̾� ����! �߰� ����");
            }
        }

        // �� ���� ���� (�����Ÿ� ����)
        Vector3 backwardDir = -transform.forward * Mathf.Sign(_direction.z);

        if (Physics.Raycast(rayOrigin, backwardDir, out hit, _detectionDistance * 0.5f, ~0))
        {
            if (hit.collider.CompareTag("Player"))
            {
                StateChange(State.Chase);
                Debug.Log("����: �տ� �÷��̾� ����! �߰� ����");
            }
        }

        // Debug�� ���� ǥ��
        Debug.DrawRay(rayOrigin, forwardDir * _detectionDistance, Color.red);
        Debug.DrawRay(rayOrigin, backwardDir * (_detectionDistance * 0.5f), Color.blue);
    }


    void MoveTowards(Vector3 target, float speed)
    {
        Vector3 moveDir = (target - transform.position).normalized;

        // ���� ���⸸ ���� (Y�� 0)
        Vector3 flatDir = new Vector3(moveDir.x, 0f, moveDir.z).normalized;

        // ��ġ �̵� (y���� �״��)
        transform.position += flatDir * speed * Time.deltaTime;

        // ���� �������θ� ȸ��
        if (flatDir != Vector3.zero)
            transform.forward = flatDir;
    }

    bool IsObstacleAhead()//�տ� ��ֹ��� �ִ��� ���̷� Ȯ��
    {
        return Physics.Raycast(transform.position, _direction, 1f, obstacleMask);
    }


    private IEnumerator DieAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
        // �Ǵ� Destroy(gameObject);
    }

    public void StateChange(State state)
    {
        switch (state)
        {
            case State.Patrol:
                _currentState = State.Patrol;
                _isStep = true;
                StartCoroutine(StepSoundCoroutine());
                Flip();
                animator.SetInteger("MovingPattren", 0);
                break;
            case State.Wait:
                _isStep = false;
                _currentState = State.Wait;
                animator.SetInteger("MovingPattren", 1);
                break;
            case State.Chase:
                _isStep = false;
                if (!_isDetected)
                {
                    _isDetected = true;
                    Manager.Sound.SfxPlay(_zombieDetectSound, transform);
                }
                _currentState = State.Chase;
                animator.SetInteger("MovingPattren", 2);
                break;
            case State.Attack:
                _isStep = false;
                _currentState = State.Attack;
                _attack.Attack();
                break;
            case State.Dead:
                _isStep = false;
                _currentState = State.Dead;
                StartCoroutine(DieAfterDelay());
                animator.SetBool("IsDead", true);
                break;
            case State.TakeDamage:
                _isStep = false;
                _currentState = State.TakeDamage;
                break;
            default:
                break;
        }
    }
    public void Flip()//�Ž� �� ������ ġ��ġ�� �ϱ� ���� ���� �ٲܶ����� �̰� ȣ��
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private IEnumerator StepSoundCoroutine()
    {
        while (_isStep)
        {
            Manager.Sound.SfxPlay(_zombieStepSound, transform);
            yield return new WaitForSeconds(1f);
        }
    }
}
