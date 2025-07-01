using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

public class PlayerHide : MonoBehaviour
{
    private bool isHiding = false;             // ���� ���� ���� ����
    private HideSpot currentSpot = null;       // ���� ���� �� �ִ� ���
    private Renderer[] renderers;              // �÷��̾� ��������
    private CharacterController controller;    // �÷��̾� �̵� ����� (�Ǵ� ���� ������ �̵� ��ũ��Ʈ)

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        controller = GetComponent<CharacterController>(); // �Ǵ� Rigidbody, ���� ���� PlayerMovement ��
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentSpot != null)
        {
            if (!isHiding)
                EnterHide();
            else
                ExitHide();
        }
    }

    // ĳ��� ������ ����
    void EnterHide()
    {
        isHiding = true;

        // �÷��̾� ��ġ�� ĳ��� ������ �̵�
        transform.position = currentSpot.hidePosition.position;

        // ������ ��Ȱ��ȭ (������ �ʰ�)
        SetRenderers(false);

        // �̵� ����
        if (controller != null)
            controller.enabled = false;

        Debug.Log("�÷��̾ �������ϴ�");
    }

    // ������ ������
    void ExitHide()
    {
        isHiding = false;

        SetRenderers(true);

        if (controller != null)
            controller.enabled = true;

        Debug.Log("�÷��̾ ���Խ��ϴ�");
    }

    // ������ ��ü �Ѱų� ����
    void SetRenderers(bool value)
    {
        foreach (var rend in renderers)
        {
            rend.enabled = value;
        }
    }

    // ĳ��ְ� �浹�ϸ� ����صα�
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out HideSpot spot))
        {
            currentSpot = spot;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out HideSpot spot) && currentSpot == spot)
        {
            if (!isHiding) currentSpot = null;
        }
    }

    public bool IsHiding()
    {
        return isHiding;
    }
}
