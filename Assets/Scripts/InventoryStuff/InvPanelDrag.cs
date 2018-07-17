using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class InvPanelDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerClickHandler
{
    private Vector2 _offset;
    public Transform target;
    public bool usingTarget;
   
    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
        if (usingTarget)
        {
            _offset = eventData.position - new Vector2(target.transform.position.x, target.transform.position.y);
        }
        else
        {
            _offset = eventData.position - new Vector2(this.transform.position.x, this.transform.position.y);
        }
     
        
    }

    public void OnDrag(PointerEventData eventData)
    {      
        if(usingTarget)
        {
            target.transform.position = eventData.position - _offset;
        }
        else
        {
            this.transform.position = eventData.position - _offset;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
    }
}
