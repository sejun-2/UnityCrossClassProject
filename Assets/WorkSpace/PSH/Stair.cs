using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Stair : MonoBehaviour, IInteractable
{
    public Transform pointUp;   // ���� ����Ʈ
    public Transform pointDown; // �Ʒ��� ����Ʈ

    [SerializeField] private Image fadeImage; // ���� �г� �̹���
    [SerializeField] private float fadeDuration = 0.5f;

    public bool Interact(Transform player, bool goUp)
    {
        float distToUp = Vector3.Distance(player.position, pointUp.position);
        float distToDown = Vector3.Distance(player.position, pointDown.position);

        // ���� �����µ� �̹� ���� ����
        if (goUp && distToUp < distToDown)
        {
            Debug.Log("�̹� ��� ���� ����. ���� �̵� �Ұ�.");
            return false;
        }

        // �Ʒ��� �����µ� �̹� �Ʒ��� ����
        if (!goUp && distToDown < distToUp)
        {
            Debug.Log("�̹� ��� �Ʒ��� ����. �Ʒ��� �̵� �Ұ�.");
            return false;
        }

        Transform target = goUp ? pointUp : pointDown;
        StartCoroutine(FadeTeleport(player, target.position));
        return true;
    }

    private IEnumerator FadeTeleport(Transform player, Vector3 targetPos)
    {
        yield return StartCoroutine(Fade(1)); // ���̵� �ƿ�

        player.position = targetPos;

        yield return StartCoroutine(Fade(0)); // ���̵� ��
    }

    private IEnumerator Fade(float targetAlpha)
    {
        Color c = fadeImage.color;
        float startAlpha = c.a;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            c.a = Mathf.Lerp(startAlpha, targetAlpha, timer / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        c.a = targetAlpha;
        fadeImage.color = c;
    }

    // IInteractable �⺻ ����
    public void Interact()
    {
        Debug.Log("��� ��ȣ�ۿ� - Up/Down ����Ű �Է� �ʿ�.");
    }
}
