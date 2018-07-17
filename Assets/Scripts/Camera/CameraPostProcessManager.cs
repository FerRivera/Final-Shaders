using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPostProcessManager : MonoBehaviour
{
    public CameraPostProceso cpp;
    public StunPostProceso spp;
    public HeroModel hero;
    public float healthPercent;
    public float healthToActivateShader;
    public float stunEffectSpeed;
    
    //public float currentGreyscale;
    //float _greyScaleTotal;

    void Start ()
    {
        cpp = GetComponent<CameraPostProceso>();
        cpp.enabled = false;
        hero = Finder.Instance.hero;
        spp.material.SetFloat("_MaskRange", 1);
        EventsManager.SubscribeToEvent(EventsType.minotaurStunn, heroStunnedEffectCoRoutineStarter);
    }
	
	void Update ()
    {
        healthToActivateShader = hero.maxHealth * healthPercent / 100;
        //healthToActivateShader2 = hero.maxHealth * healthPercent2 / 100;
        //healthToActivateShader3 = hero.maxHealth * healthPercent3 / 100;

        if (hero.health > healthToActivateShader)
            cpp.enabled = false;
        //else if (hero.health <= healthToActivateShader1 && hero.health >= healthToActivateShader2)
        //{
        //    cpp.enabled = true;
        //    cpp.material.SetFloat("_MaskRange", 1);
        //    cpp.material.SetFloat("_GreyRange", 0.4f);
        //}
        //else if (hero.health <= healthToActivateShader2 && hero.health >= healthToActivateShader3)
        //{
        //    cpp.enabled = true;
        //    cpp.material.SetFloat("_MaskRange", 0.5f);
        //    cpp.material.SetFloat("_GreyRange", 0.7f);
        //}

        if (hero.health < healthToActivateShader)
        {
            //int value = 50;
            //int newValue = Mathf.Lerp(-10, 10, Mathf.InverseLerp(0, 100, value));
            cpp.enabled = true;
            cpp.material.SetFloat("_MaskRange", hero.health);
            cpp.material.SetFloat("_GreyRange", 0.4f);
            cpp.material.SetFloat("_FinalGreyscale", Mathf.Lerp(1, 0, Mathf.InverseLerp(0, healthToActivateShader, hero.health)));
        }
        //else
        //{
        //    cpp.enabled = true;
        //    cpp.material.SetFloat("_MaskRange", 0);
        //    cpp.material.SetFloat("_GreyRange", 1);
        //}
            
    }

    public void heroStunnedEffectCoRoutineStarter(params object[] parameters)
    {
        StartCoroutine(heroStunnedEffect());
    }

    public IEnumerator heroStunnedEffect()
    {
        yield return new WaitForEndOfFrame();

        float currentValue = spp.material.GetFloat("_MaskRange");

        if (currentValue >= 1)
            spp.material.SetFloat("_MaskRange", 0);

        currentValue = spp.material.GetFloat("_MaskRange");

        spp.material.SetFloat("_MaskRange", currentValue = Mathf.MoveTowards(currentValue, 1, stunEffectSpeed));

        if (currentValue >= 1)
            yield break;

        StartCoroutine(heroStunnedEffect());
    }
}
