using UnityEngine;
using System.Collections;

public class IceSkill : SkillProyectil
{
    public float posY;
    public float zOffset;

    protected override void Start ()
    {
        base.Start();
        InitializeProjector();
        StartCoroutine(Cast());
    }   

    protected override IEnumerator Cast()
    {
        yield return new WaitForSeconds(timeToCastSkill);
        DisableProjector();
        _skillInitialized = true;
        base.Cast();
        Init();
    }

    void Init()
    {
        transform.position = new Vector3(caster.transform.position.x, caster.transform.position.y + posY, caster.transform.position.z + zOffset);
        transform.rotation = caster.transform.rotation;
        var dir = (caster.GetMousePos() - caster.transform.position).normalized;
        transform.forward = new Vector3(dir.x, 0, dir.z);
        Item item = _itemsDatabase.FetchItemByID(2);
        _damage = item.HitDamage;
        Death();
    }

    protected override void Update ()
    {
        base.Update();
        if(_skillInitialized)
            transform.position += transform.forward * speed * Time.deltaTime;
	}
}
