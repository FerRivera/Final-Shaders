using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextMovement : MonoBehaviour
{
    public float speedMovement;
    public float timeToDissapear;

    void Start()
    {
        StartCoroutine(BackToThePool(timeToDissapear));

	}
	
	void Update ()
    {
        transform.position += transform.up * speedMovement * Time.deltaTime;
	}

    IEnumerator BackToThePool(float time)
    {
        yield return new WaitForSeconds(time);
        TextManager.DisposeTextDamage(this.gameObject);
    }
}
