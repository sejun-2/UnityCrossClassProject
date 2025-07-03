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
    public enum State { Idle, Run, Climb, Hide, Farm, Stair, Attack, Die }
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

    private readonly int hashClimb = Animator.StringToHash("Climb");
    private readonly int hashFarm = Animator.StringToHash("Farm");
    private readonly int hashHide = Animator.StringToHash("Hide");
    private readonly int hashDie = Animator.StringToHash("Die");
    private readonly int hashDoor = Animator.StringToHash("Door");
    private readonly int hashStair = Animator.StringToHash("Stair");


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

    //����������
    [SerializeField] GameObject playerWeaponPrefab;
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

        playerWeaponPrefab.SetActive(false);
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
        stateText.text = $"{_currentState} {isGrounded} {Manager.Player.Stats.Weapon.Value}";

        origin = transform.position + Vector3.up;
        //���� ��� �ִ�
        isGrounded = Physics.SphereCast(origin, checkRadius, Vector3.down, out RaycastHit hit, checkDistance, groundLayer);
        //Debug.Log($"{hit.collider.gameObject.name}");
        //isGrounded = Physics.Raycast(transform.position + Vector3.up, Vector3.down, 1.5f, groundLayer);

        //���⸦ �������̴�
        if (Manager.Player.Stats.Weapon.Value != null)
        {
            playerWeaponPrefab.SetActive(true);
        }
        else
        {
            playerWeaponPrefab.SetActive(false);
        }

        //�κ��丮 ������ ���� �ٲ�����ؼ�
        if (Input.GetKeyDown(KeyCode.X))
        {
            _isInventoryOpen = false;
        }
        if (_isInventoryOpen)
        {
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

        //�κ��丮 ����
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!_isInventoryOpen)
            {
                _inventoryCanvas.ShowInven();
                _isInventoryOpen = true;
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

            //�� �Ʒ�Ű�� ������ ��� �̵��õ�
            if (Manager.Player.Stats.CurrentNearby is Stair stair)
            {
                Debug.Log("��� �̵� �õ�");
                if (stair.Interact(transform, goUp))
                {
                    StateChange(State.Stair);
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
            Manager.Player.Stats.isFarming = true;
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
            case State.Stair:
                animator.Play(hashStair);               
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

        Manager.Player.Stats.isFarming = false;

    }
}
