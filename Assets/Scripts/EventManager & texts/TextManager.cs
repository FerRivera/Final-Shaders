using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    private Pool<GameObject> _textDamagePool;
    
    public GameObject textDamage;
    public GameObject canvasT;
    public static TextManager instance;

    void Start ()
    {
      if(instance == null)
        {
            instance = this;
            _textDamagePool = new Pool<GameObject>(20, FactoryTextDamage, InitializeTextDamage, DisposeTextDamage, true);
            EventsManager.SubscribeToEvent(EventsType.spawnText, spawnText);       
            EventsManager.SubscribeToEvent(EventsType.changeScene, sceneChanged);
        }       
    }
    public void sceneChanged(params object [] p)
    {
        EventsManager.UnsubscribeToEvent(EventsType.spawnText, spawnText);
        EventsManager.UnsubscribeToEvent(EventsType.changeScene, sceneChanged);
    }

	void Update ()
    {
	    
	}

    void spawnText(params object[] parameters)
    {
        var pos = (Transform)parameters[0];
        var dmg = (float)parameters[1];
        var color = (Color)parameters[2];
        var t = _textDamagePool.GetObjectFromPool();
        dmg = Mathf.Round(dmg);
        t.GetComponent<Text>().color = color;
        t.GetComponent<Text>().text = dmg.ToString();
        t.transform.SetParent(pos);
        t.transform.position = pos.position;
    }

 
    public GameObject FactoryTextDamage()
    {
        var t = Instantiate<GameObject>(textDamage);
        t.transform.SetParent(transform);
        return t;
    }
 

    public static void InitializeTextDamage(GameObject textDamage)
    {
        textDamage.gameObject.SetActive(true);
    }
 
    public void Reset()
    {
        EventsManager.UnsubscribeToEvent(EventsType.spawnText, spawnText);        
    }

    public static void DisposeTextDamage(GameObject textDamage)
    {
        textDamage.GetComponent<Text>().text = "";
        textDamage.SetActive(false);
        textDamage.transform.SetParent(instance.transform);
    }
    public void ReturnTextDamageToPool(GameObject textDamage)
    {
        _textDamagePool.DisablePoolObject(textDamage);
        
    }
}
