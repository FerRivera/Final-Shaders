using UnityEngine;
using System.Collections;

public class VampireTouch : MonoBehaviour
{
    HeroModel _hero;
    public int liveTime;
    float damageToIncrease;
    float currentDamage;

	// Use this for initialization
	void Start () {

        _hero = Finder.Instance.hero;   
        StartCoroutine(Dead(liveTime));
	}

	
	// Update is called once per frame
	void Update ()
    {
        if(_hero.damage != currentDamage)
        {
            damageToIncrease = 10 * _hero.damage / 100;
            _hero.damage += (int)damageToIncrease;
            currentDamage = _hero.damage;
        }
	}

    IEnumerator Dead(int deadTime)
    {       
        yield return new WaitForSeconds(deadTime);
        
        _hero.damage -= (int)damageToIncrease;
        Destroy(this.gameObject);
    }


}
