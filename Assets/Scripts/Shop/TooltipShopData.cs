using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class TooltipShopData : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public Item item;
    
    public TooltipShop tultip;
    void Start()
    {
        tultip = GameObject.Find("Shop").GetComponent<TooltipShop>();
        
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        /*item = eventData.pointerEnter.GetComponent<ItemShop>().item;
        tultip.Activate(item);*/
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //tultip.Deactivate();
    }
}
