using UnityEngine;
using System.Collections;
using FSM;
using System.Collections.Generic;
using System;

public class ExplosiveSkeletonFSM : EntityFSM
{

    public GameObject projectorAOEFill;
    public GameObject barrel;
    public GameObject explotion;
    GameObject _explotionSphere;
    public Projector dmgProjector;

    public float expRange;
    public float timeToExplodeTotal;
    float _timeToExplodeActual;

    public LayerMask damageable;
    private List<Collider> _colls;

    private bool _getColliders;
    private bool _activated;

    public ParticleSystem explosionParticle;
    float _explotionCurrentLerpTimeSphere;
    public float explotionLerpTimeSphere;
    public float explotionMinValueTimeSphere;

    protected override void Start()
    {
        base.Start();

        var idle = new State<EnemiesAction>("idle");
        var beforeSpawn = new State<EnemiesAction>("beforeSpawn");
        var spawn = new State<EnemiesAction>("spawn");
        var chase = new State<EnemiesAction>("chasing");
        var attack = new State<EnemiesAction>("attack");
        var death = new State<EnemiesAction>("death");

        StateConfigurer.New(beforeSpawn)
        .SetTransition(EnemiesAction.Spawn, spawn)
        .Done();

        StateConfigurer.New(spawn)
        .SetTransition(EnemiesAction.Chase, chase)
        .SetTransition(EnemiesAction.Death, death)
        .Done();

        StateConfigurer.New(idle)
        .SetTransition(EnemiesAction.Chase, chase)
        .SetTransition(EnemiesAction.Spawn, spawn)
        .SetTransition(EnemiesAction.Attack, attack)
        .SetTransition(EnemiesAction.Death, death)
        .Done();

        StateConfigurer.New(chase)
        .SetTransition(EnemiesAction.Attack, attack)
        .SetTransition(EnemiesAction.Death, death)
        .Done();

        StateConfigurer.New(attack)
        .SetTransition(EnemiesAction.Chase, chase)
        .SetTransition(EnemiesAction.Death, death)
        .Done();

        StateConfigurer.New(death)
        .SetTransition(EnemiesAction.Death, death)
        .SetTransition(EnemiesAction.Null, null)
        .Done();

        spawn.OnEnter += action => SpawnEnter();
        chase.OnEnter += action => ChaseEnter();
        beforeSpawn.OnEnter += action => beforeSpawnEnter();
        attack.OnEnter += action => AttackEnter();
        death.OnEnter += action => DeathEnter();

        spawn.OnUpdate += SpawnUpdate;
        chase.OnUpdate += ChaseUpdate;
        beforeSpawn.OnUpdate += beforeSpawnUpdate;
        attack.OnUpdate += AttackUpdate;

        //attack.OnExit += action => AttackExit();
        chase.OnExit += action => ChaseExit();

        _fsm = new EventFSM<EnemiesAction>(beforeSpawn);

        _fsm.Feed(EnemiesAction.BeforeSpawn);

        _explotionCurrentLerpTimeSphere = explotionLerpTimeSphere;
    }

    
    #region Enter
    private void DeathEnter()
    {
        //dead = true;
        explotion.SetActive(false);
        speed = 0;
        _animator.SetTrigger("dead");

        gameObject.layer = (int)LayersEnum.IGNOREPLAYER;

        if(barrel != null)
        {
            Destroy(barrel);
        }
        
        //Destroy(barrel);
        if (weapon != null)
        {
            if (weapon.GetComponent<Rigidbody>() == null)
            {
                weapon.AddComponent<Rigidbody>();
            }
            weapon.transform.parent = null;
        }
        Death();
        Destroy(parent.gameObject, timeToDisappear);
    }    

    private void AttackEnter()
    {
        _explotionSphere = Instantiate(explotion);
        _explotionSphere.SetActive(true);
        Vector3 temp = transform.position;
        _explotionSphere.transform.position = new Vector3(temp.x, temp.y + 1,temp.z);
        //_explotionSphere.transform.Rotate(-525.532f, -576.159f, -240.409f); //rotation = new Quaternion(-500f, -900f, -243.192f, Quaternion.identity.w);        
        _explotionSphere.transform.rotation = Quaternion.Euler(0,0,0);//Quaternion.Euler(-525.532f, -576.159f, -240.409f);
        _explotionSphere.transform.SetParent(transform);
        _animator.SetTrigger("explode");
        speed = 0;
        Timer();
    }

    protected void ExplotionSphere()
    {
        _explotionCurrentLerpTimeSphere -= Time.deltaTime;

        if (_explotionCurrentLerpTimeSphere < explotionMinValueTimeSphere)
        {
            _explotionCurrentLerpTimeSphere = explotionMinValueTimeSphere;
        }

        float perc = _explotionCurrentLerpTimeSphere / explotionLerpTimeSphere;
        float lerp = 0;

        if (_explotionCurrentLerpTimeSphere < 0.6)
            perc = perc * 0.1f;

        lerp = Mathf.Lerp(0f, _explotionCurrentLerpTimeSphere, perc);

        _explotionSphere.GetComponent<Renderer>().material.SetFloat("_Dissolve", lerp);

    }

    public void beforeSpawnEnter()
    {
        barrel.GetComponent<MeshRenderer>().enabled = false;
        _colls = new List<Collider>();
        dmgProjector.enabled = false;
        DeactivateWeaponBoxCollider();
        BeforeSpawned();

        if (trigger != null)
        {
            trigger.chaseCharacter += () =>
            {
                if (this != null && _fsm != null)
                {
                    _fsm.Feed(EnemiesAction.Spawn);
                }
            };
        }

        if (triggerComplex != null)
        {
            triggerComplex.chaseCharacter += () =>
            {
                if (this != null && _fsm != null)
                {
                    _fsm.Feed(EnemiesAction.Spawn);
                }
            };
        }
    }

    private void ChaseEnter()
    {
        speed = maxSpeed;
        _animator.SetTrigger("walk");
        //_animator.SetBool("walk",true);
    }

    public void SpawnEnter()
    {
        Spawn();
        barrel.GetComponent<MeshRenderer>().enabled = true;
    }
    #endregion

    #region Update

    public void beforeSpawnUpdate()
    {
        trigger.chaseCharacter += () =>
        {
            if (this != null)
            {
                _fsm.Feed(EnemiesAction.Spawn);
            }
        };
    }

    public void SpawnUpdate()
    {
        Spawn();

        if (_spawned)
            _fsm.Feed(EnemiesAction.Chase);
    }

    public void ChaseUpdate()
    {

        Vector3 _moveDir = _target.transform.position - transform.position;
        _moveDir = _moveDir.normalized;
        _moveDir.y = 0;
        transform.forward = Vector3.Lerp(transform.forward, _moveDir, rotationSpeed * Time.deltaTime);
        _rb.velocity = _moveDir * speed;

        if ((int)Vector3.Distance(transform.position, _target.transform.position) <= 1)
        {
            _fsm.Feed(EnemiesAction.Attack);
        }
    }

    public void AttackUpdate()
    {
        ExplotionSphere();
    }

    #endregion

    #region Exit
    private void ChaseExit()
    {
       // _animator.SetBool("walk", true);
    }
    #endregion
    protected override void Update()
    {
        base.Update();

        if(health <= 0)
            _fsm.Feed(EnemiesAction.Death);

        if (_activated)
        {
            if (_timeToExplodeActual >= timeToExplodeTotal)
            {
                _explotionSphere.SetActive(false);
                DoAttack();
            }
            else
            {
                _timeToExplodeActual += Time.deltaTime;
                var finalScale = new Vector3(13.5f, 13.5f, 13.5f);
                projectorAOEFill.transform.localScale = Vector3.Lerp(projectorAOEFill.transform.localScale, finalScale, 1.2f * Time.deltaTime);
            }
        }
    }

    public void DoAttack()
    {
        if (!_getColliders)
        {            
            Destroy(barrel);
            dmgProjector.enabled = false;
            _getColliders = true;
            projectorAOEFill.SetActive(false);
            _colls.AddRange(Physics.OverlapSphere(transform.position, expRange, damageable));
            var e = Instantiate(explosionParticle);
            e.transform.position = transform.position;
            foreach (var item in _colls)
            {
                if (item.Equals(_target.GetComponent<Collider>()))
                {
                    _target.GetComponent<HeroModel>().health -= damage;
                }
            }
            health = 0;
            _fsm.Feed(EnemiesAction.Death);
            Death();
        }
    }

    public void Timer()
    {
        projectorAOEFill.SetActive(true);
        projectorAOEFill.transform.localScale = new Vector3(0, 0, 0);
        dmgProjector.enabled = true;
        _activated = true;
    }
}
