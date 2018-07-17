using UnityEngine;
using System.Collections;

public class ShieldBlock : MonoBehaviour {

	

    void OnTriggerEnter(Collider c)
    {
        if(c.gameObject.layer == (int)LayersEnum.ENEMYWEAPON)
        {
            if(c.gameObject.GetComponentInParent<EntityFSM>() != null)
            {             
                c.GetComponentInParent<EntityFSM>().blockedAttack = true;
            }

            if(c.GetComponentInParent<MinotaurFSM>() != null)
            {
                c.GetComponentInParent<MinotaurFSM>().blockedAttack = true;
            }

            if (c.GetComponentInParent<SkeletonFSM>() != null)
            {            
                c.GetComponentInParent<SkeletonFSM>().blockedAttack = true;
            }
           
        }
       
    }

}
