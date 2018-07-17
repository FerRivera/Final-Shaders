using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    protected ItemDatabase _itemsDatabase;
    protected Color _textColor;
    protected float _damage;
    public HeroModel caster;
    public int timeToDestroy;
    public float timeToCastSkill;

    protected virtual void Start ()
    {
        _textColor = new Color(0, 186, 255);
        _itemsDatabase = Finder.Instance.itemDatabase;
        caster = Finder.Instance.hero;
    }
    
	protected virtual void Update ()
    {
        timeToCastSkill -= Time.deltaTime;
        if (timeToCastSkill <= 0)
            caster.casting = false;
    }

    protected virtual void Death()
    {
        Destroy(this.gameObject, timeToDestroy);
    }
}
