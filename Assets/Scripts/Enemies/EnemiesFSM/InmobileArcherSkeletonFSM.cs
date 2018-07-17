using UnityEngine;
using System.Collections;
using FSM;

public class InmobileArcherSkeletonFSM : EntityFSM
{
    public GameObject proyectil;
    bool _rotating;
    public float timeToAttackAgain;
    public GameObject weaponFBX;  

    protected override void Start()
    {
        base.Start();

        var idle = new State<EnemiesAction>("idle");
        var beforeSpawn = new State<EnemiesAction>("beforeSpawn");
        var spawn = new State<EnemiesAction>("spawn");
        //var chase = new State<EnemiesAction>("chasing");
        var attack = new State<EnemiesAction>("attack");
        var death = new State<EnemiesAction>("death");

        StateConfigurer.New(beforeSpawn)
        .SetTransition(EnemiesAction.Spawn, spawn)
        .Done();

        StateConfigurer.New(spawn)
        .SetTransition(EnemiesAction.Attack, attack)
        .SetTransition(EnemiesAction.Death, death)
        .Done();

        StateConfigurer.New(idle)
        .SetTransition(EnemiesAction.Spawn, spawn)
        .SetTransition(EnemiesAction.Attack, attack)
        .SetTransition(EnemiesAction.Death, death)
        .Done();

        StateConfigurer.New(attack)
        .SetTransition(EnemiesAction.Attack, attack)
        .SetTransition(EnemiesAction.Death, death)
        .Done();

        StateConfigurer.New(death)
        .SetTransition(EnemiesAction.Death, death)
        .SetTransition(EnemiesAction.Null, null)
        .Done();

        spawn.OnEnter += action => SpawnEnter();
        beforeSpawn.OnEnter += action => beforeSpawnEnter();
        attack.OnEnter += action => AttackEnter();
        death.OnEnter += action => DeathEnter();

        spawn.OnUpdate += SpawnUpdate;
        beforeSpawn.OnUpdate += beforeSpawnUpdate;
        attack.OnUpdate += AttackUpdate;

        attack.OnExit += action => AttackExit();

        _fsm = new EventFSM<EnemiesAction>(beforeSpawn);

        _fsm.Feed(EnemiesAction.BeforeSpawn);

        if (proyectil != null)
        {
            proyectil.GetComponent<Arrow>().damage = damage;
            proyectil.GetComponent<Arrow>().speed = data.proyectilSpeed;
        }
            
    }    
    #region Enter
    public void SpawnEnter()
    {
        Spawn();
        weaponFBX.GetComponent<MeshRenderer>().enabled = true;
    }

    public void beforeSpawnEnter()
    {
        DeactivateWeaponBoxCollider();
        BeforeSpawned();

        if (weaponFBX != null)
            weaponFBX.GetComponent<MeshRenderer>().enabled = false;

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

    public void AttackEnter()
    {
        _rotating = true;
        _animator.SetTrigger("attack");
        speed = 0;
    }

    public void DeathEnter()
    {
        dead = true;
        speed = 0;
        _animator.SetTrigger("dead");
        RemoveBothColliders();

        if (weapon != null)
        {
            if (weapon.GetComponent<Rigidbody>() == null)
            {
                weapon.AddComponent<Rigidbody>();
            }
            weapon.transform.parent = null;
        }
        _fsm = null;
        Destroy(parent.gameObject, timeToDisappear);
    }
    #endregion

    #region Update
    public void SpawnUpdate()
    {
        Spawn();

        if (_spawned)
            _fsm.Feed(EnemiesAction.Attack);
    }

    public void beforeSpawnUpdate()
    {
        trigger.chaseCharacter += () =>
        {
            if (this != null)
            {
                _fsm.Feed(EnemiesAction.Spawn);
            }
            //_activated = true;
        };

        if (triggerComplex != null)
        {
            triggerComplex.chaseCharacter += () =>
            {
                if (this != null)
                {
                    _fsm.Feed(EnemiesAction.Spawn);
                }
            };
        }
    }

    public void AttackUpdate()
    {
        Vector3 targetDir = _target.transform.position - transform.position;
        targetDir.y = transform.forward.y;

        if (_rotating)
        {
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, rotationSpeed * Time.deltaTime, 0.0F);

            if (transform.forward == newDir)
            {
                _rotating = false;
                StartCoroutine(Fire(new Vector3(newDir.x, 0, newDir.z), transform.position , 1.2f));
            }
            else
            {
                transform.rotation = Quaternion.LookRotation(newDir);
            }
        }
    }
    #endregion

    #region Exit
    public void AttackExit()
    {
        speed = maxSpeed;
    }
    #endregion

    protected override void Update()
    {
        base.Update();

        if (health <= 0 && _fsm != null)
            _fsm.Feed(EnemiesAction.Death);

        if (_target.health <= 0)
        {
            StopAllCoroutines();
            _animator.Play("Idle");
            speed = 0;
            return;
        }
    }

    IEnumerator Fire(Vector3 dir, Vector3 pos, float timer)
    {
        yield return new WaitForSeconds(timer);
        if (dead)
            yield break;
        var p = Instantiate(proyectil,transform.position,Quaternion.identity);    
        p.transform.forward = dir;
        p.transform.position = new Vector3(transform.position.x,transform.position.y + 1,transform.position.z);
        p.GetComponent<Proyectiles>().owner = this;
        yield return new WaitForSeconds(timeToAttackAgain);

        if(!dead)
            _fsm.Feed(EnemiesAction.Attack);
    }
}
