using System.Collections;
using UnityEngine;

public partial class PlayerStats
{
    public bool isFarming = false;//�Ĺ������� ��Ÿ���� �Ұ�
    public bool isHiding = false;//��������
    public IInteractable CurrentNearby;//����� ��ȣ�ۿ� ���
}
public class PlayerInteraction : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float climbSpeed = 3f;

    private Rigidbody rb;//�ʿ���� �� ������ ���� ���� ������ �����Ű���
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
        //�Ĺ��߿��� xŰ�� �ݱ� �������� �ٸ� Ű �Է� �Ұ�
        if (Manager.Player.Stats.isFarming)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                Manager.Player.Stats.isFarming = false;
            }
            return;
        }

        //�����߿��� uparrowŰ�� ���� Ǯ�� �������� �ٸ� Ű �Է� �Ұ�
        if (Manager.Player.Stats.isHiding)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Z))//zŰ �ϴ� �߰��� �غ�
            {
                Manager.Player.Stats.isHiding = false;
            }
            return;
        }

        //��� ���¶�� �����
        if (isAutoClimbing)
        {
            AutoClimb();
            return;
        }

        //���Ʒ� Ű�� ������ ��ٸ� �̵� �õ�
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            bool goUp = Input.GetKeyDown(KeyCode.UpArrow);

            
            if (Manager.Player.Stats.CurrentNearby is Ladder ladder)
            {
                Debug.Log("��ٸ� �̵� �õ�");
                ladder.Interact(transform, goUp, climbSpeed, this);
            }
        }

        //zŰ�� ������ ��ȣ�ۿ� �õ�
        if (Input.GetKeyDown(KeyCode.Z) && Manager.Player.Stats.CurrentNearby != null)
        {
            Manager.Player.Stats.CurrentNearby.Interact();
            Debug.Log("��ȣ�ۿ� ����");
        }

        //uparrowŰ�� ������ ���� �õ�
        if (Input.GetKeyDown(KeyCode.UpArrow) && Manager.Player.Stats.CurrentNearby != null)
        {
            Manager.Player.Stats.CurrentNearby.Interact();
            Debug.Log("���� ����");
        }

        //spaceŰ�� ������ ���� �õ�
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            Debug.Log("���� ����");
        }

        //�̿ܿ��� �¿� �̵�
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

            Debug.Log("��ٸ� �ڵ� �̵� �Ϸ�");

            StartCoroutine(ReenableColliderAfterDelay());
        }
    }

    IEnumerator ReenableColliderAfterDelay()
    {
        yield return new WaitForSeconds(0.1f); // ª�� ���� ��
        playerCollider.enabled = true;
    }
}
