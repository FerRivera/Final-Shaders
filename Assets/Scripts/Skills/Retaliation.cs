using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Retaliation : SkillSelfCast
{
    Item _item;
    public float percentDMG;

	protected override void Start ()
    {
        base.Start();
        _item = _itemsDatabase.FetchItemByID(28);
        EventsManager.SubscribeToEvent(EventsType.retaliationSkill, Execute);
        transform.SetParent(caster.transform,false);
   
        transform.localPosition = new Vector3(0,0.85f,0);
        transform.localRotation = Quaternion.Euler(-90,0,0);


        var prnt = caster.GetComponent<Transform>().FindChild("CATRigHub001");

        if(prnt != null)
        {            
            transform.parent = prnt;
        }

        StartCoroutine(Destroy());
    }

    public void Execute(params object[] p)
    {
        var enemy = (EnemyCharacter)p[0];
        var dmg = (float)p[1];

        var damageForEnemy = (dmg * percentDMG) / 100;

        if (enemy.canvasHP != null)
        {
            EventsManager.TriggerEvent(EventsType.spawnText, new object[] { enemy.canvasHP.GetComponentInParent<RectTransform>(), damageForEnemy, _textColor });
            enemy.health -= damageForEnemy;
        } 
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(timeToDestroy);
        EventsManager.UnsubscribeToEvent(EventsType.retaliationSkill, Execute);
        Destroy(this.gameObject);
    }
    
    protected override void Update ()
    {
        base.Update();
	}
}
