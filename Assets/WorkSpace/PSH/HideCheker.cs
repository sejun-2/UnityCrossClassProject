using UnityEngine;

public class HideCheker : MonoBehaviour
{
    public GameObject player;
    public float hideDistanceThreshold = 5f;

    public bool CanHide()
    {
        GameObject[] zombies = GameObject.FindGameObjectsWithTag("Zombie");

        foreach (GameObject zombieObj in zombies)
        {
            Zombie zombie = zombieObj.GetComponent<Zombie>();
            if (zombie != null)
            {
                float distance = Vector3.Distance(player.transform.position, zombieObj.transform.position);

                if (zombie._currentState == Zombie.State.Chase && distance < hideDistanceThreshold)
                {
                    Debug.Log("추격 중인 좀비가 가까워서 숨을 수 없음");
                    return false;
                }
            }
        }

        Debug.Log("은신 가능");
        return true;
    }
}
