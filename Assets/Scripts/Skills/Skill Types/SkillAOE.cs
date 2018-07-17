using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillAOE : Skill
{
    public GameObject child;
    protected bool _skillInitialized;
    public float proyectorPosY;

    protected override void Start ()
    {
        base.Start();        
        InitializeProjector();
        //caster.projectorSkillAOE.gameObject.transform.parent = transform;
    }

    protected virtual void Init()
    {
        transform.position = new Vector3(caster.GetMousePosAoE().x, 0, caster.GetMousePosAoE().z);
    }

    protected virtual IEnumerator Cast()
    {        
        yield return null;
    }

    protected void InitializeProjector()
    {
        child.SetActive(false);
        GetComponent<BoxCollider>().enabled = false;
        caster.projectorSkillAOE.SetActive(true);
        caster.projectorSkillAOEFill.SetActive(true);
    }

    protected void DisableProjector()
    {
        child.SetActive(true);
        //GetComponent<BoxCollider>().enabled = true;
        caster.projectorSkillAOEFill.transform.localScale = new Vector3(0, 0, 0);
        caster.projectorSkillAOE.SetActive(false);
        caster.projectorSkillAOEFill.SetActive(false);
    }

    protected override void Update ()
    {
        base.Update();

        if (_skillInitialized)
            return;
        
        var pos = caster.GetMousePosAoE();
        caster.projectorSkillAOE.transform.position = new Vector3(pos.x, proyectorPosY, pos.z);
        caster.projectorSkillAOEFill.transform.position = new Vector3(pos.x, proyectorPosY, pos.z);
        var dir = caster.GetMousePos() - caster.transform.position;
        caster.transform.forward = new Vector3(dir.x, 0, dir.z);
        var finalScale = new Vector3(10,10, 10);
        caster.projectorSkillAOEFill.transform.localScale = Vector3.Lerp(caster.projectorSkillAOEFill.transform.localScale, finalScale, 10 * Time.deltaTime);

    }

    protected void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == (int)LayersEnum.ENEMY)
        {
            var co = c.GetComponent<EntityFSM>();
            EventsManager.TriggerEvent(EventsType.spawnText, new object[] { co.canvasHP.GetComponentInParent<RectTransform>(), _damage, _textColor });
            co.health -= _damage;
        }

        if (c.gameObject.layer == (int)LayersEnum.BOSS)
        {
            var co = c.GetComponent<MinotaurBoss>();
            EventsManager.TriggerEvent(EventsType.spawnText, new object[] { co.canvasHP.GetComponentInParent<RectTransform>(), _damage, _textColor });
            co.health -= _damage;
        }
    }

    protected IEnumerator DeathCollider()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<Collider>().enabled = false;
    }
}
