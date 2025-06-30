using UnityEngine;

public class Hideout : MonoBehaviour,IInteractable
{
    private GameObject _player;
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }
    [SerializeField] float hideDistanceThreshold = 5f; // ���� �Ұ��� �Ÿ� �Ӱ谪
    public void Interact()
    {
        GameObject nearestZombie = FindNearestZombie();

        if (nearestZombie != null)
        {
            Debug.Log("���� ����� �����: " + nearestZombie.name);
        }
        else
        {
            Debug.Log("���� ����");
        }

        float distanceToZombie = Vector3.Distance(_player.transform.position, nearestZombie.transform.position);

        if (distanceToZombie < hideDistanceThreshold)
        {
            Debug.Log("���� ������� ���� �� ����");
        }
        else
        {
            Debug.Log("���� ����");

            Manager.Player.Stats.isHiding = !Manager.Player.Stats.isHiding;

            if (Manager.Player.Stats.isHiding)
            {
                Debug.Log("����");
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
