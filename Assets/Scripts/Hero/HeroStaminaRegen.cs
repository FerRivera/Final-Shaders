using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroStaminaRegen : MonoBehaviour
{
    float _health;
    float _stamina;
    float _maxStamina;
    public float timeToRecoverStamina;
    public float staminaRegen;
    bool _recoverStamina;
    float _currentTime;
    HeroModel _hero;

    void Start()
    {
        _hero = GetComponent<HeroModel>();
        _stamina = _hero.stamina;
        _health = _hero.health;
        _maxStamina = _hero.maxStamina;
        StartCoroutine(StaminaRegenCO());
    }

    void Update()
    {
        _recoverStamina = _hero.recoverStamina;

        if (_recoverStamina)
        {
            _currentTime += Time.deltaTime;            
        }
        else
        {
            _currentTime = 0;
        }
    }

    void StaminaRegeneration()
    {
        _stamina = _hero.stamina;
        _maxStamina = _hero.maxStamina;
        _health = _hero.health;

        if (_health > 0 && _currentTime >= timeToRecoverStamina)
        {
            _currentTime = 0;
            _stamina = Mathf.Clamp(_stamina, 0, _maxStamina);
            _stamina += staminaRegen;
            _hero.stamina = _stamina;
        }
    }

    IEnumerator StaminaRegenCO()
    {
        yield return new WaitForSeconds(timeToRecoverStamina);      
        StaminaRegeneration();
        StartCoroutine(StaminaRegenCO());
    }
}
