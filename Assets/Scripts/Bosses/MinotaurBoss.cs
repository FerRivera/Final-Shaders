using UnityEngine;
using System.Collections;
using FSM;
using UnityEngine.UI;

public enum MinotaurBossAction
{
    Null,
    Idle,
    Chase,
    Attack,
    Recover,
    SkillAOE,
    Charge,
    ChargeChanneling,
    Death
}
//IA2-P3 / FSM BOSS
public class MinotaurBoss : EnemyCharacter
{
    EventFSM<MinotaurBossAction> _fsm;
    public HeroModel target;
    //public float speed;
    public float rotationSpeed;
    public float distanceToChase;
    public float timeToRecover;
    public float distanceToCharge;
    public float timeToCharge;
    public float chargeSpeed;
    public float raycastDistance;
    public float ChargeMaxDistance;
    //[HideInInspector]
    //public float health;
    public float maxHealth;
    int _hitsCount;
    public int hitsMinCount;
    public int hitsMaxCount;
    int _randomHitsCount;
    public float damageAOESkill;
    public float timeToDoAOESkill;
    public float stunTime;
    public float damage;
    public float timeToActivateWeapon;
    public float chargeDamage;
    float _dissolveShaderCurrentLerpTime;
    float _dissolveShaderLerpTime;
    float _dissolveShaderDelayTotal;
    float _dissolveShaderDelayCurrent;
    public LayerMask collisionable;
    public GameObject particleCharge;
    public GameObject instancedChargeParticle;
    public GameObject particleGround;
    public Projector chargeProjector;
    public Projector AOEDamageProjector;
    public Projector AOEStunProjector;
    public GameObject projectorAOEFillStun;
    public GameObject projectorAOEFillDamage;
    public GameObject minotaurChargeCollider;
    Vector3 _savePos;
    public GameObject weapon;
    [HideInInspector]public bool charging;
    //bool dead;
    public Image healthBar;
    //public GameObject canvasHP;

    protected GameObject _objectShader;
    protected Material _dissolveMaterial;

    public BoxCollider colliderDistance;

    public Color textColor;
    Animator _animator;

    public GameObject portal;

    void Start ()
    {
        _animator = GetComponent<Animator>();
        health = maxHealth;
        textColor = Color.white;
        _objectShader = transform.FindChild("ObjectShader").gameObject;
        _dissolveShaderLerpTime = 10;
        _dissolveShaderDelayTotal = 3;
        _dissolveMaterial = _objectShader.GetComponent<SkinnedMeshRenderer>().materials[1];
        _randomHitsCount = Random.Range(hitsMinCount,hitsMaxCount+1);

        var idle = new State<MinotaurBossAction>("idle");
        var chase = new State<MinotaurBossAction>("chasing");
        var attack= new State<MinotaurBossAction>("attacking");
        var recover = new State<MinotaurBossAction>("recovering");
        var skillAOE = new State<MinotaurBossAction>("skillAOE");
        var charge = new State<MinotaurBossAction>("charging");
        var chargeChanneling = new State<MinotaurBossAction>("chargeChanneling");
        var death = new State<MinotaurBossAction>("death");

        StateConfigurer.New(idle)
            .SetTransition(MinotaurBossAction.Charge, charge)
            .SetTransition(MinotaurBossAction.Chase, chase)
            .SetTransition(MinotaurBossAction.Attack, attack)
            .SetTransition(MinotaurBossAction.ChargeChanneling, chargeChanneling)
            .SetTransition(MinotaurBossAction.SkillAOE, skillAOE)
            .SetTransition(MinotaurBossAction.Death, death)
            .Done()
            ;
        StateConfigurer.New(death)
            .SetTransition(MinotaurBossAction.Death, death)
            .SetTransition(MinotaurBossAction.Null, null)
            .Done()
            ;
        StateConfigurer.New(attack)
            .SetTransition(MinotaurBossAction.Recover, recover)
            .SetTransition(MinotaurBossAction.Death, death)
            .Done()
            ;

        StateConfigurer.New(recover)
            .SetTransition(MinotaurBossAction.Idle, idle)
            .SetTransition(MinotaurBossAction.Death, death)
            .Done()
            ;

        StateConfigurer.New(skillAOE)
            .SetTransition(MinotaurBossAction.Recover, recover)
            .SetTransition(MinotaurBossAction.Death, death)
            .Done()
            ;

        StateConfigurer.New(charge)
            .SetTransition(MinotaurBossAction.Recover, recover)
            .SetTransition(MinotaurBossAction.Death, death)
            .Done()
            ;

        StateConfigurer.New(chase)
            .SetTransition(MinotaurBossAction.Idle, idle)
            .SetTransition(MinotaurBossAction.SkillAOE, skillAOE)
            .SetTransition(MinotaurBossAction.Attack, attack)
            .SetTransition(MinotaurBossAction.ChargeChanneling, chargeChanneling)
            .SetTransition(MinotaurBossAction.Death, death)
            .Done()
            ;

        StateConfigurer.New(chargeChanneling)
            .SetTransition(MinotaurBossAction.Charge, charge)
            .SetTransition(MinotaurBossAction.Death, death)
            .Done()
            ;

        #region OnEnter
        chase.OnEnter += action => _animator.Play("Walk");
        attack.OnEnter += action => 
        {
            var r = Random.Range(0, 2);
      
            if (r == 1)
            {
                //_animator.Play("AttackLeftHand");
                //if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("AttackLeftHand"))
                    _animator.SetTrigger("AttackLeft");
            }
            else
            {
               // _animator.Play("AttackRightHand");
               //if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("AttackRightHand"))
                    _animator.SetTrigger("AttackRight");
            }
            _fsm.Feed(MinotaurBossAction.Recover);
            weapon.SetActive(false);
            StartCoroutine(DeactivateWeapon(timeToActivateWeapon));
            var enemyDir = new Vector3(target.transform.position.x, 0, target.transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z);
            transform.forward = enemyDir;
        };
        charge.OnEnter += action => 
        {
            instancedChargeParticle = Instantiate(particleCharge, transform.position + transform.forward * 3, transform.rotation);
            instancedChargeParticle.transform.Rotate(180,0,0);
            instancedChargeParticle.transform.SetParent(transform,true);
            charging = true;
            _animator.Play("Chase");
            minotaurChargeCollider.SetActive(true);
            //_animator.SetTrigger("Chase");
        };
        recover.OnEnter += action => 
        {
           // _animator.Play("Idle");
            StartCoroutine(RecoverTimer(timeToRecover));
        };
        chargeChanneling.OnEnter += action => 
        {
            _animator.Play("PrepareChase");
            //_animator.SetTrigger("PrepareChase");
            var enemyDir = new Vector3(target.transform.position.x, 0, target.transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z);
            transform.forward = enemyDir;
            StartCoroutine(ChargeChanneling(timeToCharge));
            chargeProjector.enabled = true;
            _savePos = transform.position;
        };
        
        skillAOE.OnEnter += action =>
        {
            _animator.Play("AOESkill");
            StartCoroutine(SkillAOETimer(timeToDoAOESkill));
        };

        death.OnEnter += action =>
        {            
            projectorAOEFillDamage.SetActive(false);
            projectorAOEFillStun.SetActive(false);
            //Destroy(projectorAOEFillDamage);
            //Destroy(AOEDamageProjector);
            //Destroy(AOEStunProjector);
            StopAllCoroutines();
            Destroy(canvasHP);
            var temp = transform.FindChild("ParticleOnFire 1(Clone)");
            if (temp != null)
            {
                Destroy(temp.gameObject);
            }
            dead = true;
            DissolveAfterDeath();
            _animator.Play("Death");
            EventsManager.TriggerEvent(EventsType.firstBossAfter);
            Destroy(colliderDistance);
            gameObject.layer = (int)LayersEnum.IGNOREPLAYER;
        };

        #endregion

        #region OnUpdate
        chase.OnUpdate += OnUpdateChasing;
        attack.OnUpdate += OnUpdateAttack;
        charge.OnUpdate += OnUpdateCharge;
        skillAOE.OnUpdate += OnUpdateAoe;
        #endregion

        #region OnExit
        chargeChanneling.OnExit += action => chargeProjector.enabled = false;
        skillAOE.OnExit += action =>
        {
            Instantiate(particleGround, new Vector3(transform.position.x, transform.position.y +0.3f, transform.position.z), Quaternion.Euler(-90,0,0));
            AOEDamageProjector.enabled = false;
            AOEStunProjector.enabled = false;
            _hitsCount = 0;
            _randomHitsCount = Random.Range(hitsMinCount, hitsMaxCount);
        };
        charge.OnExit += action => 
        {
            // Destroy();
            var em2 = instancedChargeParticle.GetComponent<ParticleSystem>().emission;
            em2.enabled = false;
            em2.rateOverTime = 0;
            foreach (Transform item in instancedChargeParticle.transform)
            {
                var em = item.GetComponent<ParticleSystem>().emission;
                em.enabled = false;
                em.rateOverTime = 0;
            }
            StartCoroutine(KillChargeParticle());

            charging = false;
            minotaurChargeCollider.SetActive(false);            
        };
        #endregion

        _fsm = new EventFSM<MinotaurBossAction>(idle);
    }
	IEnumerator KillChargeParticle()
    {
        yield return new WaitForSeconds(1f);
        Destroy(instancedChargeParticle);
    }
    void OnUpdateChasing()
    {
        var enemyDir = new Vector3(target.transform.position.x, 0, target.transform.position.z) - new Vector3(transform.position.x,0,transform.position.z);
        transform.forward = Vector3.Lerp(transform.forward, enemyDir, rotationSpeed * Time.deltaTime);
        transform.position += transform.forward * speed * Time.deltaTime;
    }
    void OnUpdateAoe()
    {
        var finalScale = new Vector3(32.6f, 32.6f, 32.6f);
        projectorAOEFillDamage.transform.localScale = Vector3.Lerp(projectorAOEFillDamage.transform.localScale, finalScale, 6f * Time.deltaTime);
            var finalScale2 = new Vector3(42.6f, 42.6f, 42.6f);
        projectorAOEFillStun.transform.localScale = Vector3.Lerp(projectorAOEFillStun.transform.localScale, finalScale2, 6f * Time.deltaTime);
    }
    void OnUpdateAttack()
    {
        var enemyDir = new Vector3(target.transform.position.x, 0, target.transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z);
        transform.forward = Vector3.Lerp(transform.forward, enemyDir, rotationSpeed * Time.deltaTime);
    }

    void OnUpdateCharge()
    {
        RaycastHit rch;
        if (Vector3.Distance(_savePos, transform.position) >= ChargeMaxDistance)
        {
            _animator.SetTrigger("Stun");
            _fsm.Feed(MinotaurBossAction.Recover);
        }
        else if (!Physics.Raycast(transform.position + (Vector3.up / 2), transform.forward + (Vector3.up/2), out rch, raycastDistance, collisionable) && charging == true)
        {
            transform.position += transform.forward * chargeSpeed * Time.deltaTime;
        }       
        else
        {
            CameraShake.instance.Shake();
            _animator.SetTrigger("Stun");
            _fsm.Feed(MinotaurBossAction.Recover);
        }
    }



    void Update ()
    {
        _fsm.Update();
        HealthBar();

        if (target.health <= 0)
            return;

        if (health <= 0)
        {            
            _fsm.Feed(MinotaurBossAction.Death);
        }
        var distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        if (distanceToTarget >= distanceToCharge)
        {
            _fsm.Feed(MinotaurBossAction.ChargeChanneling);
        }

        else if (_hitsCount >= _randomHitsCount && distanceToTarget <= distanceToChase)
        {
            _fsm.Feed(MinotaurBossAction.SkillAOE);
        }

        else if (distanceToTarget >= distanceToChase && distanceToTarget <= distanceToCharge)
        {           
            _fsm.Feed(MinotaurBossAction.Chase);
        }          
        else
        {
            _fsm.Feed(MinotaurBossAction.Attack);
        }
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

    IEnumerator RecoverTimer(float timeToRecover)
    {
        yield return new WaitForSeconds(timeToRecover);
        _fsm.Feed(MinotaurBossAction.Idle);
    }

    IEnumerator ChargeChanneling(float timeToCharge)
    {
        yield return new WaitForSeconds(timeToCharge);
        _fsm.Feed(MinotaurBossAction.Charge);
    }

    IEnumerator DeactivateWeapon(float timeToActivateWeapon)
    {
        yield return new WaitForSeconds(timeToActivateWeapon);
        if (dead)
            yield break;
        weapon.SetActive(true);
        yield return new WaitForSeconds(0.4f); //para volver a desactivar el collider del arma
        weapon.SetActive(false);
    }

    IEnumerator SkillAOETimer(float timeToDoDamage)
    {
        AOEDamageProjector.enabled = true;
        AOEStunProjector.enabled = true;
        projectorAOEFillDamage.SetActive(true);
        projectorAOEFillStun.SetActive(true);
        projectorAOEFillDamage.transform.localScale = new Vector3(0, 0, 0);
     
        projectorAOEFillStun.transform.localScale = new Vector3(0, 0, 0);

        yield return new WaitForSeconds(timeToDoDamage);

        CameraShake.instance.Shake();

        var distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance <= 7)
        {
            EventsManager.TriggerEvent(EventsType.spawnText, new object[] { target.healthBar.GetComponentInParent<RectTransform>(), damageAOESkill, Color.red });


            //target.health -= damageAOESkill;
            if (target.healing)
            {
                target.saveHp -= damageAOESkill;
            }
            else
            {
                target.health -= damageAOESkill;

            }
        }
        else if (distance >= 7 && distance <= 8)
        {
            //Hero.inputBlock = true;
            target.stun = true;
            EventsManager.TriggerEvent(EventsType.minotaurStunn);
            StartCoroutine(target.UnStun(stunTime));
        }

        projectorAOEFillDamage.SetActive(false);
        projectorAOEFillStun.SetActive(false);


        _fsm.Feed(MinotaurBossAction.Recover);
    }

    protected void DissolveAfterDeath()
    {
        
        _dissolveShaderDelayCurrent += Time.deltaTime;

        if (_dissolveShaderDelayCurrent >= _dissolveShaderDelayTotal)
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

            if (_dissolveShaderLerpTime >= 0.5f)
            {
                portal.SetActive(true);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + (target.transform.position - transform.position).normalized * raycastDistance);
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == (int)LayersEnum.SWORD)
        {
            var dmg = c.gameObject.GetComponentInParent<HeroModel>().damage;
            health -= dmg;
            _hitsCount++;
            if (canvasHP.gameObject != null)
            {
                EventsManager.TriggerEvent(EventsType.spawnText, new object[] { canvasHP.GetComponentInParent<RectTransform>(), dmg,textColor});
            }
        }
    }
}
