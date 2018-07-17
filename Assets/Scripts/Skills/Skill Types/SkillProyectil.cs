using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillProyectil : Skill
{
    public int speed;
    public GameObject explosion;
    public GameObject child;
    protected bool _skillInitialized;
    public float projectorFillSpeed;

    protected override void Start ()
    {
        base.Start();        
    }

    protected void InitializeProjector()
    {
        child.SetActive(false);
        GetComponent<BoxCollider>().enabled = false;
        caster.projectorSkill.enabled = true;
        caster.projectorFillProyectil.SetActive(true);
        caster.projectorFillProyectilDistance.SetActive(true);
    }

    protected void DisableProjector()
    {
        child.SetActive(true);
        GetComponent<BoxCollider>().enabled = true;
        caster.projectorFillProyectilDistance.GetComponent<Image>().fillAmount = 0;
        caster.projectorSkill.enabled = false;
        caster.projectorFillProyectil.SetActive(false);
        caster.projectorFillProyectilDistance.SetActive(false);        
    }

    protected virtual IEnumerator Cast()
    {
        yield return null;
    }

    protected override void Update ()
    {
        base.Update();

        if (_skillInitialized)
            return;

        //var finalScale = new Vector3(7.2f, 7.2f, 7.2f);
        //caster.projectorFillProyectil.transform.localScale = Vector3.Lerp(caster.projectorFillProyectil.transform.localScale, finalScale, 11 * Time.deltaTime);
        var dir = caster.GetMousePos() - caster.transform.position;
        caster.transform.forward = new Vector3(dir.x,0,dir.z);
        caster.projectorFillProyectilDistance.GetComponent<Image>().fillAmount = Mathf.Lerp(caster.projectorFillProyectilDistance.GetComponent<Image>().fillAmount, 1, projectorFillSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == (int)LayersEnum.ENEMY)
        {
            EventsManager.TriggerEvent(EventsType.spawnText, new object[] { c.GetComponent<EntityFSM>().canvasHP.GetComponentInParent<RectTransform>(), _damage, _textColor });
            c.GetComponent<EntityFSM>().health -= _damage;
            var particle = Instantiate(explosion);
            particle.transform.position = this.transform.position;
            Destroy(this.gameObject);
        }
        if (c.gameObject.layer == (int)LayersEnum.BOSS)
        {
            EventsManager.TriggerEvent(EventsType.spawnText, new object[] { c.GetComponent<MinotaurBoss>().canvasHP.GetComponentInParent<RectTransform>(), _damage, _textColor });
            c.GetComponent<MinotaurBoss>().health -= _damage;
            var particle = Instantiate(explosion);
            particle.transform.position = this.transform.position;
            Destroy(this.gameObject);
        }
        if (c.gameObject.layer == (int)LayersEnum.FLOOR || c.gameObject.layer == (int)LayersEnum.WALL)
        {
            var particle = Instantiate(explosion);
            particle.transform.position = this.transform.position;
            Destroy(this.gameObject);
        }
    }
}
