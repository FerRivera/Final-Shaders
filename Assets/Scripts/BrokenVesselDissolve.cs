using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenVesselDissolve : MonoBehaviour {

    public string vessselType;
    bool _on;
    float _dissolveShaderCurrentLerpTime;
    float _dissolveShaderLerpTime;
    float _dissolveShaderDelayTotal;
    float _dissolveShaderDelayCurrent;
    Material _dissolveMaterial;

    void Start()
    {
        _dissolveMaterial = (Material)Resources.Load("VesselsMaterials/" + vessselType);
        gameObject.GetComponent<Renderer>().material = _dissolveMaterial;
        gameObject.GetComponent<Renderer>().material.SetFloat("_Dissolve", -0.1f);
        _dissolveShaderCurrentLerpTime = 0;
        _dissolveShaderLerpTime = 4;
        _dissolveShaderDelayTotal = 1;
        _on = false;
        StartCoroutine(DissolveCoroutine());
    }
    IEnumerator DissolveCoroutine()
    {
        yield return new WaitForSeconds(0.4f);
        _on = true;
       
    }

    void Update()
    {
        if(_on)
        {
            DissolveAfterDeath();

        }
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

          //  gameObject.GetComponent<Renderer>().material = _dissolveMaterial;

            var lerp = Mathf.Lerp(-0.1f, 0.6f, perc);

            gameObject.GetComponent<Renderer>().material.SetFloat("_Dissolve", lerp);
        }
    }
}
