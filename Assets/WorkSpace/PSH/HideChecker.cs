using UnityEngine;

public class HideChecker : MonoBehaviour
{
    private GameObject _player;
   [SerializeField] float hideDistanceThreshold = 5f;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");

        if (_player == null)
        {
            Debug.LogError("플레이어를 찾을 수 없습니다. Player 태그를 확인해주세요.");
        }
    }
    public bool CanHide()
    {
        GameObject[] zombies = GameObject.FindGameObjectsWithTag("Zombie");

        foreach (GameObject zombieObj in zombies)
        {
            Zombie zombie = zombieObj.GetComponent<Zombie>();
            if (zombie != null)
            {
                float distance = Vector3.Distance(_player.transform.position, zombieObj.transform.position);
                Debug.Log($"{zombieObj.name} 거리: {distance}, 상태: {zombie.CurrentState}");

                if ((zombie.CurrentState == Zombie.State.Chase || zombie.CurrentState == Zombie.State.Attack)
                    && distance < hideDistanceThreshold)
                {
                    Debug.Log("canhide is false");
                    return false;
                }
            }
        }
        Debug.Log("canhide is true");
        return true;
    }
}
