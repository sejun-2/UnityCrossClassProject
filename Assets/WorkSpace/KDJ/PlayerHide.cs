using UnityEngine;

// �ڵ� ����	���� �ð� ���� ���� �ڵ����� ������ �ý���
// �׸��� �� AI ����	IsHiding() ���� �� AI�� �ν����� �ʵ��� ��� ���� �׸���
// ���� ���� �ð� & �Ƿε� �Ҹ� �׸��� �ʸ� Ž�� ����	Physics.Raycast�� ��ֹ� Ž��
// �÷��̾ ����óĳ��ֿ� �� ����, �ڵ����� ������ ����� ������ ��ũ��Ʈ
public class PlayerHide : MonoBehaviour
{
    private bool isHiding = false;// ���� ���� ������ ����
    private HideSpot currentSpot = null;// �÷��̾ ������ ����ó ����
    private Renderer[] renderers;// �÷��̾��� �������� �������� ��Ȱ��ȭ
    private CharacterController controller;// �̵� ����� ������Ʈ

    public float maxHideTime = 10f;// �ִ� ���� ���� �ð� 
    public float fatiguePerSecond = 5f;// ���� �� �ʴ� �Ƿε� ���ҷ�

    private float hideTimer = 0f;// ���� ���� �ð� ����
    private PlayerStats stats;// �Ƿε� �� �÷��̾� ���� ���� ��ũ��Ʈ ����

    public static bool IsHidden { get; private set; } // �ٸ� Ŭ�������� ���� ���� Ȯ�ο�

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();// �÷��̾��� ��� ������ ����
        controller = GetComponent<CharacterController>();// �̵� ���� ������Ʈ ����
        stats = GetComponent<PlayerStats>();// �Ƿε� ������ ��ũ��Ʈ ����
    }

    void Update()
    {
        // E Ű�� ������ ����ó ��ó�� ������ ���ų� ����
        if (Input.GetKeyDown(KeyCode.E) && currentSpot != null)
        {
            if (!isHiding)
                EnterHide(); // ����
            else
                ExitHide();  // ������
        }

        if (isHiding)// ���� ���� ���: Ÿ�̸� �� �Ƿε� ���� ó��
        {
            hideTimer += Time.deltaTime; // ��� �ð� ����

            if (stats != null)// �Ƿε� ����
                stats.ReduceFatigue(fatiguePerSecond * Time.deltaTime);

            if (hideTimer >= maxHideTime)// ���� �ð��� ������ �ڵ����� ����
            {
                ExitHide();
                Debug.Log("���� �ð� �ʰ��� �ڵ� ����");
            }
        }
    }

    void EnterHide()// ���� ���� ó��
    {
        isHiding = true;// ���� ���� ON
        IsHidden = true;// �ܺο����� ���� ���·� �ν� ����
        hideTimer = 0f;// ���� Ÿ�̸� �ʱ�ȭ

        // ���� ��ġ�� �̵� ĳ�����
        transform.position = currentSpot.hidePosition.position;

        SetRenderers(false);// ������ ����-������ �ʰ�
        if (controller != null) controller.enabled = false; // ������ ����

        Debug.Log("����");
    }

    void ExitHide()// ���� ���� ó��
    {
        isHiding = false;// ���� ���� OFF
        IsHidden = false;

        SetRenderers(true);// �ٽ� ���̰� �ϱ�
        if (controller != null) controller.enabled = true;  // ������ ���

        Debug.Log("����");
    }

    // �÷��̾��� ���������� �Ѱų� ���� �Լ�
    void SetRenderers(bool visible)
    {
        foreach (var rend in renderers)
        {
            rend.enabled = visible;
        }
    }

    // ����ó�� ������ �� �ش� ����ó ���� ����
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out HideSpot spot))
        {
            currentSpot = spot;
        }
    }

    // ����ó�� ����� �� ���� ���� �����ִ� ��쿣 ����
    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out HideSpot spot) && currentSpot == spot && !isHiding)
        {
            currentSpot = null;
        }
    }

    // �ܺο��� �÷��̾��� ���� ���¸� Ȯ���� �� �ִ� �Լ�
    public static bool IsHiding()
    {
        return IsHidden;
    }
}

