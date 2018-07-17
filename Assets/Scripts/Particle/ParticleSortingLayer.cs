using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSortingLayer : MonoBehaviour {

	
	void Start () {
        GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerName = "Particles";
        GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingOrder = 2;
    }
	
}
