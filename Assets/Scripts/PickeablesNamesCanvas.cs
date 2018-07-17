using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PickeablesNamesCanvas : MonoBehaviour {

    public GameObject canvasToParent;
    public GameObject prefabImageNText;
    private Pool<GameObject> _textNamesPool;
    public static PickeablesNamesCanvas instance;
    public List<PickupItem> allDrops = new List<PickupItem>();
    


    void Start () {
        if(instance == null)
        {
            instance = this;
            _textNamesPool = new Pool<GameObject>(20, FactoryTextNames, InitializeTextNames, DisposeTextNames, true);
          
            EventsManager.SubscribeToEvent(EventsType.spawnNamesText, spawnNamesText);
            EventsManager.SubscribeToEvent(EventsType.changeScene, sceneChanged);
           // EventsManager.SubscribeToEvent(EventType.suscribePickUpItem, SuscribeDropList);

        }
     
    }

    //public void SuscribeDropList(params object[] p)
    //{
    //    allDrops.Add((PickupItem0)p[0]);

    //    UpdateListDropNames();
    //}

    //public void UpdateListDropNames()
    //{
    //    StartCoroutine(UpdateDropList());
    //}
    //IEnumerator UpdateDropList()
    //{
    //    foreach (var item in allDrops)
    //    {
    //        yield return new WaitForEndOfFrame();

            
    //        var l = Physics.OverlapSphere(transform.position, 2, 1 << 26);
    //        item.lColl.AddRange(l);
    //        item.lColl.Remove(GetComponent<Collider>());
    //        item.stackCount = item.lColl.Count;
    //        if (!item.isSkillSave)
    //        {

    //            EventsManager.TriggerEvent(EventType.spawnNamesText, item.reliquiasColor, item.titleSave, item.stackCount, item.gameObject, item.GetComponent<PickupItem0>());

    //        }
    //        else
    //        {
    //            EventsManager.TriggerEvent(EventType.spawnNamesText, item.skillColor, item.titleSave, item.stackCount, item.gameObject, item.GetComponent<PickupItem0>());

    //        }


    //    }
    //}

    public void sceneChanged(params object[] p)
    {
        EventsManager.UnsubscribeToEvent(EventsType.spawnNamesText, spawnNamesText);
        EventsManager.UnsubscribeToEvent(EventsType.changeScene, sceneChanged);
       // EventsManager.UnsubscribeToEvent(EventType.suscribePickUpItem, SuscribeDropList);

    }

    public void ReturnTextNamesToPool(GameObject textDamage)
    {
        _textNamesPool.DisablePoolObject(textDamage);

    }
    public static void DisposeTextNames(GameObject textNames)
    {
        textNames.GetComponentInChildren<Text>().text = "";
        textNames.SetActive(false);
        textNames.transform.SetParent(instance.transform);
    }
    public static void InitializeTextNames(GameObject textNames)
    {

        textNames.gameObject.SetActive(true);
        textNames.GetComponent<ReleaseAlt>().Set();
    }


    public void Reset()
    {
        EventsManager.UnsubscribeToEvent(EventsType.spawnText, spawnNamesText);
        EventsManager.UnsubscribeToEvent(EventsType.spawnNamesText, spawnNamesText);
    }
    public GameObject FactoryTextNames()
    {
        var t = Instantiate<GameObject>(prefabImageNText);
        t.transform.SetParent(canvasToParent.transform);
        return t;
    }


    public void spawnNamesText(params object[] parameters)
    {
        var color = (Color)parameters[0];
        var name = (string)parameters[1];
        var offset = (int)parameters[2];


        var t = _textNamesPool.GetObjectFromPool();

        var s = (PickupItem)parameters[4];
        s.saveRRelease = t.gameObject;
        t.GetComponent<ReleaseAlt>().sPickUp = s;
        t.GetComponentInChildren<Text>().text = name;
        t.GetComponent<Image>().color = color;
        t.transform.SetParent(canvasToParent.transform);


        EventsManager.TriggerEvent(EventsType.followNameText,parameters[3],canvasToParent,offset);


    }
    void despawnNamesText(params object[] parameters)
    {
        var pos = (Transform)parameters[0];
        var dmg = (float)parameters[1];
        var t = _textNamesPool.GetObjectFromPool();
        t.GetComponent<Text>().text = dmg.ToString();
        t.transform.SetParent(pos);
        t.transform.position = pos.position;
    }

    void Update()
    {
        //if (Input.GetKey(KeyCode.LeftAlt))
        //{


        //    UpdateListDropNames();
        //}
    }



   
}
