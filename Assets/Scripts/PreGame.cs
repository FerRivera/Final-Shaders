using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreGame : MonoBehaviour
{
    public Image image;
    public Text toappear;
	// Use this for initialization
	void Start ()
    {
        image.canvasRenderer.SetAlpha(0.0f);
        image.CrossFadeAlpha(255f, 15, false);
        StartCoroutine(appearText());
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<ScreenManager>().GoToGame();
        }
	}
    public IEnumerator appearText()
    {
        yield return new WaitForSeconds(5);
        toappear.gameObject.SetActive(true);
    }
}
