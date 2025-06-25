using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;//�ȱ� �ӵ�
    public float climbSpeed = 3f; // ��ٸ� Ÿ�� �ӵ�
    // �÷��̾� Rigidbody
    private Rigidbody rb;

    public Transform cam;// ī�޶� ���� (�̵� ���� �� ȸ���� ���)
    private bool isClimbing = false;// ��ٸ��� �پ� �ִ��� ���θ� �Ǵ��ϴ� �÷���

    void Start()
    {
        rb = GetComponent<Rigidbody>();// Rigidbody ������Ʈ ��������
        Cursor.lockState = CursorLockMode.Locked;// ���콺 Ŀ���� ȭ�� �߾ӿ� ����
    }

    void Update()
    {
        // ��ٸ��� �پ� �ִ� ��� ��/�Ʒ� �̵�
        if (isClimbing)
        {
            Climb();
        }
        // �ƴ� ��� �¿� �̵�
        else
        {
            MoveSideways();
        }
        Rotate();// �׻� ī�޶� ������ ���� ȸ��
    }
    void MoveSideways()
    {
        // A/D Ű �Ǵ�  ���ʿ����� �Է°� (-1 ~ 1)
        float h = Input.GetAxis("Horizontal");

        // ī�޶��� ������ ������ �������� �̵� ���� ���
        Vector3 move = cam.right * h;
        move.y = 0;// Y��(����)�� �����ؼ� ���� �̵��� �ϵ��� ����
        transform.position += move.normalized * moveSpeed * Time.deltaTime; // ���� �̵� ó��
    }

    // ��ٸ����� ��/�Ʒ� �̵� ó�� �Լ�
     void Climb()
    {
        // W/S Ű �Ǵ� ���Ʒ� �Է°� (-1 ~ 1)
        float v = Input.GetAxis("Vertical");
        Vector3 climb = Vector3.up * v;// �� �������θ� �̵� ���� ����
        transform.position += climb * climbSpeed * Time.deltaTime;// ���� �̵� ó��
    }

    // ī�޶� ������ �������� �÷��̾� ȸ�� ó��
    void Rotate()
    {
        Vector3 lookDir = cam.forward;// ī�޶��� ���� ������ �������� ȸ��
        lookDir.y = 0;// ��/�Ʒ� ���� ���� (Y���� ȸ������ ����)
        transform.forward = lookDir; // �÷��̾��� �� ���� ����
    }

    // ��ٸ� ������ ���� �� ȣ��� 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            isClimbing = true;         // ��ٸ� ���� Ȱ��ȭ
            rb.useGravity = false;     // �߷� ��Ȱ��ȭ 
            rb.velocity = Vector3.zero; // ���� �ӵ� ����
        }
    }
    private void OnTriggerExit(Collider other)// ��ٸ� �������� ������ �� ȣ���
    {
        // �±װ� "Ladder"�� ������Ʈ���� ��� ���
        if (other.CompareTag("Ladder"))
        {
            isClimbing = false;       // ��ٸ� ���� ����
            rb.useGravity = true;     // �߷� �ٽ� Ȱ��ȭ
        }
    }
}
