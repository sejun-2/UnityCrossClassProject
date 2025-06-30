using System.Collections;
using UnityEngine;

public class ZombieTest : MonoBehaviour
{
    public enum State { Idle, Patrol, Chase, Attack }
    public State currentState = State.Idle;

    public Transform player;
    public LayerMask obstacleMask;

    [Header("Settings")]
    public float baseDetectionRange = 30f; // 기본 인식 범위
    public float detectionMultiplierWhenAware = 2.5f; // 한 번 인식했거나 달리는 경우 배수
    public float backDetectionMultiplier = 0.5f; // 등 뒤 인식 배수
    public float detectionThroughWallMultiplier = 1f / 3f; // 벽 너머 인식 배수

    public float slowSpeed = 3f;
    public float fastSpeed = 8f;
    public float attackDuration = 1f; // 공격 모션 시간
    public float attackHitStart = 0.2f;
    public float attackHitEnd = 0.6f;

    private bool isAware = false;
    private bool isAttacking = false;

    private void Start()
    {
        StartCoroutine(StateMachine());
    }

    IEnumerator StateMachine()
    {
        while (true)
        {
            switch (currentState)
            {
                case State.Idle:
                    yield return IdleRoutine();
                    break;
                case State.Patrol:
                    yield return PatrolRoutine();
                    break;
                case State.Chase:
                    yield return ChaseRoutine();
                    break;
                case State.Attack:
                    yield return AttackRoutine();
                    break;
            }
            yield return null;
        }
    }

    IEnumerator IdleRoutine()
    {
        float waitTime = Random.Range(3f, 5f);
        yield return new WaitForSeconds(waitTime);
        currentState = State.Patrol;
    }

    IEnumerator PatrolRoutine()
    {
        float patrolTime = Random.Range(5f, 10f);
        Vector3 dir = Random.value < 0.5f ? Vector3.left : Vector3.right;
        float timer = 0f;

        while (timer < patrolTime)
        {
            transform.Translate(dir * slowSpeed * Time.deltaTime);
            timer += Time.deltaTime;

            if (IsObstacleAhead(dir))
            {
                yield return new WaitForSeconds(Random.Range(3f, 5f));
                dir = -dir;
                timer = 0f; // 반대방향으로 다시
            }

            if (CheckPlayerDetection())
            {
                currentState = State.Chase;
                yield break;
            }

            yield return null;
        }

        currentState = State.Idle;
    }

    IEnumerator ChaseRoutine()
    {
        isAware = true;
        while (true)
        {
            Vector3 dir = (player.position - transform.position).normalized;
            transform.Translate(dir * fastSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, player.position) < 1f)
            {
                currentState = State.Attack;
                yield break;
            }

            if (!CheckPlayerDetection())
            {
                float chaseTimer = 0f;
                while (chaseTimer < 15f)
                {
                    transform.Translate(dir * fastSpeed * Time.deltaTime);
                    chaseTimer += Time.deltaTime;

                    if (CheckPlayerDetection())
                    {
                        break;
                    }
                    yield return null;
                }

                currentState = State.Idle;
                isAware = false;
                yield break;
            }

            yield return null;
        }
    }

    IEnumerator AttackRoutine()
    {
        if (isAttacking)
            yield break;

        isAttacking = true;
        float timer = 0f;
        while (timer < attackDuration)
        {
            // 피격 판정 구간
            if (timer > attackHitStart && timer < attackHitEnd)
            {
                // 플레이어 피격 처리
                Debug.Log("플레이어 피격!");
            }
            timer += Time.deltaTime;
            yield return null;
        }
        isAttacking = false;
        currentState = State.Chase;
    }

    bool CheckPlayerDetection()
    {
        float detectionRange = baseDetectionRange;

        if (isAware)
            detectionRange *= detectionMultiplierWhenAware;

        Vector3 toPlayer = player.position - transform.position;
        float dot = Vector3.Dot(transform.forward, toPlayer.normalized);
        if (dot < 0) // 등 뒤라면
            detectionRange *= backDetectionMultiplier;

        // Raycast로 벽 확인
        if (Physics.Raycast(transform.position, toPlayer, out RaycastHit hit, detectionRange, obstacleMask))
        {
            if (hit.transform != player)
            {
                detectionRange *= detectionThroughWallMultiplier;
            }
        }

        return toPlayer.magnitude < detectionRange;
    }

    bool IsObstacleAhead(Vector3 dir)
    {
        return Physics.Raycast(transform.position, dir, 1f, obstacleMask);
    }
}
