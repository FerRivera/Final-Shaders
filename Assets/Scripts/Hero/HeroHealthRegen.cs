using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroHealthRegen : MonoBehaviour
{
    float _health;
    float _maxHealth;
    public float timeToRecoverHealth;
    public float healthRegen;
    HeroModel _hero;

    void Start ()
    {
        _hero = GetComponent<HeroModel>();
        _health = _hero.health;
        _maxHealth = _hero.maxHealth;
        StartCoroutine(HealthRegenCO());
    }
	
	void Update ()
    {
		
	}

    void HealthRegeneration()
    {
        _health = _hero.health;
        _maxHealth = _hero.maxHealth;

        if (_health > 0)
        {
            _health = Mathf.Clamp(_health, 0, _maxHealth);
            _health += healthRegen;
            _hero.health = _health;
        }

        if (_hero.health > _hero.maxHealth)
            _hero.health = _hero.maxHealth;
    }

    IEnumerator HealthRegenCO()
    {
        yield return new WaitForSeconds(timeToRecoverHealth);
        HealthRegeneration();
        StartCoroutine(HealthRegenCO());
    }
}
