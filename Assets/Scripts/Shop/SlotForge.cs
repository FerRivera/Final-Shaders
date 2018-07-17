using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using System;

public class SlotForge : MonoBehaviour, IDropHandler
{
    public int id;
    public ItemData skillInInv;
    private InventoryForge _invForge;
    private Inventory _invGeneral;
    public bool isLocked;
    Image _raycast;

    void Start()
    {
        _invForge = Finder.Instance.inventory.GetComponent<InventoryForge>();
        _invGeneral = Finder.Instance.inventory.GetComponent<Inventory>();

        _raycast = GetComponent<Image>();

    }
    void Update()
    {
        if (_invForge.items[id].ID != -1)
        {
            _raycast.raycastTarget = false;
        }
        else
        {
            _raycast.raycastTarget = true;
        }

    
    }

    public void OnDrop(PointerEventData eventData)
    {
        ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData>();
        
        if (droppedItem == null || isLocked )
        {
            return;
        }
        if (!droppedItem.item.IsGem || droppedItem.item.ID != _invForge.idAuthorized && !_invForge.isInventoryEmpty() || droppedItem.item.Title.Contains("Epic"))
        {
            // ItemData.returnItemToSlot()
            //Debug.Log("NULLLLLL");
            Debug.Log("returned or not authorized");
            return;

        }

        else if (droppedItem.item.SlotsUsed == 2)
        {
            for (int i = 2; i < _invForge.slots.Count; i += 3)
            {
                if (id == i)
                {
                    returnItemToPlace(droppedItem.gameObject);
                    return;
                }
                else if (i > id)
                    break;
            }
        }

        if (droppedItem != null && _invForge.items[id].ID == -1)
        {


            if (droppedItem.item.SlotsUsed == 1 || droppedItem.item.SlotsUsed == 2 && _invForge.items[id+1].ID == -1)
            {             
                if(!droppedItem.wasInForge) // si no estaba en el inv de FORGE
                {
                    if(droppedItem.slot > _invGeneral.slotAmount + _invGeneral.slotAmountSecondaryInv)
                    {
                        _invGeneral.slots[droppedItem.slot].GetComponent<Slot>().CalcHeroNewStats(droppedItem, false, droppedItem.slot, false);
                    }
                     _invForge.AddItem(id,droppedItem.item.ID);
                     skillInInv = droppedItem;
                    _invGeneral.RemoveItemFromSlot(droppedItem.slot,droppedItem.item.ID,droppedItem);
                    _invForge.idAuthorized = droppedItem.item.ID;
                    if (droppedItem.item.SlotsUsed == 1)
                    {
                        _invForge.amountForThisId = 6;
                    }
                    else
                        _invForge.amountForThisId = 3;
                    //Debug.Log("NO ESTABA EN FORGE Y LO CREO");

                }
                else // si estaba
                {
                    _invForge.items[droppedItem.slot] = new Item();
                    _invForge.items[id] = droppedItem.item;
                    droppedItem.slot = id;
                    //Debug.Log("ESTABA EN FORGE Y REPOSICIONO");

                }

            }
                    
        }

        //droppedItem = ITEM QUE ARRASTRO Y SUELTO
        //item = ITEM QUE ESTABA EN EL SLOT EN EL QUE SOLTE
        //SWAPEA ITEMS
        else if (droppedItem.slot != id)
        {
           /* if (this.id >= _invForge.slotAmount && droppedItem.item.IsSkill == false)
            {
                return;
            }*/

            Transform item;

            if (this.transform.childCount > 1)
            {
                item = this.transform.GetChild(1);
            }
            else if (this.transform.childCount > 0)
            {

                item = this.transform.GetChild(0);
            }
            else
            {
                return;
                //returnItemToPlace(droppedItem.gameObject);
            }


            //EL ITEM QUE SOLTE USA 1 SLOT?
            if (droppedItem.item.SlotsUsed == 1)
            {
                // var asd = item.GetComponent<ItemData>();
                //EL ITEM EN EL CUAL SOLTE, USA 1 SLOT?
                if (item.GetComponent<ItemData>().item.SlotsUsed == 1)
                {
                    if (!item.GetComponent<ItemData>().item.IsSkill && droppedItem.slot >= _invForge.slotAmount || _invForge.slots[item.GetComponent<ItemData>().slot].GetComponent<Slot>().locked)
                    {
                        //return;

                        returnItemToPlace(droppedItem.gameObject);
                    }
                    else
                    {
                        item.GetComponent<ItemData>().slot = droppedItem.slot;

                        item.transform.SetParent(_invForge.slots[droppedItem.slot].transform);
                        item.transform.position = _invForge.slots[droppedItem.slot].transform.position;

                        droppedItem.slot = id;
                        droppedItem.transform.SetParent(this.transform);
                        droppedItem.transform.position = this.transform.position;

                        _invForge.items[droppedItem.slot] = item.GetComponent<ItemData>().item;
                        _invForge.items[id] = droppedItem.item;
                    }


                }
                //EL ITEM EN EL CUAL SOLTE, USA 2 SLOTS?


                else if (item.GetComponent<ItemData>().item.SlotsUsed == 2 && _invForge.items[droppedItem.slot + 1].ID == -1 && _invForge.slots[droppedItem.slot].GetComponent<Slot>().id != 23)
                {
                    if (!item.GetComponent<ItemData>().item.IsSkill && droppedItem.slot >= _invForge.slotAmount)
                    {
                        returnItemToPlace(droppedItem.gameObject);

                        //return;
                    }
                    else
                    {
                        itemDe1en2(droppedItem, item);
                    }


                }
                //EL ITEM EN EL CUAL SOLTE, USA 4 SLOTS (2x2)?

                else if (item.GetComponent<ItemData>().item.SlotsUsed == 4)
                {
                    //var temp = inv.items[droppedItem.slot];
                    if (droppedItem.slot + 7 > _invForge.items.Count || droppedItem.slot >= 18 && droppedItem.slot <= 23 || droppedItem.slot == 5 || droppedItem.slot == 11 || droppedItem.slot == 17 || droppedItem.slot == 23)
                    {
                        return;
                    }
                    

                }

            }


            //EL ITEM QUE SOLTE, USA 2 SLOTS?
            else if (droppedItem.item.SlotsUsed == 2)
            {
                //Lo solte en un item de 1 slot?
                if (item.GetComponent<ItemData>().item.SlotsUsed == 1)
                {
                    var temp = item.GetComponent<ItemData>().slot;
                    if (droppedItem.slot == temp + 1)
                    {
                        returnItemToPlace(droppedItem.gameObject);
                    }
                    else if (_invForge.items[id + 1].ID == -1)
                    {
                        itemDe2en1(droppedItem, item);
                    }
                    else
                    {
                        returnItemToPlace(droppedItem.gameObject);
                    }
                }
                //Lo solte en un item de 4 slots?
                else if (item.GetComponent<ItemData>().item.SlotsUsed == 4)
                {
                    var temp = item.GetComponent<ItemData>().slot;
                    if (droppedItem.slot == temp + 1 || droppedItem.slot == temp + 6 || droppedItem.slot == temp + 7)
                    {
                        returnItemToPlace(droppedItem.gameObject);
                    }
                   
                    else //devolvelo donde estaba y queda  todo igual
                    {
                        returnItemToPlace(droppedItem.gameObject);
                    }
                }

                //Lo solte en un item de 2 slots?
                else if (item.GetComponent<ItemData>().item.SlotsUsed == 2)
                {
                    itemDe2en2(droppedItem, item);

                }
                else
                {
                    returnItemToPlace(droppedItem.gameObject);
                }




            }
            //EL ITEM QUE SOLTE, USA 4 SLOTS 2x2?
            else if (droppedItem.item.SlotsUsed == 4)
            {
                //solte el de 4 en uno de 1?
                if (item.GetComponent<ItemData>().item.SlotsUsed == 1)
                {
                    var temp = item.GetComponent<ItemData>().slot;
                    if (droppedItem.slot == temp + 1 || droppedItem.slot == temp + 6 || droppedItem.slot == temp + 7)
                    {
                        returnItemToPlace(droppedItem.gameObject);
                    }
                    
                    else //devolvelo donde estaba y queda  todo igual
                    {
                        returnItemToPlace(droppedItem.gameObject);
                    }
                }
                
                else //devolvelo donde estaba y queda  todo igual
                {
                    returnItemToPlace(droppedItem.gameObject);
                }

            }

        }

    }

    void itemDeDosEMPTY(ItemData droppedItem)
    {
        _invForge.items[droppedItem.slot] = new Item();
        _invForge.items[droppedItem.slot + 1] = new Item();
        _invForge.items[id] = droppedItem.item;
        _invForge.items[id + 1].ID = droppedItem.item.ID;
        droppedItem.slot = id;
    }
    void itemDeDosEMPTYReposition(ItemData droppedItem)
    {
        _invForge.items[droppedItem.slot] = new Item();
        _invForge.items[droppedItem.slot + 1] = new Item();
        _invForge.items[id] = droppedItem.item;
        _invForge.items[id - 1].ID = droppedItem.item.ID;
        droppedItem.slot = id - 1;
    }
    void itemDe1en2(ItemData droppedItem, Transform item)
    {
        item.GetComponent<ItemData>().slot = droppedItem.slot;
        _invForge.items[id + 1] = new Item();
        _invForge.items[droppedItem.slot + 1].ID = droppedItem.item.ID;

        item.transform.SetParent(_invForge.slots[droppedItem.slot].transform);
        item.transform.position = _invForge.slots[droppedItem.slot].transform.position + new Vector3(33, 0);


        droppedItem.slot = id;
        droppedItem.transform.SetParent(this.transform);
        droppedItem.transform.position = this.transform.position;

        _invForge.items[droppedItem.slot] = item.GetComponent<ItemData>().item;
        _invForge.items[id] = droppedItem.item;
    }
   
    void itemDe2en1(ItemData droppedItem, Transform item)
    {
        _invForge.items[droppedItem.slot + 1] = new Item();

        _invForge.items[droppedItem.slot] = item.GetComponent<ItemData>().item;
        item.GetComponent<ItemData>().slot = droppedItem.slot;
        item.transform.SetParent(_invForge.slots[droppedItem.slot].transform);
        item.transform.position = _invForge.slots[droppedItem.slot].transform.position;

        droppedItem.slot = id;
        droppedItem.transform.SetParent(this.transform);
        droppedItem.transform.position = this.transform.position;

        _invForge.items[droppedItem.slot] = item.GetComponent<ItemData>().item;
        _invForge.items[id] = droppedItem.item;

        _invForge.items[id + 1].ID = droppedItem.item.ID;
    }
    void itemDe2en2(ItemData droppedItem, Transform item)
    {
        _invForge.items[droppedItem.slot] = new Item();
        _invForge.items[droppedItem.slot + 1] = new Item();

        _invForge.items[droppedItem.slot] = item.GetComponent<ItemData>().item;
        _invForge.items[droppedItem.slot + 1].ID = item.GetComponent<ItemData>().item.ID;
        item.GetComponent<ItemData>().slot = droppedItem.slot;
        item.transform.SetParent(_invForge.slots[droppedItem.slot].transform);
        item.transform.position = _invForge.slots[droppedItem.slot].transform.position + new Vector3(33, 0);

        droppedItem.slot = id;
        droppedItem.transform.SetParent(this.transform);
        droppedItem.transform.position = this.transform.position + new Vector3(33, 0);


        _invForge.items[id] = droppedItem.item;

        _invForge.items[id + 1].ID = droppedItem.item.ID;
    }
   
    public void returnItemToPlace(GameObject item)
    {
        //Debug.Log("entre");
        var data = item.GetComponent<ItemData>();
        if (data.item.SlotsUsed == 2)
        {
            if(data.wasInForge)
            {
                _invForge.items[data.slot] = data.item;
                _invForge.items[data.slot + 1] = data.item;
            }
        }
        else if (data.item.SlotsUsed == 4)
        {
            _invForge.items[data.slot] = data.item;
            _invForge.items[data.slot + 1] = data.item;
            _invForge.items[data.slot + 6] = data.item;
            _invForge.items[data.slot + 7] = data.item;
        }

    }
}


