using UnityEngine;

// �÷��̾ ����ó ĳ��� �� ���ų� ������ ����� ����ϴ� ��ũ��Ʈ
public class PlayerHide : MonoBehaviour
{
    private bool isHiding = false;              // ���� �÷��̾ ���� �������� ����
    private HideSpot currentSpot = null;        // ���� �� �� �ִ� ����ó ����
    private Renderer[] renderers;               // �÷��̾� ���� �������� (���� �� ���� ����)
    private CharacterController controller;     // �÷��̾� �̵� ����� ������Ʈ

    // �ٸ� ��ũ��Ʈ���� ��ü ���� ���¸� �������� Ȯ���� �� �ֵ��� ���� ���� �� AI ��� ���
    public static bool IsHidden { get; private set; }

    void Start()
    {
        // �÷��̾� ��ü �� �ڽĿ� ���Ե� ��� ���������� ������
        renderers = GetComponentsInChildren<Renderer>();
        // �̵� ����� CharacterController ������Ʈ�� ������
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // E Ű�� ������, ���� ���� �� �ִ� ������Ʈ�� ��ó�� ���� ��
        if (Input.GetKeyDown(KeyCode.E) && currentSpot != null)
        {
            if (!isHiding)
                EnterHide(); // ����
            else
                ExitHide(); // ������
        }
    }
    void EnterHide()// ���� ���� ����
    {
        isHiding = true;
        IsHidden = true;

        // �÷��̾� ��ġ�� ĳ��� ���� ��ǥ�� �̵���Ŵ
        transform.position = currentSpot.hidePosition.position;
        // ���� �������� ���� �� �÷��̾ �� ���̵���
        SetRenderers(false);

        if (controller != null)// �̵� ��Ȱ��ȭ
            controller.enabled = false;

        Debug.Log("�÷��̾ �������ϴ�");
    }

    void ExitHide()// ���� ���¿��� ������ ������ ����
    {
        isHiding = false;
        IsHidden = false;
        SetRenderers(true);// ���� ������ �ٽ� �ѱ�

        if (controller != null)// �̵� �ٽ� Ȱ��ȭ
            controller.enabled = true;

        Debug.Log("�÷��̾ ���Խ��ϴ�");
    }

    // �÷��̾� ������ ���������� ���� �Ѱų� ���� �Լ�
    void SetRenderers(bool value)
    {
        foreach (var rend in renderers)
        {
            rend.enabled = value;
        }
    }
    // ĳ��� �� ���� ������ ������Ʈ�� ����� �� �ش� ��ġ ����
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out HideSpot spot))
        {
            currentSpot = spot;
        }
    }
    // ĳ��� ��� ����� �� �ش� ��ġ �ʱ�ȭ
    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out HideSpot spot) && currentSpot == spot)
        {
            if (!isHiding) currentSpot = null;
        }
    }
    // ���� ���� ���¸� ��ȯ (�ܺ� ���ٿ�)
    public bool IsHiding()
    {
        return isHiding;
    }
}
