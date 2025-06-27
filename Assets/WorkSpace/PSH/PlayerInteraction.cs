using System.Collections;
using UnityEngine;

public partial class PlayerStats
{
    public bool isFarming = false;//파밍중인지 나타내는 불값
    public IInteractable CurrentNearby;//가까운 상호작용 대상
}
public class PlayerInteraction : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float climbSpeed = 3f;

    private Rigidbody rb;//필요없을 거 같은데 굳이 없앨 이유도 없을거같음
    private Collider playerCollider;

    //나중에 문제없다면 밑에 2줄 지울것
    public float interactDistance = 2f;
    public LayerMask interactableLayer;

    private bool isAutoClimbing = false;
    private Vector3 climbTargetPos;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
        Manager.Player.Stats.isFarming = false;
    }

    void Update()
    {
        //파밍중에는 x키로 닫기 전까지는 다른 키 입력 불가
        if (Manager.Player.Stats.isFarming)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                Manager.Player.Stats.isFarming = false;
            }
            return;
        }

        //등반 상태라면 등반함
        if (isAutoClimbing)
        {
            AutoClimb();
            return;
        }

        //위아래 키를 누르면 사다리 이동 시도
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            //TryAutoClimb(Input.GetKeyDown(KeyCode.UpArrow)); // true = 위로
            bool goUp = Input.GetKeyDown(KeyCode.UpArrow);

            if (Manager.Player.Stats.CurrentNearby is Ladder ladder)
            {
                ladder.RequestClimb(transform, goUp, climbSpeed, this);
            }
        }

        //z키를 누르면 상호작용 시도
        if (Input.GetKeyDown(KeyCode.Z) && Manager.Player.Stats.CurrentNearby != null)
        {
            Manager.Player.Stats.CurrentNearby.Interact();
            Debug.Log("상호작용 실행");
        }

        //이외에는 좌우 이동
        MoveSideways();
    }

    void MoveSideways()
    {
        float h = Input.GetAxis("Horizontal");

        if (h != 0f)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(h) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        Vector3 move = Vector3.right * h;
        transform.position += move * moveSpeed * Time.deltaTime;
    }
    //안씀
    /*
    void TryInteract()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * 1f;
        float dirX = Mathf.Sign(transform.localScale.x);
        Vector3 rayDir = new Vector3(dirX, 0f, 0f);
        Ray ray = new Ray(rayOrigin, rayDir);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactableLayer, QueryTriggerInteraction.Collide))
        {
            IInteractable target = hit.collider.GetComponentInParent<IInteractable>();
            if (target != null)
                target.Interact();

            Debug.Log("Hit: " + hit.collider.name);
        }
        else
        {
            Debug.Log("Ray가 아무것도 못 맞춤");
        }
    }
    */
    //안씀
    /*
    void TryAutoClimb(bool goUp)
    {
        Vector3 rayOrigin = transform.position + Vector3.up * 1f;
        float dirX = Mathf.Sign(transform.localScale.x);
        Vector3 rayDir = new Vector3(dirX, 0f, 0f);
        Ray ray = new Ray(rayOrigin, rayDir);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactableLayer, QueryTriggerInteraction.Collide))
        {
            Ladder ladder = hit.collider.GetComponentInParent<Ladder>();
            if (ladder != null)
            {

                Transform startPoint = goUp ? ladder.GetBottom() : ladder.GetTop();
                Transform endPoint = goUp ? ladder.GetTop() : ladder.GetBottom();

                transform.position = startPoint.position;
                climbTargetPos = endPoint.position;

                isAutoClimbing = true;
                rb.useGravity = false;
                rb.velocity = Vector3.zero;

                playerCollider.enabled = false; // 콜라이더 비활성화

                Debug.Log($"자동 이동 시작: {startPoint.name} → {endPoint.name}");
            }
        }
    }
    */
    public void StartClimb(Vector3 from, Vector3 to)
    {
        transform.position = from;
        climbTargetPos = to;
        isAutoClimbing = true;

        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        playerCollider.enabled = false;
    }
    void AutoClimb()
    {
        transform.position = Vector3.MoveTowards(transform.position, climbTargetPos, climbSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, climbTargetPos) < 0.05f)
        {
            isAutoClimbing = false;
            rb.useGravity = true;

            Debug.Log("사다리 자동 이동 완료");

            StartCoroutine(ReenableColliderAfterDelay());
        }
    }

    IEnumerator ReenableColliderAfterDelay()
    {
        yield return new WaitForSeconds(0.1f); // 짧은 지연 후
        playerCollider.enabled = true;
    }
}
