using UnityEngine;
using System.Collections;

public class StaminaRestore : SkillSelfCast
{
    public float amountToRecoverStamina;
    protected override void Start()
    {
        base.Start();
        caster.stamina += amountToRecoverStamina;
        Destroy(this.gameObject, 0.1f);
        
    }

}
