using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Stair : MonoBehaviour, IInteractable
{
    private Transform pointUp;   // 위쪽 포인트
    private Transform pointDown; // 아래쪽 포인트

    [SerializeField] private Image fadeImage; // 검정 패널 이미지
    [SerializeField] private float fadeDuration = 0.5f;

    [SerializeField] AudioClip audioClip;

    private void Start()
    {
        // 자식 중 이름으로 pointUp, pointDown 찾기
        Transform[] children = GetComponentsInChildren<Transform>();

        foreach (Transform child in children)
        {
            if (child.name == "PointUp")
                pointUp = child;
            else if (child.name == "PointDown")
                pointDown = child;
        }

        // Player 태그 오브젝트에서 FadeImage 찾기
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
                Debug.LogError("FadeImage라는 이름의 자식 오브젝트를 Player 안에서 찾을 수 없습니다.");
            }
        }
        else
        {
            Debug.LogError("Player 태그 오브젝트를 찾을 수 없습니다.");
        }
    }
    public bool Interact(Transform player, bool goUp)
    {
        float distToUp = Vector3.Distance(player.position, pointUp.position);
        float distToDown = Vector3.Distance(player.position, pointDown.position);

        // 위로 가려는데 이미 위에 있음
        if (goUp && distToUp < distToDown)
        {
            Debug.Log("이미 계단 위에 있음. 위로 이동 불가.");
            return false;
        }

        // 아래로 가려는데 이미 아래에 있음
        if (!goUp && distToDown < distToUp)
        {
            Debug.Log("이미 계단 아래에 있음. 아래로 이동 불가.");
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
        yield return StartCoroutine(Fade(1)); // 페이드 아웃

        player.position = targetPos;

        yield return StartCoroutine(Fade(0)); // 페이드 인
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

    // IInteractable 기본 구현
    public void Interact()
    {
        Debug.Log("계단 상호작용 - Up/Down 방향키 입력 필요.");
    }
}
