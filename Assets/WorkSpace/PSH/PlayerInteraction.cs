using Newtonsoft.Json;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class PlayerStats
{
    public bool isFarming = false;//파밍중인지 나타내는 불값
    private bool isHiding = false;//숨었는지
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

    public bool isClimbing = false;//떨어지는 중인지
    [JsonIgnore] public IInteractable CurrentNearby;//가까운 상호작용 대상
}

public class PlayerInteraction : MonoBehaviour
{
    public float testFloat;
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

    private bool _isGrounded;
    public bool IsGrounded
    {
        get => _isGrounded;
        set
        {
            if (_isGrounded != value)
            {
                _isGrounded = value;

                if (_isGrounded)
                    OnLanded?.Invoke();
                else
                    OnLeftGround?.Invoke();
            }
        }
    }

    public event Action OnLanded;
    public event Action OnLeftGround;
    public LayerMask groundLayer;

    public Animator animator;
    private readonly int hashIdle = Animator.StringToHash("Idle");
    private readonly int hashClimb = Animator.StringToHash("Climb");
    private readonly int hashFarm = Animator.StringToHash("Farm");
    private readonly int hashHide = Animator.StringToHash("Hide");
    private readonly int hashDie = Animator.StringToHash("Die");
    private readonly int hashDoor = Animator.StringToHash("Door");
    private readonly int hashStair = Animator.StringToHash("Stair");
    private readonly int hashFall = Animator.StringToHash("Falling");
    private readonly int hashLand = Animator.StringToHash("Landing");

    private PlayerAttack _playerAttack;
    private PlayerEquipment _playerEquipment;
    private InventoryCanvas _inventoryCanvas;

    //테스트용 텍스트
    [SerializeField] TextMeshProUGUI stateText;

    //지면 확인용 스피어캐스트 변수
    [SerializeField] Vector3 origin;
    [SerializeField] float checkRadius = 0.2f;
    [SerializeField] float checkDistance = 0.1f;

    //상호작용 대기 시간
    private WaitForSeconds wait1Sec;

    //무기프리팹
    [SerializeField] GameObject playerWeaponPrefab;

    [SerializeField] Slider slider;

    //사운드
    [SerializeField] AudioClip audioClipHide;
    [SerializeField] AudioClip audioClipHiding;
    [SerializeField] AudioClip audioClipFalling;
    [SerializeField] AudioClip audioClipLanding;
    [SerializeField] AudioClip audioClipWalking;
    [SerializeField] AudioClip audioClipFarming;
    //액션
    [SerializeField] private Transform _renderObject;

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
        Manager.Player.Stats.OnHideEnded += StopHideSound;
        OnLeftGround += HandleFalling;
        IsGrounded = true;
        OnLanded += HandleLanding; 
    }
    private void OnDrawGizmos()
    {
        // 플레이어가 활성화되어 있을 때만 그리기
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;


            Vector3 direction = Vector3.down;
            // 시작 구체
            Gizmos.DrawWireSphere(origin, checkRadius);

            // 끝 구체
            Gizmos.DrawWireSphere(origin + direction * checkDistance, checkRadius);

            // 사이를 Cylinder처럼 연결하기 위해 DrawLine 4방향 예시
            Gizmos.DrawLine(origin + Vector3.forward * checkRadius, origin + direction * checkDistance + Vector3.forward * checkRadius);
            Gizmos.DrawLine(origin - Vector3.forward * checkRadius, origin + direction * checkDistance - Vector3.forward * checkRadius);
            Gizmos.DrawLine(origin + Vector3.right * checkRadius, origin + direction * checkDistance + Vector3.right * checkRadius);
            Gizmos.DrawLine(origin - Vector3.right * checkRadius, origin + direction * checkDistance - Vector3.right * checkRadius);
        }
    }
    void Update()
    {
        if (Manager.Player.Stats.isFarming)
        {
            Manager.Sound.SfxStopLoop("Walking", 0);
        }

        //테스트용
        if (stateText != null)
        {
            stateText.text = $"farming{Manager.Player.Stats.isFarming} climbing{Manager.Player.Stats.isClimbing} isGrounded{_isGrounded}";
        }
        
        origin = transform.position + Vector3.up;
        //땅을 밟고 있니
        IsGrounded = Physics.SphereCast(origin, checkRadius, Vector3.down, out RaycastHit hit, checkDistance, groundLayer);
        //Debug.Log($"{hit.collider.gameObject.name}");
        //isGrounded = Physics.Raycast(transform.position + Vector3.up, Vector3.down, 1.5f, groundLayer);

        //무기를 장착중이니
        if (Manager.Player.Stats.Weapon.Value != null)
        {
            playerWeaponPrefab.SetActive(true);
        }
        else
        {
            playerWeaponPrefab.SetActive(false);
        }

        //인벤토리 여는
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (!Manager.Player.Stats.isFarming)
            {
                Manager.UI.Inven.ShowInven();
                StateChange(State.Idle);
                Manager.Player.Stats.isFarming = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Manager.UI.PopUp.ShowPopUp<MainMenuPopUp>();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            if (!Manager.Player.Stats.isFarming)
            {
                Manager.UI.Inven.ShowSubStoryUI();
                StateChange(State.Idle);
                Manager.Player.Stats.isFarming = true;
            }
        }           

        //은신중에는 uparrow키로 은신 풀기 전까지는 다른 키 입력 불가
        if (Manager.Player.Stats.IsHiding)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (Manager.Player.Stats.CurrentNearby is Hideout hideout)
                {
                    hideout.LiftBoxOffPlayer(transform);
                }
                Manager.Player.Stats.IsHiding = false;
                animator.SetBool("IsHiding", false);
                playerCollider.enabled = true;
                rb.useGravity = true;
                StateChange(State.Idle);
                Manager.Sound.SfxPlay(audioClipHide, transform, 1);
                Debug.Log("은신해제");
            }
            return;
        }

        //등반 상태라면 등반함
        if (isAutoClimbing)
        {
            AutoClimb();
            return;
        }

        //파밍중, 낙하중, 등반중에는 다른 키 입력 불가
        if (Manager.Player.Stats.isClimbing || Manager.Player.Stats.isFarming || !_isGrounded)
        {
            return;
        }

        //위아래 키를 누르면 사다리 이동 시도 은신 시도
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            bool goUp = Input.GetKeyDown(KeyCode.UpArrow);

            if (Manager.Player.Stats.CurrentNearby is Ladder ladder)
            {
                Debug.Log("사다리 이동 시도");
                if (ladder.Climb(transform, goUp, climbSpeed, this))
                {
                    StartCoroutine(RotateAndRestore());
                    StateChange(State.Climb);
                }
            }

            if (Manager.Player.Stats.CurrentNearby is Hideout hideout && goUp)
            {
                Debug.Log("은신 시도");
                if (hideout.Interact(transform))
                {
                    playerCollider.enabled = false;
                    rb.useGravity = false;
                    StateChange(State.Hide);
                    Manager.Sound.SfxPlay(audioClipHide, transform, 1);
                }
            }

            //위 아래키를 누르면 계단 이동시도
            if (Manager.Player.Stats.CurrentNearby is Stair stair)
            {
                Debug.Log("계단 이동 시도");
                if (stair.Interact(transform, goUp))
                {
                    StateChange(State.Stair);
                }
            }

        }

        //z키를 누르면 상호작용 시도
        if (Input.GetKeyDown(KeyCode.Z) && Manager.Player.Stats.CurrentNearby != null)
        {
            if (Manager.Player.Stats.CurrentNearby is Ladder ||
                Manager.Player.Stats.CurrentNearby is Stair ||
                Manager.Player.Stats.CurrentNearby is Hideout)
            {
                Debug.Log("현재 상호작용 대상이 Ladder, Stair, Hideout이므로 Z키 상호작용 차단");
                return;
            }

            GameObject target = (Manager.Player.Stats.CurrentNearby as MonoBehaviour).gameObject;

            Debug.Log("상호작용 시도");
            Manager.Player.Stats.isFarming = true;
            if (target.CompareTag("Door"))
            {
                StartCoroutine(NotRotateAndInteract());
                animator.Play(hashDoor);
            }
            else
            {
                Manager.Sound.SfxPlay(audioClipFarming, transform, 1);
                StartCoroutine(RotateAndInteract());
                animator.Play(hashFarm);
            }
        }

        //space키를 누르면 공격 시도
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("공격 시도");
            _playerAttack.Attack();
        }

        //이외에는 좌우 이동
        MoveSideways();
    }

    void MoveSideways()
    {
        if (Manager.Player.Stats.IsHiding)
        {
            Manager.Sound.SfxStopLoop("Walking", 0f);
            return;
        }

        float h = Input.GetAxis("Horizontal");

        if (Mathf.Abs(h) > 0.1f)
        {
            StateChange(State.Run);

            Manager.Sound.SfxPlayLoop("Walking",audioClipWalking, transform, 1);
        }
        else
        {
            StateChange(State.Idle);

            Manager.Sound.SfxStopLoop("Walking", 0f);
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

            Debug.Log("사다리 자동 이동 완료");

            playerCollider.enabled = true;
            if (_isGrounded)
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
                Debug.Log("등반ㅁㄴㅇㄻㄴㅇㄹ");
                break;
            case State.Stair:
                animator.Play(hashStair);
                break;
            case State.Farm:
                animator.Play(hashFarm);
                break;
            case State.Hide:
                animator.CrossFade(hashHide, .1f);
                animator.SetBool("IsHiding", true);
                Debug.Log("숨기ㅁㄹㄴㅇㄻㅇㄴ");
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
        Vector3 rotation = _renderObject.eulerAngles;

        rotation.y += 90f * scale.x * -1;
        _renderObject.eulerAngles = rotation;
    }

    public void RestoreRotation()
    {
        _renderObject.localRotation = Quaternion.Euler(new Vector3(0, 90, 0));
    }

    private IEnumerator RotateAndInteract()
    {
        RotateToInteract();

        // 슬라이더 UI 활성화 및 초기화
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

        // 파밍 종료
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

    //사운드
    void PlayHideSound()
    {
        Manager.Sound.SfxStopLoop("Walking");
        Manager.Sound.SfxPlayLoop("Hiding", audioClipHiding, transform, 0.3f);
    }
    void StopHideSound()
    {
        Manager.Sound.SfxStopLoop("Hiding", 1.5f);
    }
    void OnDestroy()
    {
        Manager.Player.Stats.OnHideStarted -= PlayHideSound;
        Manager.Player.Stats.OnHideEnded -= StopHideSound;
    }

    //낙하

    private void HandleFalling()
    {
        if (!Manager.Player.Stats.isClimbing)
        {
            animator.Play(hashFall);
            Manager.Sound.SfxPlay(audioClipFalling, transform, .7f);
        }
    }

    private void HandleLanding()
    {
        animator.Play(hashLand);
        Manager.Sound.SfxPlay(audioClipLanding, transform, .7f);
        StartCoroutine(LandingMoveForward(3f, 0.5f)); // (이동 거리, 시간)
    }

    private IEnumerator LandingMoveForward(float distance, float duration)
    {
        float elapsed = 0f;
        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + transform.right * transform.localScale.x * distance;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            transform.position = Vector3.Lerp(startPos, targetPos, t);

            yield return null;
        }

        transform.position = targetPos;
    }
}
