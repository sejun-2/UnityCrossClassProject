using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public int hp = 50;
    public int maxHp = 100;

    public int hunger = 50;
    public int maxHunger = 100;

    public int thirst = 50;
    public int maxThirst = 100;

    

    public void AddHp(int amount)
    {
        hp = Mathf.Clamp(hp + amount, 0, maxHp);
        Debug.Log($"[HP] {hp}/{maxHp}");
    }

    public void AddHunger(int amount)
    {
        hunger = Mathf.Clamp(hunger + amount, 0, maxHunger);
        Debug.Log($"[Hunger] {hunger}/{maxHunger}");
    }

    public void AddThirst(int amount)
    {
        thirst = Mathf.Clamp(thirst + amount, 0, maxThirst);
        Debug.Log($"[Thirst] {thirst}/{maxThirst}");
    }
    
}
