using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillRoomAbsorb : MonoBehaviour {

    public GameObject skillRoom;
    public GameObject skillRoomSphere;
    public GameObject heroAura;
    public GameObject cloud;
    public GameObject ray;
    Material _skillRoomShader;
    Material _skillRoomShaderSphere;
    float _absorbCurrentLerpTime;
    float _absorbLerpTime;
    bool _startAbsorb;
    float _absorbCurrentLerpTimeSphere;
    float _absorbLerpTimeSphere;
    float _heroAuraCurrentLerp;
    float _heroAuraLerpTime;
    bool _startAbsorbSphere;
    bool _startHeroAuraOnce;
    bool _endHeroAura;


    void Awake()
    {
        EventsManager.SubscribeToEvent(EventsType.resetHeroAura, ResetHeroAura);

        _skillRoomShader = skillRoom.GetComponent<Renderer>().material;
        _skillRoomShaderSphere = skillRoomSphere.GetComponent<Renderer>().material;

        _absorbCurrentLerpTime = 0;
        _absorbLerpTime = 2;

        _absorbCurrentLerpTimeSphere = 0;
        _absorbLerpTimeSphere = 1;

        _heroAuraCurrentLerp = 0;
        _heroAuraLerpTime = 2;

    }
    protected void AbsorbSkill()
    {
        ray.SetActive(true);
        _absorbCurrentLerpTime += Time.deltaTime;
        if (_absorbCurrentLerpTime > _absorbLerpTime/2)
        {
            _startAbsorbSphere = true;

        }
        if (_absorbCurrentLerpTime > _absorbLerpTime)
        {
            _absorbCurrentLerpTime = _absorbLerpTime;
        }

        float perc = _absorbCurrentLerpTime / _absorbLerpTime;

        var lerp = Mathf.Lerp(1.25f, 0, perc);
        
        skillRoom.GetComponent<Renderer>().material.SetFloat("_Hardness", lerp);

        var lerp2 = Mathf.Lerp(0, 3, perc);        

        if (_startHeroAuraOnce && !_endHeroAura)
            heroAura.GetComponent<Renderer>().material.SetFloat("_Opacity", lerp2);

        if (_endHeroAura)
        {
            _heroAuraCurrentLerp += Time.deltaTime;

            if (_heroAuraCurrentLerp > _heroAuraLerpTime / 2)
            {
                _startAbsorbSphere = true;
            }

            if (_heroAuraCurrentLerp > _heroAuraLerpTime)
            {
                _heroAuraCurrentLerp = _heroAuraLerpTime;
            }

            float perc2 = _heroAuraCurrentLerp / _heroAuraLerpTime;

            var lerp3 = Mathf.Lerp(3, 0, perc2);

            heroAura.GetComponent<Renderer>().material.SetFloat("_Opacity", lerp3);

            Debug.Log(lerp3);

            if(lerp3 <= 0)
                heroAura.SetActive(false);
        }
            
    }
    protected void AbsorbSkillSphere()
    {

        _absorbCurrentLerpTimeSphere += Time.deltaTime;
        if (_absorbCurrentLerpTimeSphere > _absorbLerpTimeSphere)
        {
            _absorbCurrentLerpTimeSphere = _absorbLerpTimeSphere;
        }

        float perc = _absorbCurrentLerpTimeSphere / _absorbLerpTimeSphere;
        
        var colorLerp = Color.Lerp(skillRoomSphere.GetComponent<Renderer>().material.GetColor("_SphereColor"), Color.black, perc);
        skillRoomSphere.GetComponent<Renderer>().material.SetColor("_SphereColor", colorLerp);

    }

    void StartAura()
    {

    }

    void Update()
    {
        if(_startAbsorb)
        {
            AbsorbSkill();
        }
        if(_startAbsorbSphere)
        {
            AbsorbSkillSphere();
        }
    }

    void OnTriggerEnter(Collider c)
    {
        if(c.gameObject.layer == (int)LayersEnum.HERO)
        {
            _startAbsorb = true;            
            if (!_startHeroAuraOnce)
                StartCoroutine(StopHeroAura());            
        }
    }

    public void ResetHeroAura(params object[] p)
    {
        _startHeroAuraOnce = false;
        _endHeroAura = false;
        _heroAuraCurrentLerp = 0;
        _heroAuraLerpTime = 2;
    }

    IEnumerator StopHeroAura()
    {
        heroAura.SetActive(true);
        _startHeroAuraOnce = true;
        if (cloud != null)
            cloud.SetActive(true);
        yield return new WaitForSeconds(6);
        cloud.SetActive(false);
        _endHeroAura = true;
        yield return new WaitForSeconds(5);
        EventsManager.TriggerEvent(EventsType.resetRayAura);
        Destroy(gameObject.transform.parent.gameObject);
    }


}
