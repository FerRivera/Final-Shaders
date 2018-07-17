using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeSkill : MonoBehaviour {

    ParticleSystem _spikeSkill;
    public float timeToPause;


    void Awake()
    {
        _spikeSkill = GetComponent<ParticleSystem>();
        StartCoroutine(WaitTime(timeToPause));
    }

    IEnumerator WaitTime(float t)
    {
        yield return new WaitForSeconds(t);
        _spikeSkill.Pause();

    }
   



}
