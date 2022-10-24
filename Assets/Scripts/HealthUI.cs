using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Image fill;
    public Slider slider;
    public Gradient gradient;

    private void Start() {
        slider = GetComponent<Slider>();
    }

    public void SetMaxHealth(int maxHealth) {
        slider.maxValue = maxHealth;
        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(int health) {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
