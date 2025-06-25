using UnityEngine;

public class Test : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float climbSpeed = 3f;

    private Rigidbody rb;
    public float interactDistance = 2f;
    public LayerMask interactableLayer;

    private bool isClimbing = false;
    private Ladder currentLadder;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private bool isAutoClimbing = false;
    private Vector3 climbTargetPos;

    void Update()
    {
        if (isAutoClimbing)
        {
            AutoClimb();
            return;
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
        {
            TryAutoClimb(Input.GetKeyDown(KeyCode.W)); // true면 위로, false면 아래로
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            TryInteract();
        }

        MoveSideways();
    }

    void MoveSideways()
    {
        float h = Input.GetAxis("Horizontal");

        if (h != 0f)
        {
            // 바라보는 방향을 입력값에 따라 좌우로 반전
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(h) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        Vector3 move = Vector3.right * h;
        transform.position += move * moveSpeed * Time.deltaTime;
    }



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
                currentLadder = ladder;

                Transform startPoint = goUp ? ladder.GetBottom() : ladder.GetTop();
                Transform endPoint = goUp ? ladder.GetTop() : ladder.GetBottom();

                // 먼저 시작 지점으로 순간이동
                transform.position = startPoint.position;

                // 이후 도착 지점으로 자동 이동
                climbTargetPos = endPoint.position;

                isAutoClimbing = true;
                rb.useGravity = false;
                rb.velocity = Vector3.zero;

                Debug.Log($"자동 이동 시작: {startPoint.name} → {endPoint.name}");
            }
        }
    }

    void AutoClimb()
    {
        transform.position = Vector3.MoveTowards(transform.position, climbTargetPos, climbSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, climbTargetPos) < 0.05f)
        {
            isAutoClimbing = false;
            rb.useGravity = true;
            currentLadder = null;
            Debug.Log("사다리 자동 이동 완료");
        }
    }
}
