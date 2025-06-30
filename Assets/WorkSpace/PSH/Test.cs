using UnityEngine;

public class Test : MonoBehaviour
{
   [SerializeField] Zombie zombie;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            zombie.TakeDamage(1000f);
            Debug.Log("");
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            
            Debug.Log($"현재 플레이어가 {Manager.Player.Stats.CurrentNearby}에 있음");
        }
    }
}
