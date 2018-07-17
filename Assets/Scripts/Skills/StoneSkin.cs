using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneSkin : SkillSelfCast
{
    public float defenceIncrease;
    float _actualDefence;
    Item _item;
    public ParticleSystem particle;
    public float particlePosY;

    protected override void Start ()
    {
        base.Start();
        _item = _itemsDatabase.FetchItemByID(25);
        _actualDefence = caster.defence;
        caster.defence += defenceIncrease;
        particle = Instantiate(particle);
        particle.transform.position = new Vector3(caster.transform.position.x, particlePosY, caster.transform.position.z);
        particle.transform.SetParent(transform);
        StartCoroutine(Destroy());
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(timeToDestroy);
        caster.defence = _actualDefence;
        Destroy(this.gameObject);
    }

    protected override void Update ()
    {
        base.Update();
        particle.transform.position = new Vector3(caster.transform.position.x, particlePosY, caster.transform.position.z);
    }
}
