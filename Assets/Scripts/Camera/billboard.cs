using UnityEngine;
using System.Collections;

public class billboard : MonoBehaviour {

	
	// Update is called once per frame
	void Update () {
        transform.LookAt(Camera.main.transform);
    }
}
