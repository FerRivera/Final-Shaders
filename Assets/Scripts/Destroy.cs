using UnityEngine;
using System.Collections;

public class Destroy : MonoBehaviour
{
    public int time;

	void Start ()
    {
        Destroy(this.gameObject, time);
	}
	
	
}
