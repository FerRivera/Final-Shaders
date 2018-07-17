using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillObtainedRay : MonoBehaviour
{
    public float rotationSpeed;
    public float fallingSpeed;
    public GameObject clouds;

    void Start ()
    {
		
	}
	
	void Update ()
    {
        transform.position += new Vector3(0, fallingSpeed, 0) * Time.deltaTime;
        transform.Rotate(new Vector3(0,0, rotationSpeed));
        clouds.transform.position = Finder.Instance.hero.transform.position;
	}
}
