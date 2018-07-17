using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.UI;
using System;

public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Item item;
    public int amount;
    public int slot;
    private Inventory inv;
    private InventoryForge invForge;

    private Tooltip tooltip;
    private Vector2 offset;
    public bool wasInForge;
    Transform canvas;

    void Start()
    {
       // canvas = GameObject.Find("Canvas").transform;
        canvas = Finder.Instance.canvas.transform;
        inv = Finder.Instance.inventory;
        invForge = Finder.Instance.inventory.GetComponent<InventoryForge>();
        tooltip = inv.GetComponent<Tooltip>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        GetComponent<AspectRatioFitter>().enabled = false;
       if(inv.slots[slot].GetComponent<Slot>().locked)
       {
           // item = null;
            return;
       }
        if (item != null && item.SlotsUsed == 1)
        {
            AudioManager.instance.PlaySound(SoundsEnum.GENERAL_DRAG_ITEM);

            offset = eventData.position - new Vector2(this.transform.position.x, this.transform.position.y);
            //this.transform.SetParent(this.transform.parent.parent.parent.parent);
           
            this.transform.SetParent(canvas);
            this.transform.position = eventData.position - offset;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
            //ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData>();
            // inv.items[droppedItem.slot] = new Item();
        }
        else if (item != null && item.SlotsUsed == 2)
        {
            //offset = eventData.position - new Vector2(this.transform.position.x, this.transform.position.y);
            offset = new Vector2(-this.GetComponent<RectTransform>().rect.width / 3, this.GetComponent<RectTransform>().rect.height / 3);
            //this.transform.SetParent(this.transform.parent.parent.parent.parent);
            this.transform.SetParent(canvas);

            this.transform.position = eventData.position - offset;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
            ItemData itemGrabbed = eventData.pointerDrag.GetComponent<ItemData>();
            if (itemGrabbed.wasInForge)
            {
                invForge.items[itemGrabbed.slot] = new Item();
                invForge.items[itemGrabbed.slot + 1] = new Item();
            }
            inv.items[itemGrabbed.slot] = new Item();
            inv.items[itemGrabbed.slot + 1] = new Item();
        }
        else if (item != null && item.SlotsUsed == 4)
        {
            //offset = eventData.position - new Vector2(this.transform.position.x, this.transform.position.y);

            offset = new Vector2(-this.GetComponent<RectTransform>().rect.width / 3, this.GetComponent<RectTransform>().rect.height / 3);

            //this.transform.SetParent(this.transform.parent.parent.parent.parent);
            this.transform.SetParent(canvas);
            this.transform.position = eventData.position - offset;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
            ItemData itemGrabbed = eventData.pointerDrag.GetComponent<ItemData>();
            inv.items[itemGrabbed.slot] = new Item();
            inv.items[itemGrabbed.slot + 1] = new Item();
            inv.items[itemGrabbed.slot + 6] = new Item();
            inv.items[itemGrabbed.slot + 7] = new Item();

        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (inv.slots[slot].GetComponent<Slot>().locked)
        {
            //item = null;
            return;
        }
        if (item != null)
        {
            this.transform.position = eventData.position - offset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<AspectRatioFitter>().enabled = true;

        if (inv.slots[slot].GetComponent<Slot>().locked)
         {
             return;
         }
        if (eventData.pointerDrag.GetComponent<ItemData>().item.SlotsUsed == 1)
        {
            if (eventData.pointerDrag.GetComponent<ItemData>().wasInForge)
            {
                this.transform.SetParent(invForge.slots[slot].transform);
                this.transform.position = invForge.slots[slot].transform.position;
                GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
            else
            {
                this.transform.SetParent(inv.slots[slot].transform);
      
             
                  //  transform.localScale = new Vector3(1, 1, 1);
                
                this.transform.position = inv.slots[slot].transform.position;
                GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
            

        }

        else if (eventData.pointerDrag.GetComponent<ItemData>().item.SlotsUsed == 2)
        {
            if (eventData.pointerDrag.GetComponent<ItemData>().wasInForge)
            {
                this.transform.SetParent(invForge.slots[slot].transform);
                this.transform.position = invForge.slots[slot].transform.position + new Vector3(33, 0);
                GetComponent<CanvasGroup>().blocksRaycasts = true;
                returnItemToSlot(eventData.pointerDrag.GetComponent<ItemData>(), 2);
            }
            else
            {
                this.transform.SetParent(inv.slots[slot].transform);
                this.transform.position = inv.slots[slot].transform.position + new Vector3(33, 0);
                GetComponent<CanvasGroup>().blocksRaycasts = true;
                returnItemToSlot(eventData.pointerDrag.GetComponent<ItemData>(), 2);
            }
        }
        else if (eventData.pointerDrag.GetComponent<ItemData>().item.SlotsUsed == 4)
        {

            this.transform.SetParent(inv.slots[slot].transform);
            this.transform.position = inv.slots[slot].transform.position + new Vector3(33, -30);
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            returnItemToSlot(eventData.pointerDrag.GetComponent<ItemData>(), 4);

        }

    }
   
    void returnItemToSlot(ItemData data, int numberOfSlots )
    {
        if(numberOfSlots == 2)
        {
            if(data.wasInForge)
            {
                invForge.items[data.slot] = data.item;
                invForge.items[data.slot + 1] = data.item;
            }
            else
            {
                inv.items[data.slot] = data.item;
                inv.items[data.slot + 1] = data.item;
            }
           // Debug.Log("item in place again 2");
        }
        else if(numberOfSlots == 4)
        {
            inv.items[data.slot] = data.item;
            inv.items[data.slot + 1] = data.item;
            inv.items[data.slot + 6] = data.item;
            inv.items[data.slot + 7] = data.item;
           // Debug.Log("item in place again 4");
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (inv.slots[slot].GetComponent<Slot>().locked)
        {           
            return;
        }
        if (item != null)
        {
            offset = eventData.position - new Vector2(this.transform.position.x, this.transform.position.y);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.Activate(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.Deactivate();
    }

   
}
