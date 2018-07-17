using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Buffsword : SkillSelfCast
{
    private BoxCollider _col;
    private Item _item;
    public GameObject particleOnFire;
    public Image underBarInstance;
    private Image _uiInstance;
    private GameObject enemy;

    protected override void Start()
    {
        base.Start();
        _col = GetComponent<BoxCollider>();
        //_col = caster.swordCollider.GetComponent<BoxCollider>();
        _item = _itemsDatabase.FetchItemByID(0);
        caster.sword.transform.FindChild("fire").gameObject.SetActive(true); //activo las particulas que ya estan en la espada del hero
        transform.SetParent(caster.sword.transform);
        _col.enabled = false;
        var particles = transform.GetComponentsInChildren<ParticleSystem>(); //desactivo las particulas que instancio
        foreach (var item in particles)
        {
            item.gameObject.SetActive(false);
        }
        caster.sword.transform.FindChild("SwordEdge").GetComponent<MeshRenderer>().material.SetFloat("_EmissionPower",1);
        caster.sword.transform.FindChild("SwordEdge").GetComponent<MeshRenderer>().material.SetFloat("_ASEOutlineWidth", 0.001f);
        transform.position = caster.sword.transform.position + caster.sword.transform.forward * 0.2f;
       //_uiInstance = Instantiate(underBarInstance);
        //Utility.instance.SetUIPosition(_uiInstance.gameObject);

        transform.Rotate(330, 60, 300);
        StartCoroutine(Destroy());
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(Collider());
        }
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(timeToDestroy);
        caster.sword.transform.FindChild("SwordEdge").GetComponent<MeshRenderer>().material.SetFloat("_EmissionPower", 0);
        caster.sword.transform.FindChild("SwordEdge").GetComponent<MeshRenderer>().material.SetFloat("_ASEOutlineWidth", 0);
       // Utility.instance.RemoveUIPosition(_uiInstance.gameObject);
        caster.sword.transform.FindChild("fire").gameObject.SetActive(false);
        Destroy(this.gameObject);
    }

    IEnumerator Collider()
    {
        _col.enabled = true;
        yield return new WaitForSeconds(0.2f);
        _col.enabled = false;
    }

    void OnTriggerEnter(Collider c)
    {     
        if (c.gameObject.layer == (int)LayersEnum.ENEMY || c.gameObject.layer == (int)LayersEnum.BOSS)
        {
            //_col.enabled = false;
            enemy = c.GetComponent<EnemyCharacter>().gameObject;
            if(enemy.GetComponent<damageOverTime>() == null)
            {
                enemy.gameObject.AddComponent<damageOverTime>();
                var dmgOverTime = enemy.gameObject.GetComponent<damageOverTime>();
                dmgOverTime.StartdamageOverTime(enemy.GetComponent<EnemyCharacter>(), _item.HitDamage,_textColor);
                var part = Instantiate(particleOnFire);
                part.gameObject.transform.SetParent(enemy.transform);
                part.transform.position = enemy.transform.position;
                dmgOverTime.Destroy(timeToDestroy);
            }

        }

        //if (c.gameObject.layer == (int)LayersEnum.BOSS)
        //{
        //    //_col.enabled = false;
        //    enemy = c.GetComponent<MinotaurBoss>().gameObject;
        //    if (enemy.GetComponent<damageOverTime>() == null)
        //    {
        //        enemy.gameObject.AddComponent<damageOverTime>();
        //        var dmgOverTime = enemy.gameObject.GetComponent<damageOverTime>();
        //        dmgOverTime.StartdamageOverTimeBoss(enemy.GetComponent<MinotaurBoss>(), _item.HitDamage);
        //        var part = Instantiate(particleOnFire);
        //        part.gameObject.transform.SetParent(enemy.transform);
        //        part.transform.position = enemy.transform.position;
        //    }
        //}
    }

}
