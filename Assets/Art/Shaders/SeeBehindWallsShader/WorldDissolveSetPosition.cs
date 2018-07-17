using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldDissolveSetPosition : MonoBehaviour {

    public Transform position;
    Material dissolveMat;


    void Awake()
    {
        dissolveMat = GetComponent<Renderer>().material;

    }

    void Update()
    {
        dissolveMat.SetVector("_CenterPos",position.position);
    }
}
