using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Heal : SkillSelfCast
{
    public Image underBarInstance;
    public bool overTime;
    private Image _uiInstance;
    public float timeHealOverTimeActual;
    [Range(0.5f,4)]
    public float timeHealOverTimeTotal;
    float healAmount;
    //float saveHp;
    bool destroyed;
    Item item;
    public float posY;

    protected override void Start ()
    {
        base.Start();
        item = _itemsDatabase.FetchItemByID(1);
        healAmount = (int)(caster.maxHealth * item.Health) / 100;
        caster.saveHp = caster.health;
        caster.healing = true;
        transform.SetParent(caster.transform);

        if (!overTime)
        {
            Death();
            caster.health += healAmount;
            if (caster.health >= caster.maxHealth)
            {
                caster.health = caster.maxHealth;
            }
        }
        else
        {
            StartCoroutine(HealFixed());
            _uiInstance = Instantiate(underBarInstance);
            //Utility.instance.SetUIPosition(_uiInstance.gameObject);            
        }
        transform.position = new Vector3(caster.transform.position.x, posY, caster.transform.position.z);        
	}
	
	protected override void Update()
    {
        base.Update();
	    if(overTime && timeHealOverTimeActual >= timeHealOverTimeTotal)
        {
            if(!destroyed)
            {
                StopCoroutine(HealFixed());

                //Utility.instance.RemoveUIPosition(_uiInstance.gameObject);
                caster.healing = false;
                Death();
                destroyed = true;
            }
         
        }
        else if(overTime)
        {
            timeHealOverTimeActual += Time.deltaTime;
        }
    }

    IEnumerator HealFixed()
    {
        while (timeHealOverTimeActual < timeHealOverTimeTotal)
        {
            yield return new WaitForEndOfFrame();

            var clampedvalue = (((healAmount / timeHealOverTimeTotal) * Time.deltaTime));
            caster.saveHp += clampedvalue;
            caster.health = (int)caster.saveHp;

            if (caster.health >= caster.maxHealth)
            {
                caster.health = caster.maxHealth;
            }


        }
    }
}
