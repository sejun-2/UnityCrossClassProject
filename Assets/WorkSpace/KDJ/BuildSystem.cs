using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾ �ٸ����̵带 ��ġ�� �� �ֵ��� ���ִ� �Ǽ� �ý���
public class BuildSystem : MonoBehaviour
{
    public GameObject barricadePrefab;// ��ġ�� �ٸ����̵� ������ 
    public LayerMask groundLayer;// �ٴ����� �νĵ� ���̾� (Ray�� �¾ƾ� ��ġ��)
    public KeyCode buildKey = KeyCode.B;// ��ġŰ �⺻: B Ű
    void Update()
    {
        if (Input.GetKeyDown(buildKey))// ������ ��ġ Ű�� ������ �� ����
        {
            // ī�޶󿡼� ���콺 ��ġ �������� ����(Ray)�� ��
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // Ray�� groundLayer�� ���� ������Ʈ�� �ε������� �˻� (�ִ� 10���� �Ÿ�)
            if (Physics.Raycast(ray, out RaycastHit hit, 10f, groundLayer))
            {
                // �ε��� ��ġ�� �ٸ����̵带 ���� (ȸ�� ���� ����)
                Instantiate(barricadePrefab, hit.point, Quaternion.identity);
            }
        }
    }
}
