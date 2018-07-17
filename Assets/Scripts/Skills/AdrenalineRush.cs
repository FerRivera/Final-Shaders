using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdrenalineRush : SkillSelfCast
{
    private Item _item;
    public float staminaRegenerationToIncrease;
    float _staminaRegenActual;
    public ParticleSystem particle;
    public float particlePosY;

    protected override void Start ()
    {
        base.Start();
        _item = _itemsDatabase.FetchItemByID(26);
        _staminaRegenActual = caster.GetComponent<HeroStaminaRegen>().staminaRegen;
        caster.GetComponent<HeroStaminaRegen>().staminaRegen += staminaRegenerationToIncrease;

        particle = Instantiate(particle);
        particle.transform.position = new Vector3(caster.transform.position.x, particlePosY, caster.transform.position.z);
        particle.transform.SetParent(transform);
        StartCoroutine(Destroy());
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(timeToDestroy);
        caster.GetComponent<HeroStaminaRegen>().staminaRegen = _staminaRegenActual;
        Destroy(this.gameObject);
    }

    protected override void Update ()
    {
        base.Update();
        particle.transform.position = new Vector3(caster.transform.position.x, particlePosY, caster.transform.position.z);
    }
}
