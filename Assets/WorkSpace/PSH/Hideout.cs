using UnityEngine;

public class Hideout : MonoBehaviour,IInteractable
{
    private GameObject _player;
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }
    [SerializeField] float hideDistanceThreshold = 5f; // 은신 불가능 거리 임계값
    public void Interact()
    {
        GameObject nearestZombie = FindNearestZombie();

        if (nearestZombie != null)
        {
            Debug.Log("가장 가까운 좀비는: " + nearestZombie.name);
        }
        else
        {
            Debug.Log("좀비가 없음");
        }

        float distanceToZombie = Vector3.Distance(_player.transform.position, nearestZombie.transform.position);

        if (distanceToZombie < hideDistanceThreshold)
        {
            Debug.Log("좀비가 가까워서 숨을 수 없다");
        }
        else
        {
            Debug.Log("은신 가능");

            Manager.Player.Stats.isHiding = !Manager.Player.Stats.isHiding;

            if (Manager.Player.Stats.isHiding)
            {
                Debug.Log("숨음");
            }
        }       
    }
    GameObject FindNearestZombie()
    {
        GameObject[] zombies = GameObject.FindGameObjectsWithTag("Zombie");
        GameObject closest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject zombie in zombies)
        {
            float distance = Vector3.Distance(Manager.Player.transform.position, zombie.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = zombie;
            }
        }

        return closest;
    }
}
