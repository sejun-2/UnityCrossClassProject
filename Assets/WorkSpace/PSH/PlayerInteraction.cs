using System.Collections;
using UnityEngine;

public partial class PlayerStats
{
    public bool isFarming = false;//파밍중인지 나타내는 불값
    public bool isHiding = false;//숨었는지
    public IInteractable CurrentNearby;//가까운 상호작용 대상
}
public class PlayerInteraction : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float climbSpeed = 3f;

    private Rigidbody rb;//필요없을 거 같은데 굳이 없앨 이유도 없을거같음
    private Collider playerCollider;

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

        //은신중에는 uparrow키로 은신 풀기 전까지는 다른 키 입력 불가
        if (Manager.Player.Stats.isHiding)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Z))//z키 일단 추가는 해봄
            {
                Manager.Player.Stats.isHiding = false;
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
            bool goUp = Input.GetKeyDown(KeyCode.UpArrow);

            
            if (Manager.Player.Stats.CurrentNearby is Ladder ladder)
            {
                Debug.Log("사다리 이동 시도");
                ladder.Interact(transform, goUp, climbSpeed, this);
            }
        }

        //z키를 누르면 상호작용 시도
        if (Input.GetKeyDown(KeyCode.Z) && Manager.Player.Stats.CurrentNearby != null)
        {
            Manager.Player.Stats.CurrentNearby.Interact();
            Debug.Log("상호작용 실행");
        }

        //uparrow키를 누르면 은신 시도
        if (Input.GetKeyDown(KeyCode.UpArrow) && Manager.Player.Stats.CurrentNearby != null)
        {
            Manager.Player.Stats.CurrentNearby.Interact();
            Debug.Log("은신 실행");
        }

        //space키를 누르면 공격 시도
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            Debug.Log("공격 실행");
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
