using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeroCollisions : MonoBehaviour
{
    HeroModel _hero;
    MinotaurBoss _minotaurBoss;
    List<SoundsEnum> _hitsSoundAmount = new List<SoundsEnum>();
    //Coroutine _crHitSound;
    public float timeToRepeatHitSound;
    bool _canRepeatHitSound;

    private void Start()
    {
        _canRepeatHitSound = true;
        _hitsSoundAmount.Add(SoundsEnum.HERO_INJURY_1);
        _hitsSoundAmount.Add(SoundsEnum.HERO_INJURY_2);
        _hitsSoundAmount.Add(SoundsEnum.HERO_INJURY_3);

        _hero = GetComponent<HeroModel>();
        _minotaurBoss = Finder.Instance.minotaurBoss;
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == (int)LayersEnum.PORTAL)
        {
            //if (c.gameObject.GetComponent<PortalLevel>().pLevelSelect == Scenes.Main)
            //{
            //    EventsManager.TriggerEvent(EventsType.changeScene);
            //    EventsManager.UnsubscribeToEvent(EventsType.spawnNamesText, PickeablesNamesCanvas.instance.spawnNamesText);

            //    SceneManager.LoadScene((int)Scenes.Main);
            //}
            //else if (c.gameObject.GetComponent<PortalLevel>().pLevelSelect == Scenes.Level1)
            //{
            //    _hero.inv.SaveInventory();
            //    EventsManager.TriggerEvent(EventsType.changeScene);

            //    EventsManager.UnsubscribeToEvent(EventsType.spawnNamesText, PickeablesNamesCanvas.instance.spawnNamesText);

            //    SceneManager.LoadScene((int)Scenes.Level1);
            //}
            //else if (c.gameObject.GetComponent<PortalLevel>().pLevelSelect == Scenes.Hub)
            //{
            //    EventsManager.TriggerEvent(EventsType.changeScene);

            //    EventsManager.UnsubscribeToEvent(EventsType.spawnNamesText, PickeablesNamesCanvas.instance.spawnNamesText);

            //    SceneManager.LoadScene((int)Scenes.Hub);
            //}
        }

        if (c.gameObject.layer == (int)LayersEnum.ENEMYWEAPON)
        {
            GameObject enemyWeapon = null;
            float enemyWeaponDamage = 0;

            if (c.gameObject.GetComponentInParent<EntityFSM>())
            {
                enemyWeaponDamage = c.gameObject.GetComponentInParent<EntityFSM>().damage;
                enemyWeapon = c.gameObject.GetComponentInParent<EntityFSM>().weapon;
            }

            if (_hero.shieldActive && _hero.InFrontOfShield(c.gameObject.transform.parent.position))
            {
                AudioManager.instance.PlaySound(SoundsEnum.HERO_IMPACT_ON_SHIELD);
                _hero.stamina -= _hero.blockStaminaCost;
                var finaldamageBlocked = _hero.CalcFinalShieldDefense(enemyWeaponDamage) - enemyWeaponDamage;
                finaldamageBlocked = Mathf.Abs(finaldamageBlocked);
                finaldamageBlocked = Mathf.RoundToInt(finaldamageBlocked);
                _hero.health -= finaldamageBlocked;

                EventsManager.TriggerEvent(EventsType.retaliationSkill,new object[] { c.gameObject.GetComponentInParent<EnemyCharacter>(), finaldamageBlocked});
                EventsManager.TriggerEvent(EventsType.spawnText, new object[] { _hero.healthBar.GetComponentInParent<RectTransform>(), finaldamageBlocked, _hero.textColor });
                if (_hero.healing)
                {
                    _hero.saveHp -= (int)finaldamageBlocked;
                }
                else
                {
                    _hero.health -= (int)finaldamageBlocked;
                }

                return;
            }
            else
            {
                float finalDamage = 0;
                _hero.CalcFinalDefense(enemyWeaponDamage);

                if (_hero.damageToReduce > 0)
                    finalDamage = enemyWeaponDamage - _hero.damageToReduce;
                else
                    finalDamage = enemyWeaponDamage;

                if (!_hero.dashing)
                {
                    if (_canRepeatHitSound)
                    {
                        AudioManager.instance.PlaySound(_hitsSoundAmount[Random.Range(0, _hitsSoundAmount.Count)]);
                        StartCoroutine(HitSound());
                        _canRepeatHitSound = false;
                    }
                    
                    EventsManager.TriggerEvent(EventsType.retaliationSkill, new object[] { c.gameObject.GetComponentInParent<EnemyCharacter>(), finalDamage });
                    EventsManager.TriggerEvent(EventsType.spawnText, new object[] { _hero.healthBar.GetComponentInParent<RectTransform>(), finalDamage, _hero.textColor });
                    if (_hero.healing)
                    {
                        _hero.saveHp -= (int)finalDamage;
                    }
                    else
                    {
                        _hero.health -= (int)finalDamage;
                    }
                }

                enemyWeapon.SetActive(false);
            }
        }
        if (c.gameObject.layer == (int)LayersEnum.BOSSMINOTAURWEAPON)
        {
            float enemyWeaponDamage = c.gameObject.GetComponentInParent<MinotaurBoss>().damage;
            GameObject enemyWeapon = c.gameObject.GetComponentInParent<MinotaurBoss>().weapon;

            if (_hero.shieldActive && _hero.InFrontOfShield(c.gameObject.transform.parent.position))
            {
                AudioManager.instance.PlaySound(SoundsEnum.HERO_IMPACT_ON_SHIELD);
                _hero.stamina -= _hero.blockStaminaCost;  
                var finaldamageBlocked = _hero.CalcFinalShieldDefense(enemyWeaponDamage) - enemyWeaponDamage;
                finaldamageBlocked = Mathf.Abs(finaldamageBlocked);
                finaldamageBlocked = Mathf.RoundToInt(finaldamageBlocked);
                _hero.health -= finaldamageBlocked;

                EventsManager.TriggerEvent(EventsType.retaliationSkill, new object[] { c.gameObject.GetComponentInParent<EnemyCharacter>(), finaldamageBlocked });
                EventsManager.TriggerEvent(EventsType.spawnText, new object[] { _hero.healthBar.GetComponentInParent<RectTransform>(), finaldamageBlocked, _hero.textColor });
                if (_hero.healing)
                {
                    _hero.saveHp -= (int)finaldamageBlocked;
                }
                else
                {
                    _hero.health -= (int)finaldamageBlocked;
                }
                return;
            }
            else
            {
                float finalDamage = 0;
                _hero.CalcFinalDefense(enemyWeaponDamage);

                if (_hero.damageToReduce > 0)
                    finalDamage = enemyWeaponDamage - _hero.damageToReduce;
                else
                    finalDamage = enemyWeaponDamage;

                if (!_hero.dashing)
                {
                    if (_canRepeatHitSound)
                    {
                        AudioManager.instance.PlaySound(_hitsSoundAmount[Random.Range(0, _hitsSoundAmount.Count)]);
                        StartCoroutine(HitSound());
                        _canRepeatHitSound = false;
                    }
                    EventsManager.TriggerEvent(EventsType.retaliationSkill, new object[] { c.gameObject.GetComponentInParent<EnemyCharacter>(), finalDamage });
                    EventsManager.TriggerEvent(EventsType.spawnText, new object[] { _hero.healthBar.GetComponentInParent<RectTransform>(), finalDamage, _hero.textColor });
                    if (_hero.healing)
                    {
                        _hero.saveHp -= (int)finalDamage;
                    }
                    else
                    {
                        _hero.health -= (int)finalDamage;
                    }
                }

                enemyWeapon.SetActive(false);
            }
        }

        if (c.gameObject.layer == (int)LayersEnum.MINOTAURCHARGECOLLIDER /*&& c.gameObject.GetComponent<MinotaurBoss>().charging*/)
        {
            float enemyChargeDamage = _minotaurBoss.chargeDamage;

            float finalDamage = 0;
            _hero.CalcFinalDefense(enemyChargeDamage);

            if (_hero.damageToReduce > 0)            
                finalDamage = enemyChargeDamage - _hero.damageToReduce;            
            else            
                finalDamage = enemyChargeDamage;

            if (!_hero.dashing)
            {
                if (!_hero.chargeHit)
                {
                    AudioManager.instance.PlaySound(_hitsSoundAmount[Random.Range(0, _hitsSoundAmount.Count)]);
                    _hero.chargeHit = true;
                    StartCoroutine(_hero.ChargeMinotaurDamageAgain());
                    EventsManager.TriggerEvent(EventsType.retaliationSkill, new object[] { c.gameObject.GetComponentInParent<EnemyCharacter>(), finalDamage });
                    EventsManager.TriggerEvent(EventsType.spawnText, new object[] { _hero.healthBar.GetComponentInParent<RectTransform>(), finalDamage, _hero.textColor });
                    if (_hero.healing)
                    {
                        _hero.saveHp -= (int)finalDamage;
                    }
                    else
                    {
                        _hero.health -= (int)finalDamage;

                    }
                }                
            }
            _minotaurBoss.charging = false;
        }

        if (c.gameObject.layer == (int)LayersEnum.ENEMYRANGEWEAPON)
        {
            EntityFSM enemy = c.GetComponent<Proyectiles>().owner;
            c.GetComponent<Proyectiles>().DestroyProyectil();
            var pdamage = c.GetComponent<Proyectiles>().damage;

            if (_hero.shieldActive && _hero.InFrontOfShield(c.gameObject.transform.position))
            {                
                AudioManager.instance.PlaySound(SoundsEnum.HERO_IMPACT_ON_SHIELD);
                _hero.stamina -= _hero.blockStaminaCost;
                var finaldamageBlocked = _hero.CalcFinalShieldDefense(pdamage) - pdamage;
                finaldamageBlocked = Mathf.Abs(finaldamageBlocked);
                finaldamageBlocked = Mathf.RoundToInt(finaldamageBlocked);
                _hero.health -= finaldamageBlocked;

                EventsManager.TriggerEvent(EventsType.retaliationSkill, new object[] { enemy.GetComponent<EnemyCharacter>(), finaldamageBlocked });
                EventsManager.TriggerEvent(EventsType.spawnText, new object[] { _hero.healthBar.GetComponentInParent<RectTransform>(), finaldamageBlocked, _hero.textColor });
                if (_hero.healing)
                {
                    _hero.saveHp -= (int)finaldamageBlocked;
                }
                else
                {
                    _hero.health -= (int)finaldamageBlocked;
                }
                return;
            }
            else
            {
                float finalDamage = 0;
                _hero.CalcFinalDefense(pdamage);

                if (_hero.damageToReduce > 0)                
                    finalDamage = pdamage - _hero.damageToReduce;                
                else                
                    finalDamage = pdamage;

                if (!_hero.dashing)
                {
                    if (_canRepeatHitSound)
                    {
                        AudioManager.instance.PlaySound(_hitsSoundAmount[Random.Range(0, _hitsSoundAmount.Count)]);
                        StartCoroutine(HitSound());
                        _canRepeatHitSound = false;
                    }
                    EventsManager.TriggerEvent(EventsType.retaliationSkill, new object[] { enemy.GetComponent<EnemyCharacter>(), finalDamage });
                    EventsManager.TriggerEvent(EventsType.spawnText, new object[] { _hero.healthBar.GetComponentInParent<RectTransform>(), finalDamage, _hero.textColor });
                    if (_hero.healing)
                    {
                        _hero.saveHp -= (int)finalDamage;
                    }
                    else
                    {
                        _hero.health -= (int)finalDamage;

                    }
                }
                c.GetComponent<BoxCollider>().enabled = false;
            }
        }
    }

    public IEnumerator HitSound()
    {
        yield return new WaitForSeconds(timeToRepeatHitSound);
        _canRepeatHitSound = true;
    }
}
