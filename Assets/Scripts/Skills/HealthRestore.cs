using UnityEngine;
using System.Collections;

public class HealthRestore : SkillSelfCast
{
    public float amountToHeal;
    protected override void Start()
    {
        base.Start();

        if(caster.healing)
        {
            caster.saveHp += amountToHeal;
        }
        else
        {
            caster.health += amountToHeal;
        }
        Destroy(this.gameObject, 0.1f);
    }

}
