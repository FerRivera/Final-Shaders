using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldDissolveSetPositionMulti : MonoBehaviour {

    public Transform position;
    Material _dissolveMat;
    Renderer _renderer;

    void Awake()
    {
        _dissolveMat = GetComponent<Renderer>().material;
        _renderer = GetComponent<Renderer>();

    }

    void Update()
    {
        foreach (var item in _renderer.materials)
        {
            item.SetVector("_CenterPos", position.position);
        }
    }
}
