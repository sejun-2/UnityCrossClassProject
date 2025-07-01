using Newtonsoft.Json;
using System.Collections;
using System.Threading;
using UnityEngine;

public partial class PlayerStats
{
    public bool isFarming = false;//�Ĺ������� ��Ÿ���� �Ұ�
    public bool isHiding = false;//��������
    [JsonIgnore] public IInteractable CurrentNearby;//����� ��ȣ�ۿ� ���
}

public class PlayerInteraction : MonoBehaviour
{
    public enum State { Idle, Run, Climb, Hide, Farm, Attack, Die }
    private State _currentState = State.Idle;
    public State CurrentState
    {
        get { return _currentState; }
    }

    public float moveSpeed = 5f;
    public float climbSpeed = 3f;

    private Rigidbody rb;
    private Collider playerCollider;

    private bool isAutoClimbing = false;
    private Vector3 climbTargetPos;

    public Animator animator;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
        Manager.Player.Stats.isFarming = false;
    }

    void Update()
    {
        //�Ĺ��߿��� xŰ�� �ݱ� �������� �ٸ� Ű �Է� �Ұ�
        if (Manager.Player.Stats.isFarming)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                Manager.Player.Stats.isFarming = false;
                animator.SetBool("IsFarming", false);
                StateChange(State.Idle);
            }
            return;
        }

        //�����߿��� uparrowŰ�� ���� Ǯ�� �������� �ٸ� Ű �Է� �Ұ�
        if (Manager.Player.Stats.isHiding)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Manager.Player.Stats.isHiding = false;
                animator.SetBool("IsHiding", false);
                playerCollider.enabled = true;
                rb.useGravity = true;
                StateChange(State.Idle);

                Debug.Log("��������");
            }
            return;
        }

        //��� ���¶�� �����
        if (isAutoClimbing)
        {
            AutoClimb();
            return;
        }

        //���Ʒ� Ű�� ������ ��ٸ� �̵� �õ� ���� �õ�
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            bool goUp = Input.GetKeyDown(KeyCode.UpArrow);
           
            if (Manager.Player.Stats.CurrentNearby is Ladder ladder)
            {
                Debug.Log("��ٸ� �̵� �õ�");
                if(ladder.Interact(transform, goUp, climbSpeed, this))
                StateChange(State.Climb);
            }

            if (Manager.Player.Stats.CurrentNearby is Hideout hideout && goUp)
            {
                Debug.Log("���� �õ�");
                if (hideout.Interact(1))
                {
                    playerCollider.enabled = false;
                    rb.useGravity = false;
                    StateChange(State.Hide);
                }
            }
        }

        //zŰ�� ������ ��ȣ�ۿ� �õ�
        if (Input.GetKeyDown(KeyCode.Z) && Manager.Player.Stats.CurrentNearby != null)
        {
            Debug.Log("��ȣ�ۿ� �õ�");
            Manager.Player.Stats.CurrentNearby.Interact();
        }     

        //spaceŰ�� ������ ���� �õ�
        if (Input.GetKeyDown(KeyCode.Space))
        {           
            Debug.Log("���� �õ�");
            Attack();
        }

        //�̿ܿ��� �¿� �̵�
        MoveSideways();
    }

    void MoveSideways()
    {
        float h = Input.GetAxis("Horizontal");

        if (Mathf.Abs(h) > 0.01f)
        {
            StateChange(State.Run);
        }
        else
        {
            StateChange(State.Idle);
        }

        if (h != 0f)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(h) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        Vector3 move = Vector3.right * h;
        transform.position += move * moveSpeed * Time.deltaTime;
    }

    public void StartClimb(Vector3 from, Vector3 to)
    {
        transform.position = from;
        climbTargetPos = to;
        isAutoClimbing = true;

        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        playerCollider.enabled = false;

        StateChange(State.Climb);
    }
    void AutoClimb()
    {
        transform.position = Vector3.MoveTowards(transform.position, climbTargetPos, climbSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, climbTargetPos) < 0.05f)
        {
            isAutoClimbing = false;
            rb.useGravity = true;

            Debug.Log("��ٸ� �ڵ� �̵� �Ϸ�");

            StartCoroutine(ReenableColliderAfterDelay());
        }
    }

    IEnumerator ReenableColliderAfterDelay()
    {
        yield return new WaitForSeconds(0.1f); // ª�� ���� ��
        playerCollider.enabled = true;
    }
    public void Attack()
    {
        Item weapon = Manager.Player.Stats.Weapon.Value;

        if (weapon == null || Manager.Player.Stats.IsAttack.Value) return;

        Manager.Player.Stats.IsAttack.Value = true;
        StartCoroutine(AttackCoroutine(weapon.attackSpeed));
        StateChange(State.Attack);
        Debug.Log("���� ����ffffff");

        Vector3 direction = new Vector3(transform.localScale.x, 0, 0);

        Debug.DrawRay(transform.position + Vector3.up, direction * weapon.attackRange * 10, Color.red, 1f);
        RaycastHit[] hits = Physics.RaycastAll(transform.position + Vector3.up, direction, weapon.attackRange * 10, ~0, QueryTriggerInteraction.Collide);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.CompareTag("Zombie"))
            {
                IDamageable zombie = hit.collider.gameObject.GetComponent<IDamageable>();

                zombie.TakeDamage(weapon.attackValue);

                Debug.Log($"{hit.collider.gameObject.name}���� {weapon.attackValue} ��ŭ�� ������");

                break;
            }
        }      
    }

    private IEnumerator AttackCoroutine(float time)
    {
        yield return new WaitForSeconds(time);

        Manager.Player.Stats.IsAttack.Value = false;
    }
    
    public void StateChange(State state)
    {
        switch (state)
        {
            case State.Idle:
                _currentState = State.Idle;
                animator.SetBool("IsRunning", false);
                break;
            case State.Run:
                _currentState = State.Run;
                animator.SetBool("IsRunning", true);
                break;
            case State.Climb:
                _currentState = State.Climb;
                animator.SetTrigger("Climbing");
                break;
            case State.Attack:
                _currentState = State.Attack;
                animator.SetTrigger("Attack");
                break;
            case State.Farm:
                _currentState = State.Farm;
                animator.SetBool("IsFarming", true);
                break;
            case State.Hide:
                _currentState = State.Hide;
                animator.SetBool("IsHiding", true);
                break;
            case State.Die:
                _currentState = State.Die;
                animator.SetBool("IsDead", true);
                break;
            default:
                break;
        }
    }
}
