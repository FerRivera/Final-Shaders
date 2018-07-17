using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroHealthBar : MonoBehaviour
{
    float _health;
    float _maxHealth;
    public Image healthBar;
    public Text healthBarNumbers;
    HeroModel _hero;

    void Start ()
    {
        _hero = GetComponent<HeroModel>();
    }
	
	void Update ()
    {
        _health = _hero.health;
        _maxHealth = _hero.maxHealth;

        if (!pauseGame.paused)
            HealthBar();
    }

    void HealthBar()
    {
        _health = Mathf.Clamp(_health, 0, _maxHealth);
        var speed = 10f;
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, _health / _maxHealth, Time.deltaTime * speed);
        healthBarNumbers.text = _health + "/\n" + _maxHealth;
        healthBarNumbers.color = Color.white;
    }
}
