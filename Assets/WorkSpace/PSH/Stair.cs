using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Stair : MonoBehaviour, IInteractable
{
    private Transform pointUp;   // ���� ����Ʈ
    private Transform pointDown; // �Ʒ��� ����Ʈ

    [SerializeField] private Image fadeImage; // ���� �г� �̹���
    [SerializeField] private float fadeDuration = 0.5f;

    [SerializeField] AudioClip audioClip;

    private void Start()
    {
        // �ڽ� �� �̸����� pointUp, pointDown ã��
        Transform[] children = GetComponentsInChildren<Transform>();

        foreach (Transform child in children)
        {
            if (child.name == "PointUp")
                pointUp = child;
            else if (child.name == "PointDown")
                pointDown = child;
        }

        // Player �±� ������Ʈ���� FadeImage ã��
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            Image image = playerObj.GetComponentInChildren<Image>();

            if (image != null)
            {
                fadeImage = image;
            }
            else
            {
                Debug.LogError("FadeImage��� �̸��� �ڽ� ������Ʈ�� Player �ȿ��� ã�� �� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogError("Player �±� ������Ʈ�� ã�� �� �����ϴ�.");
        }
    }
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
        Manager.Player.Stats.isFarming = true;
        Transform target = goUp ? pointUp : pointDown;
        StartCoroutine(FadeTeleport(player, target.position));
        Manager.Sound.SfxPlay(audioClip,transform,1);
        return true;
    }

    private IEnumerator FadeTeleport(Transform player, Vector3 targetPos)
    {
        yield return new WaitForSeconds(1f);
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
        if(targetAlpha != 0)
        {
            Manager.Player.Stats.isFarming = false;
        }

    }

    // IInteractable �⺻ ����
    public void Interact()
    {
        Debug.Log("��� ��ȣ�ۿ� - Up/Down ����Ű �Է� �ʿ�.");
    }
}
