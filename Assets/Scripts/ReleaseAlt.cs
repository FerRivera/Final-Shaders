using UnityEngine;
using System.Collections;

public class ReleaseAlt : MonoBehaviour {

    public GameObject toFollow;
    RectTransform rT;
    public GameObject canvasToParent;
    public PickupItem sPickUp;
    public int offSetStack;
    public int amountDistanceStack;

  

    void Start ()
    {
        rT = GetComponent<RectTransform>();        
    }
	public void Set()
    {
        EventsManager.SubscribeToEvent(EventsType.followNameText, FollowName);
        EventsManager.SubscribeToEvent(EventsType.changeScene, sceneChanged);
    }

    public void sceneChanged(params object[] p)
    {
        EventsManager.UnsubscribeToEvent(EventsType.spawnNamesText, FollowName);
        EventsManager.UnsubscribeToEvent(EventsType.changeScene, sceneChanged);
    }

    void FollowName(params object[] p)
    {
      
            toFollow = (GameObject)p[0];
            canvasToParent = (GameObject)p[1];
            offSetStack = (int)p[2];
            EventsManager.UnsubscribeToEvent(EventsType.followNameText, FollowName);


    }
    void Update () {
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            toFollow = null;
            canvasToParent = null;
            offSetStack = 0;
            PickeablesNamesCanvas.instance.ReturnTextNamesToPool(gameObject);
          

        }
        if (toFollow != null && canvasToParent != null)
        {
            rT.anchoredPosition = GetPositionInCanvas(toFollow.transform.position);

        }


    }

    public void PickUp()
    {
     
      

        sPickUp.OnMouseDown(gameObject);


    }
    public void OnMouseUp()
    {
   
        //Hero.inputBlock = false;
    }

    Vector2 GetPositionInCanvas(Vector3 wO)
    {
        RectTransform CanvasRect = canvasToParent.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(wO);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
        ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

        return new Vector2(WorldObject_ScreenPosition.x, WorldObject_ScreenPosition.y + 30 + (offSetStack * amountDistanceStack));
    }
}
