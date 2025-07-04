using Newtonsoft.Json;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class PlayerStats
{
    public bool isFarming = false;//�Ĺ������� ��Ÿ���� �Ұ�
    private bool isHiding = false;//��������
    public bool IsHiding
    {
        get => isHiding;
        set
        {
            if (isHiding != value)
            {
                isHiding = value;

                if (isHiding)
                    OnHideStarted?.Invoke();
                else
                    OnHideEnded?.Invoke();
            }
        }
    }
    public event Action OnHideStarted;
    public event Action OnHideEnded;

    public bool isClimbing = false;//�������� ������
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

    private readonly int hashIdle = Animator.StringToHash("Idle");
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

    [SerializeField] Slider slider;

    //����
    [SerializeField] AudioClip audioClipHide;
    [SerializeField] AudioClip audioClipHiding;
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
        slider.gameObject.SetActive(false);
        playerWeaponPrefab.SetActive(false);

        Manager.Player.Stats.OnHideStarted += PlayHideSound;
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
        stateText.text = $"farming{Manager.Player.Stats.isFarming} climbing{Manager.Player.Stats.isClimbing} isGrounded{isGrounded}";

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
            Manager.Player.Stats.isFarming = false;
        }
        if (_isInventoryOpen)
        {
            return;
        }

        //�����߿��� uparrowŰ�� ���� Ǯ�� �������� �ٸ� Ű �Է� �Ұ�
        if (Manager.Player.Stats.IsHiding)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Manager.Player.Stats.IsHiding = false;
                animator.SetBool("IsHiding", false);
                playerCollider.enabled = true;
                rb.useGravity = true;
                StateChange(State.Idle);
                Manager.Sound.SfxPlay(audioClipHide, transform, 1);
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
        //�Ĺ���, �����߿��� �ٸ� Ű �Է� �Ұ� �� ��ݽô� ����
       
            if (Manager.Player.Stats.isClimbing ||Manager.Player.Stats.isFarming || !isGrounded)
            {
                return;
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
                    Manager.Sound.SfxPlay(audioClipHide, transform, 1);
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

        //�κ��丮 ����
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!_isInventoryOpen)
            {
                _inventoryCanvas.ShowInven();
                _isInventoryOpen = true;
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
        Manager.Player.Stats.isClimbing = true;
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

            playerCollider.enabled = true;
            if (isGrounded)
            {
                Manager.Player.Stats.isClimbing = false;
            }
        }
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

        // �����̴� UI Ȱ��ȭ �� �ʱ�ȭ
        slider.gameObject.SetActive(true);
        slider.value = 0f;

        float timer = 0f;
        float duration = 1f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            slider.value = Mathf.Clamp01(timer / duration);
            yield return null;
        }

        Manager.Player.Stats.CurrentNearby.Interact();

        // �Ĺ� ����
        slider.gameObject.SetActive(false);
        animator.SetBool("IsRunning", false);

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

    //����
    void PlayHideSound()
    {
        Manager.Sound.SfxPlay(audioClipHiding, transform, 0.5f);
    }
    void OnDestroy()
    {
        Manager.Player.Stats.OnHideStarted -= PlayHideSound;
    }
}
