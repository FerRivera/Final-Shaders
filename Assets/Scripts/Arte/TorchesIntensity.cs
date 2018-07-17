using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchesIntensity : MonoBehaviour {

    Light _lightToModify;
    float _saveIntensity;
    float _saveRange;

    void Awake()
    {
        _lightToModify = GetComponent<Light>();
        

        _saveIntensity = _lightToModify.intensity;
        _saveRange = _lightToModify.range;


       
    }
    void Start()
    {
       
    }
	void Update () {

        _lightToModify.intensity = 2 + (Mathf.Sin(Time.time) * 0.5f );
        _lightToModify.range = 3 + (Mathf.Sin(Time.time) * 0.5f);


        
    }
}
