using UnityEngine;

public class VitalsManager : MonoBehaviour
{
    public HealthUI healthUI;
    private bool _isDead;
    private int health;
    
    [Header("Health")]
    public int maxHealth = 25;
    
    private void Start() {
        health = maxHealth;
        healthUI.SetHealth(health);
        healthUI.SetMaxHealth(maxHealth);
    }
    
    void Update() {
        if (!_isDead)
        {
            Debug.Log(health + "/" + maxHealth);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            TakeDamage(10);
        }
    }
    
    public void TakeDamage(int hitDamage) {
        health = Mathf.Clamp(health - hitDamage, 0, maxHealth);
        healthUI.SetHealth(health);

        if (health <= 0) {
            if (!_isDead)
            {
                _isDead = true;
                Debug.Log("You are dead. Source: trust me bro.");
            }
        } else {
            Debug.Log("You took damage. Source: trust me bro.");
        }
    }
}
