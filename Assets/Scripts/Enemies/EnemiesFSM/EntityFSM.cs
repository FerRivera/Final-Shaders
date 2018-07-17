using UnityEngine;
using System.Collections;
using FSM;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public enum EnemyTypeFSM
{
    MINOTAUR_FSM,
    SKELETON_FSM,
    MINOTAUR_RANGE_FSM,
    EXPLOSIVE_SKELETON_FSM,
    ARCHER_SKELETON_FSM,
    INMOBILE_ARCHER_SKELETON_FSM,
}

public enum EnemiesAction
{
    Null,
    Idle,
    Spawn,
    BeforeSpawn,
    Chase,
    Attack,
    SkillAOE,
    Decide,
    Death,
    GetHit,
    Scape
}

public class EntityFSM : EnemyCharacter
{
    List<SoundsEnum> _hitsSounds = new List<SoundsEnum>();

    //protected List<EntityFSM> _nearEnemies = new List<EntityFSM>();

    protected EventFSM<EnemiesAction> _fsm;

    //[HideInInspector]
    //public float speed;
    [HideInInspector]
    public float rotationSpeed;
    //[HideInInspector]
    //public float health;
    [HideInInspector]
    public float maxHealth;
    [HideInInspector]
    public float distanceToAttackTarget;
    public float timeToDisappear;
    [HideInInspector]
    public float damage;
    protected float _velY;
    protected float _timeToRefreshAttackActual;
    [HideInInspector]
    public float maxSpeed;
    protected float _attackAnimDuration;
    protected float _currentTimeToAttackAgain;
    protected float _dissolveShaderCurrentLerpTime;
    protected float _dissolveShaderLerpTime;
    protected float _dissolveShaderDelayTotal;
    protected float _dissolveShaderDelayCurrent;

    public event Action openDoors = delegate { };

    protected HeroModel _target;
    public GameObject weapon;
    //protected GameObject _objectShader;
    public GameObject shadow;
    public GameObject particlePosition;
    public GameObject particleHit;
    //public GameObject canvasHP;
    public GameObject miniMap;
    public GameObject parent;

    protected Collider _coll;

    protected Rigidbody _rb;

    //protected CapsuleCollider _capsuleCollider;

    protected Animator _animator;

    protected bool _onMouseOverActivated;
    protected bool _spawned;
    protected bool _particleSpawned;
    [HideInInspector]public bool blockedAttack;
    //[HideInInspector]
    //public bool dead;

    public ParticleSystem spawnerParticle;

    public EnemyTypeFSM enemyTypeEnum;

    [HideInInspector]public Trigger trigger;
    [HideInInspector]public TriggerComplex triggerComplex;

    public Image healthBar;

    protected Color _textColor;

    protected GameObject _objectShader;

    protected Material _dissolveMaterial;

    public EnemyData data;

    protected virtual void Start ()
    {
        _hitsSounds.Add(SoundsEnum.HERO_ATTACK_HIT_1);
        _hitsSounds.Add(SoundsEnum.HERO_ATTACK_HIT_2);
        _hitsSounds.Add(SoundsEnum.HERO_ATTACK_HIT_3);
        speed = data.speed;
        rotationSpeed = data.rotationSpeed;        
        maxHealth = data.maxHealth;
        health = maxHealth;
        distanceToAttackTarget = data.distanceToAttackTarget;
        damage = data.damage;
        _textColor = Color.white;
        _target = Finder.Instance.hero;
        _coll = GetComponent<CapsuleCollider>();
        _objectShader = transform.FindChild("ObjectShader").gameObject;
        _animator = GetComponent<Animator>();
        miniMap.SetActive(true);
        _rb = GetComponent<Rigidbody>();
        _dissolveShaderLerpTime = 4;
        _dissolveShaderDelayTotal = 1;
        _dissolveMaterial = _objectShader.GetComponent<SkinnedMeshRenderer>().materials[1];
    }

    public void DeactivateWeaponBoxCollider()
    {
        if (weapon != null)
        {
            weapon.SetActive(false);//GetComponent<BoxCollider>().enabled = false;
        }
    }

    public virtual void RemoveBothColliders()
    {
        if (weapon != null)
        {
            weapon.GetComponent<BoxCollider>().isTrigger = false;
            weapon.layer = (int)LayersEnum.IGNOREPLAYER;
            Destroy(weapon,timeToDisappear);
        }
        gameObject.layer = (int)LayersEnum.IGNOREPLAYER;
    }

    void HealthBar()
    {
        if (health < maxHealth && canvasHP != null && !canvasHP.activeSelf)
        {
            canvasHP.SetActive(true);
        }
        else if (healthBar != null)
        {
            health = Mathf.Clamp(health, 0, maxHealth);
            var speed = 10f;
            healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, health / maxHealth, Time.deltaTime * speed);
        }
    }

    protected void Death()
    {
        if (health <= 0 && !dead)
        {
            Destroy(canvasHP);
            Destroy(shadow);
            dead = true;
            openDoors();
            health = 0;
            miniMap.SetActive(false);
            var temp = transform.FindChild("ParticleOnFire 1(Clone)");
            if (temp != null)
            {
                Destroy(temp.gameObject);
            }
        }
    }

    public void BeforeSpawned()
    {
        GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;

        if (weapon != null && weapon.GetComponent<MeshRenderer>() != null)
        {
            weapon.GetComponent<MeshRenderer>().enabled = false;
        }

        gameObject.layer = (int)LayersEnum.DEFAULT;
        _onMouseOverActivated = false;
        _animator.enabled = false;
        maxSpeed = speed;
        
    }

    public void Spawn()
    {
        StartCoroutine(EndSpawn());
        _animator.enabled = true;
        GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
        shadow.SetActive(true);

        if (weapon != null && weapon.GetComponent<MeshRenderer>() != null)
        {
            weapon.GetComponent<MeshRenderer>().enabled = true;
        }

        gameObject.layer = (int)LayersEnum.ENEMY;
        //StartCoroutine(CheckNearEnemies());
        _onMouseOverActivated = true;

        if (!_particleSpawned)
        {
            var p = Instantiate(spawnerParticle);
            spawnerParticle.Play();
            p.transform.SetParent(transform.parent,false);
            p.transform.position = particlePosition.transform.position;
            _particleSpawned = true;
        }
    }

    protected virtual void Update ()
    {
        _rb.angularVelocity = Vector3.zero;        

        CheckTargetIsAlive();        

        if (health <= 0)
        {
            DissolveAfterDeath();
        }
    }

    protected void DissolveAfterDeath()
    {
        _dissolveShaderDelayCurrent += Time.deltaTime;

        if(_dissolveShaderDelayCurrent >= _dissolveShaderDelayTotal)
        {
            _dissolveShaderCurrentLerpTime += Time.deltaTime;
            if (_dissolveShaderCurrentLerpTime > _dissolveShaderLerpTime)
            {
                _dissolveShaderCurrentLerpTime = _dissolveShaderLerpTime;
            }

            float perc = _dissolveShaderCurrentLerpTime / _dissolveShaderLerpTime;

            _objectShader.GetComponent<SkinnedMeshRenderer>().material = _dissolveMaterial;

            var lerp = Mathf.Lerp(-0.1f, 0.6f, perc);

            _dissolveMaterial.SetFloat("_Dissolve", lerp);
        }       
    }

    protected void OnMouseOver()
    {
        if (_onMouseOverActivated)
        {
            CursorPointer.instance.ChangeToAttack();
        }
    }

    protected void OnMouseExit()
    {
        CursorPointer.instance.ChangeToDefault();
    }

    protected virtual void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == (int)LayersEnum.SWORD)
        {
            AudioManager.instance.PlaySound(_hitsSounds[UnityEngine.Random.Range(0, _hitsSounds.Count)]);
            var part = Instantiate(particleHit);
            part.transform.position = c.transform.position;
            float dmg = c.gameObject.GetComponentInParent<HeroModel>().GetDamageValue();

            if(canvasHP != null)
                EventsManager.TriggerEvent(EventsType.spawnText, new object[] { canvasHP.GetComponentInParent<RectTransform>(), dmg, _textColor });

            health -= dmg;
        }

        if (c.gameObject.layer == (int)LayersEnum.ENEMY)
        {
            blockedAttack = true;
            _timeToRefreshAttackActual = 0;
        }
    }

    protected virtual void OnTriggerStay(Collider c)
    {
        if (c.gameObject.layer == (int)LayersEnum.ENEMY)
        {
            blockedAttack = true;
            _timeToRefreshAttackActual = 0;
        }

    }

    public void CheckTargetIsAlive()
    {
        if (_spawned)
        {
            if (!_target.GetComponent<HeroModel>().deadHero)
            {
                if (_fsm != null)
                    _fsm.Update();

                HealthBar();
                Death();
            }            
        }
    }

    protected IEnumerator EndSpawn()
    {
        yield return new WaitForSeconds(1);
        _spawned = true;
    }
}
