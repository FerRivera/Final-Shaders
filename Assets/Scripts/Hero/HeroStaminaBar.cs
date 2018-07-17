using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroStaminaBar : MonoBehaviour
{
    float _stamina;
    float _maxStamina;
    public Image staminaBar;
    public Text staminaBarNumbers;
    HeroModel _hero;

    void Start ()
    {
        _hero = GetComponent<HeroModel>();
    }
	
	void Update ()
    {
        _stamina = _hero.stamina;
        _maxStamina = _hero.maxStamina;

        if (!pauseGame.paused)
            StaminaBar();
    }

    void StaminaBar()
    {
        _stamina = Mathf.Clamp(_stamina, 0, _maxStamina);
        var speed = 10f;
        staminaBar.fillAmount = Mathf.Lerp(staminaBar.fillAmount, _stamina / _maxStamina, Time.deltaTime * speed);
        staminaBarNumbers.text = _stamina + "/\n" + _maxStamina;
        staminaBarNumbers.color = Color.white;

    }
}
