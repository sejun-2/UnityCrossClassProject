using UnityEngine;

public class Test : MonoBehaviour
{
   [SerializeField] Zombie zombie;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            zombie.TakeDamage(1000f);
        }
    }
}
