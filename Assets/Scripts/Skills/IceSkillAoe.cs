using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IceSkillAoe : SkillAOE
{
    public float posY;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(Cast());
    }

    protected override void Init()
    {
        Item item = _itemsDatabase.FetchItemByID(4);
        _damage = item.HitDamage;
        //base.Init();
        transform.position = new Vector3(caster.GetMousePosAoE().x, posY, caster.GetMousePosAoE().z);
        GetComponent<BoxCollider>().enabled = true;
        StartCoroutine(DeathCollider());
        Death();
    }

    protected override IEnumerator Cast()
    {
        yield return new WaitForSeconds(timeToCastSkill);
        DisableProjector();
        _skillInitialized = true;
        base.Cast();        
        Init();
    }

    protected override void Update()
    {
        base.Update();        
    }

    
}
