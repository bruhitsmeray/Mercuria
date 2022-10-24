using UnityEngine;

public class VitalsManager : MonoBehaviour
{
    public HealthUI healthUI;
    private bool _isDead;
    private int health;
    
    [Header("Health")]
    public int maxHealth = 100;
    
    private void Start() {
        health = maxHealth;
        if (healthUI) {
            healthUI.SetHealth(health);
            healthUI.SetMaxHealth(maxHealth);
        } else {
            Debug.LogWarning("HealthUI is unavailable. Unable to set the Health/MaxHealth.");
        }
    }
    
    private void Update() {
        if (!_isDead) {
            Debug.Log(health + "/" + maxHealth);
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            TakeDamage(10);
        }
    }
    
    public void TakeDamage(int hitDamage) {
        health = Mathf.Clamp(health - hitDamage, 0, maxHealth);
        if (healthUI) {
            healthUI.SetHealth(health);
        } else {
            Debug.LogWarning("HealthUI is unavailable. Unable to set the Health.");
        }
        
        if (health <= 0) {
            if (!_isDead) {
                _isDead = true;
                Debug.Log("You are dead. Source: trust me bro.");
            }
        } else {
            Debug.Log("You took damage. Source: trust me bro.");
        }
    }
}
