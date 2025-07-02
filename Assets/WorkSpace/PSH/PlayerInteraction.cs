using Newtonsoft.Json;
using System.Collections;
using TMPro;
using UnityEngine;

public partial class PlayerStats
{
    public bool isFarming = false;//�Ĺ������� ��Ÿ���� �Ұ�
    public bool isHiding = false;//��������
    public bool isFalling = false;//�������� ������
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

    private bool isGrounded;
    public LayerMask groundLayer;

    public Animator animator;
    private readonly int hashIsRunning = Animator.StringToHash("IsRunning");
    private readonly int hashIsClimbing = Animator.StringToHash("IsClimbing");
    private readonly int hashIsHiding = Animator.StringToHash("IsHiding");
    private readonly int hashIsFarming = Animator.StringToHash("IsFarming");
    private readonly int hashAttackTrigger = Animator.StringToHash("Attack");
    private readonly int hashClimbingTrigger = Animator.StringToHash("Climbing");
    private readonly int hashDeadTrigger = Animator.StringToHash("Dead");

    private readonly int hashIdle = Animator.StringToHash("Idle");
    private readonly int hashRun = Animator.StringToHash("Run");
    private readonly int hashClimb = Animator.StringToHash("Climb");
    private readonly int hashAttack = Animator.StringToHash("Attack");
    private readonly int hashFarm = Animator.StringToHash("Farm");
    private readonly int hashHide = Animator.StringToHash("Hide");
    private readonly int hashDie = Animator.StringToHash("Die");
    private readonly int hashDoor = Animator.StringToHash("Door");


    [SerializeField] float crossFadeTime = .1f;

    private PlayerAttack _playerAttack;
    private PlayerEquipment _playerEquipment;
    private InventoryCanvas _inventoryCanvas;
    private bool _isInventoryOpen = false;

    //�׽�Ʈ�� �ؽ�Ʈ
    [SerializeField] TextMeshProUGUI stateText;

    //���� Ȯ�ο� ���Ǿ�ĳ��Ʈ ����
    [SerializeField] Vector3 origin;
    [SerializeField] float checkRadius = 0.2f;
    [SerializeField] float checkDistance = 0.1f;

    //��ȣ�ۿ� ��� �ð�
    private WaitForSeconds wait1Sec;

    private void Awake()
    {
        Manager.Player.Transform = transform;

        _playerEquipment = GetComponent<PlayerEquipment>();
        _playerAttack = GetComponent<PlayerAttack>();
        _inventoryCanvas = GetComponentInChildren<InventoryCanvas>();

        wait1Sec = new WaitForSeconds(1f);
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
        Manager.Player.Stats.isFarming = false;
        groundLayer = LayerMask.GetMask("Obstacle");
        _playerAttack = GetComponent<PlayerAttack>();

        origin = transform.position + Vector3.up * 0.1f;

    }
    private void OnDrawGizmos()
    {
        // �÷��̾ Ȱ��ȭ�Ǿ� ���� ���� �׸���
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;


            Vector3 direction = Vector3.down;
            // ���� ��ü
            Gizmos.DrawWireSphere(origin, checkRadius);

            // �� ��ü
            Gizmos.DrawWireSphere(origin + direction * checkDistance, checkRadius);

            // ���̸� Cylinderó�� �����ϱ� ���� DrawLine 4���� ����
            Gizmos.DrawLine(origin + Vector3.forward * checkRadius, origin + direction * checkDistance + Vector3.forward * checkRadius);
            Gizmos.DrawLine(origin - Vector3.forward * checkRadius, origin + direction * checkDistance - Vector3.forward * checkRadius);
            Gizmos.DrawLine(origin + Vector3.right * checkRadius, origin + direction * checkDistance + Vector3.right * checkRadius);
            Gizmos.DrawLine(origin - Vector3.right * checkRadius, origin + direction * checkDistance - Vector3.right * checkRadius);
        }
    }
    void Update()
    {
        //�׽�Ʈ��
        stateText.text = $"{_currentState} {isGrounded}";

        origin = transform.position + Vector3.up;
        //���� ��� �ִ�
        isGrounded = Physics.SphereCast(origin, checkRadius, Vector3.down, out RaycastHit hit, checkDistance, groundLayer);
        //Debug.Log($"{hit.collider.gameObject.name}");
        //isGrounded = Physics.Raycast(transform.position + Vector3.up, Vector3.down, 1.5f, groundLayer);



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

        //�κ��丮 ����
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!_isInventoryOpen)
            {
                _inventoryCanvas.ShowInven();
            }
        }
       
        //���Ʒ� Ű�� ������ ��ٸ� �̵� �õ� ���� �õ�
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            bool goUp = Input.GetKeyDown(KeyCode.UpArrow);

            if (Manager.Player.Stats.CurrentNearby is Ladder ladder)
            {
                Debug.Log("��ٸ� �̵� �õ�");
                if (ladder.Climb(transform, goUp, climbSpeed, this))
                {
                    StartCoroutine(RotateAndRestore());
                    StateChange(State.Climb);
                }
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
        //�Ĺ���, �����߿��� �ٸ� Ű �Է� �Ұ�
        if (Manager.Player.Stats.isFarming || !isGrounded)
        {
            return;
        }
        //zŰ�� ������ ��ȣ�ۿ� �õ�
        if (Input.GetKeyDown(KeyCode.Z) && Manager.Player.Stats.CurrentNearby != null)
        {
            GameObject target = (Manager.Player.Stats.CurrentNearby as MonoBehaviour).gameObject;

            Debug.Log("��ȣ�ۿ� �õ�");
            if (target.CompareTag("Door"))
            {
                StartCoroutine(NotRotateAndInteract());
                animator.Play(hashDoor);
            }
            else
            {
                StartCoroutine(RotateAndInteract());
                animator.Play(hashFarm);
            }
                
        }

        //spaceŰ�� ������ ���� �õ�
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("���� �õ�");
            _playerAttack.Attack();
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

    /* public void StateChange(State state)
     {
         switch (state)
         {
             case State.Idle:
                 _currentState = State.Idle;
                 animator.SetBool(hashIsRunning, false);
                 break;
             case State.Run:
                 _currentState = State.Run;
                 animator.SetBool(hashIsRunning, true);
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
                 animator.SetTrigger("Dead");
                 break;
             default:
                 break;
         }
     }*/

    /* public void StateChange(State state)
     {
         switch (state)
         {
             case State.Idle:
                 _currentState = State.Idle;
                 animator.SetBool(hashIsRunning, false);
                 break;
             case State.Run:
                 _currentState = State.Run;
                 animator.SetBool(hashIsRunning, true);
                 break;
             case State.Climb:
                 _currentState = State.Climb;
                 animator.SetTrigger(hashClimbingTrigger);
                 break;
             case State.Attack:
                 _currentState = State.Attack;
                 animator.SetTrigger(hashAttackTrigger);
                 break;
             case State.Farm:
                 _currentState = State.Farm;
                 animator.SetBool(hashIsFarming, true);
                 break;
             case State.Hide:
                 _currentState = State.Hide;
                 animator.SetBool(hashIsHiding, true);
                 break;
             case State.Die:
                 _currentState = State.Die;
                 animator.SetTrigger(hashDeadTrigger);
                 break;
             default:
                 break;
         }
     }*/

    public void StateChange(State state)
    {
        _currentState = state;

        switch (state)
        {
            case State.Idle:
                animator.SetBool("IsRunning", false);
                break;
            case State.Run:
                animator.SetBool("IsRunning", true);
                break;
            case State.Climb:
                animator.Play(hashClimb);
                Debug.Log("��ݤ�������������");
                break;
            case State.Attack:
                animator.Play(hashAttack);
                break;
            case State.Farm:
                animator.Play(hashFarm);
                break;
            case State.Hide:
                animator.CrossFade(hashHide, crossFadeTime);
                animator.SetBool("IsHiding", true);
                Debug.Log("���⤱������������");
                break;
            case State.Die:
                animator.Play(hashDie);
                break;
            default:
                break;
        }
    }

    public void RotateToInteract()
    {
        Vector3 scale = transform.localScale;
        Vector3 rotation = transform.eulerAngles;

        rotation.y = 90f * scale.x * -1;
        transform.eulerAngles = rotation;
    }

    public void RestoreRotation()
    {
        transform.rotation = Quaternion.identity;
    }

    private IEnumerator RotateAndInteract()
    {
        RotateToInteract();

        yield return wait1Sec;

        Manager.Player.Stats.CurrentNearby.Interact();

        RestoreRotation();
    }

    private IEnumerator RotateAndRestore()
    {
        RotateToInteract();

        yield return wait1Sec;

        RestoreRotation();
    }

    private IEnumerator NotRotateAndInteract()
    {
        
        yield return wait1Sec;

        Manager.Player.Stats.CurrentNearby.Interact();

    }
}
