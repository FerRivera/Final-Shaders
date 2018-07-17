using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupText : MonoBehaviour
{
    private TextMesh _text;
    public string textToShow;
    public float speed;
    public float time;
    Color color;

	// Use this for initialization
	void Start ()
    {
        _text = GetComponent<TextMesh>();
        _text.text = textToShow;
        color = GetComponent<Renderer>().material.color;


       
    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.position += transform.up * speed * Time.deltaTime;

        color.a -= Time.deltaTime / time;
        GetComponent<Renderer>().material.color = color;
    }

   
}
