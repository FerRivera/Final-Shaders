using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Linq;
using LitJson;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class InventoryForge : MonoBehaviour
{
    [HideInInspector]
    public ItemDatabase database;
    public GameObject inventoryPanel;
    public Text popUpText;
    public GameObject inventorySlot;
    public GameObject inventoryItem;
    public GameObject tooltip;
    public int slotAmount;
    public int slotAmountSecondaryInv;
    public List<Item> items = new List<Item>();
    public List<GameObject> slots = new List<GameObject>();
    private Inventory inventoryMain;
    private Trader trader;

    public TextMesh textNoMoreInv;

    public HeroModel hero;

    public int numberOfItemsInInv;

    //FORGE SPECIFIC
    [HideInInspector]
    public int idAuthorized;
    public int amountForThisId;
    public Text successRate;
    public float successRateFloat;
    public int idItemResiduo;
    public GameObject ForgeLayout;
    public Button buttonForgeItem;
    public dropItemInv toreset;

    void Start()
    {
        inventoryMain = Finder.Instance.inventory;
        trader = Finder.Instance.trader;
        database = GetComponent<ItemDatabase>();
        foreach (var item in slots)
        {
            items.Add(new Item());
        }
  
    }
    public void QuitShopReset()
    {
        toreset.GetComponent<Image>().sprite = toreset.sprite;
        toreset.id = 0;
        toreset.hasItem = false;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log(CheckItemAmount() + " " + " out of " + " " + slots.Count + " of Inventory Forge") ;

        }
        if(successRateFloat > 100)
        {
            successRateFloat = 100;
        }
        if(successRateFloat < 0)
        {
            successRateFloat = 0;
        }
        successRate.text = successRateFloat.ToString();
        
        //bool checkerino = items.Select(x => x.ID).Where(t => t == idAuthorized).Count() == amountForThisId;
        //bool checkerino = _InventoryForge.items.GroupBy(x => x.ID)
        

        if (successRateFloat > 0)
        {
            //trader.ActualButtonForgeItem.interactable = true; 
            buttonForgeItem.interactable = true;
        }
        else
        {
            //trader.ActualButtonForgeItem.interactable = false;      
            buttonForgeItem.interactable = false;

        }

    }
    public void AddItem(int toSlot, int id)
    {
        Item itemToAdd = database.FetchItemByID(id);
        if (items[toSlot].ID == -1)
        {
            items[toSlot] = itemToAdd;
            if (items[toSlot].SlotsUsed == 1)
            {              
                GameObject itemObj = Instantiate(inventoryItem);
                var itemdata = itemObj.GetComponent<ItemData>();
                itemdata.item = itemToAdd;
                itemdata.amount = 1;
                itemdata.slot = toSlot;
                itemdata.wasInForge = true;
                successRateFloat += itemdata.item.CDTime;
                

                itemObj.transform.SetParent(slots[toSlot].transform);
                itemObj.transform.position = slots[toSlot].transform.position;
                itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
                itemObj.name = itemToAdd.Title;
                AudioManager.instance.PlaySound(SoundsEnum.GENERAL_PICKUP_ITEM);

                //itemObj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);


            }
            else if (items[toSlot].SlotsUsed == 2 )
            {
                stuffItem2ToSlot(itemToAdd, toSlot);
            }
           /* else if (items[toSlot].SlotsUsed == 4)
            {
               // stuffItem4ToSlot(itemToAdd, toSlot);
            }*/
        }
        InvSwitch.instance.rewriteAmount();

    }
    public void AddItem(int id)
    {
        Item itemToAdd = database.FetchItemByID(id);

        for (var i = 0; i < items.Count; i++)
        {
            if (items[i].ID == -1)
            {
                //items[i] = itemToAdd;
                if (itemToAdd.SlotsUsed == 1)
                {
                    items[i] = itemToAdd;
                    GameObject itemObj = Instantiate(inventoryItem);
                    var itemdata = itemObj.GetComponent<ItemData>();
                    itemdata.item = itemToAdd;
                    itemdata.amount = 1;
                    itemdata.slot = i;
                    successRateFloat += itemdata.item.CDTime;

                    itemObj.transform.SetParent(slots[i].transform);
                    itemObj.transform.position = slots[i].transform.position;
                    itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
                    itemObj.name = itemToAdd.Title;
                    //itemObj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    AudioManager.instance.PlaySound(SoundsEnum.GENERAL_PICKUP_ITEM);

                    InvSwitch.instance.rewriteAmount();
                    break;
                }
                if (itemToAdd.SlotsUsed == 2)
                {
                    stuffItem2(itemToAdd, i);
                    CheckItemAmount();
                    InvSwitch.instance.rewriteAmount();
                    break;
                }
              /*  if (itemToAdd.SlotsUsed == 4 (2x2))
                {

                    stuffItem4(itemToAdd, i);
                    CheckItemAmount();
                    break;
                }*/
            }
        }
    }
    void stuffItem2ToSlot(Item itemToAdd, int toSlot)
    {
        //if (items[toSlot].ID == -1 && items[toSlot + 1].ID == -1  && slots[toSlot].GetComponent<Slot>().id != 5 && slots[toSlot].GetComponent<Slot>().id != 11 && slots[toSlot].GetComponent<Slot>().id != 17 && slots[toSlot].GetComponent<Slot>().id != 23)
        //{
        items[toSlot] = itemToAdd;
        GameObject itemObj = Instantiate(inventoryItem);
        var itemdata = itemObj.GetComponent<ItemData>();
        itemdata.item = itemToAdd;
        itemdata.amount = 1;
        itemdata.slot = toSlot;
        itemdata.wasInForge = true;
        successRateFloat += itemdata.item.CDTime;

        itemObj.transform.SetParent(slots[toSlot].transform);
        itemObj.transform.position = slots[toSlot].transform.position + new Vector3(33, 0);
        RectTransform rt = itemObj.GetComponent(typeof(RectTransform)) as RectTransform;
        rt.sizeDelta = new Vector2(115, 56);
        itemObj.transform.SetAsLastSibling();

        itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;

        itemObj.name = itemToAdd.Title;

        items[toSlot + 1].ID = toSlot;

    }
    public void ForgeItem()// 1 equals 1 slots, 2 equals 2 slots
    {
        if(inventoryMain.hasSpaceEnoughItem())
        {
            
            bool result = UnityEngine.Random.Range(0f, 1f) < (successRateFloat/100);
            if (result)
            {
                inventoryMain.AddItem(idAuthorized+1);
                AudioManager.instance.PlaySound(SoundsEnum.FORGE_SUCCESS);
            }
            else
            {
                inventoryMain.AddItem(idItemResiduo);
                AudioManager.instance.PlaySound(SoundsEnum.FORGE_FAILURE);
            }

            for (int i = 0; i <= items.Count - 1; i++)
            {
                if (items[i].ID != -1)
                {
                    RemoveItem(slots[i].GetComponentInChildren<ItemData>());
                }
            }
        }
        //InvSwitch.instance.rewriteAmount();
    }
    public void LockSlots()
    {
        foreach (var slot in slots)
        {
            slot.GetComponent<SlotForge>().isLocked = true;
        }
    }
    public void UnlockSlots()
    {
        foreach (var slot in slots)
        {
            slot.GetComponent<SlotForge>().isLocked = false;
        }
    }
   
    public void RemoveItem(ItemData item)
    {
        if (item.item.SlotsUsed == 1)
        {
            items[item.slot] = new Item();
            successRateFloat -= item.item.CDTime;
        }
        else if (item.item.SlotsUsed == 2)
        {
            items[item.slot] = new Item();
            items[item.slot + 1] = new Item();
            successRateFloat -= item.item.CDTime;

        }

        InvSwitch.instance.rewriteAmount();
        Destroy(item.gameObject);

    }
    public void emptyInventory()
    {
        if(inventoryMain.hasSpaceEnoughItem())
        {

            List<ItemData> listOfItems = slots.Select(x => x.GetComponentInChildren<ItemData>()).Where(t => t != null).Where(y => y.item.ID != -1).ToList();
            if (listOfItems.Count == 0)
            {
                return;
            }
            //successRateFloat = 0f;
            for (int i = 0; i < listOfItems.Count; i++)
            {
                RemoveItem(listOfItems[i]);
            }
            //InvSwitch.instance.rewriteAmount();
            AudioManager.instance.PlaySound(SoundsEnum.FORGE_ONCLICK_CLEAR);
            TransferItemsToInventory(listOfItems);
        }
    }
    public bool isInventoryEmpty()
    {
        for (int i = 0; i < items.Count; i++)
        {
            if(items[i].ID != -1)
            {
                return false;
            }

        }
        return true;
    }
    public void TransferItemsToInventory(List<ItemData> listOfItems)
    {
        for (int i = 0; i < listOfItems.Count; i++)
        {
            inventoryMain.AddItem(listOfItems[i].item.ID);
        }
    }
 

    void stuffItem2(Item itemToAdd, int i)
    {
        for (int j = i; j < items.Count; j++)
        {


            if (items[j].ID == -1 && items[j + 1].ID == -1 && !slots[j].GetComponent<Slot>().isSkill && slots[j].GetComponent<Slot>().id != 5 && slots[j].GetComponent<Slot>().id != 11 && slots[j].GetComponent<Slot>().id != 17 && slots[j].GetComponent<Slot>().id != 23)
            {
                items[j] = itemToAdd;
                GameObject itemObj = Instantiate(inventoryItem);
                var itemdata = itemObj.GetComponent<ItemData>();
                itemdata.item = itemToAdd;
                itemdata.amount = 1;
                itemdata.slot = j;


                itemObj.transform.SetParent(slots[j].transform);
                itemObj.transform.position = slots[j].transform.position + new Vector3(33, 0);
                RectTransform rt = itemObj.GetComponent(typeof(RectTransform)) as RectTransform;
                rt.sizeDelta = new Vector2(115, 56);
                itemObj.transform.SetAsLastSibling();

                itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;

                itemObj.name = itemToAdd.Title;

                items[j + 1].ID = j;
                InvSwitch.instance.rewriteAmount();
                break;
            }
            else
            {
                int temp = j + 1;
                if (temp >= slots.Count - 1)
                {               
                    StartCoroutine(ShowMessage(popUpText, "Not enough space available!", 2, inventoryPanel.transform));
                    return;
                }
                continue;
            }
        }

    }
    
   



    
   
    public IEnumerator ShowMessage(Text text, string message, float delay, Transform data)
    {
        text.gameObject.SetActive(true);
        text.transform.position = data.transform.position + new Vector3(150, 0);
        text.text = message;
        yield return new WaitForSeconds(delay);
        text.gameObject.SetActive(false);
    }
    public bool spaceEnoughForItem(int numberSlots)
    {
        int h;
        if (numberSlots == 1)
        {
            h = 0;
        }
        else if (numberSlots == 2)
        {
            h = 1;
        }
        else if (numberSlots == 4)
        {
            h = 7;
        }
        else
        {
            throw new Exception("Wrong number of slots to use");
        }
        for (int j = 0; j < items.Count; j++)
        {
            int temp = j + h;
            if (h == 0)
            {
                if (items[temp].ID == -1 && !slots[temp].GetComponent<Slot>().isSkill)
                {
                    return true;
                }
            }
            else if (h == 1)
            {
                if (items[temp].ID == -1 && !slots[temp].GetComponent<Slot>().isSkill && items[temp - 1].ID == -1 && !slots[temp - 1].GetComponent<Slot>().isSkill)
                {
                    return true;
                }
            }
            else if (h == 7)
            {
                if (items[temp].ID == -1 && !slots[temp].GetComponent<Slot>().isSkill && items[temp - 1].ID == -1 && !slots[temp - 1].GetComponent<Slot>().isSkill && items[j].ID == -1 && !slots[j].GetComponent<Slot>().isSkill && items[j + 1].ID == -1 && !slots[j + 1].GetComponent<Slot>().isSkill)
                {
                    return true;
                }
            }

            if (temp >= slots.Count - 1)
            {
                return false;
            }
        }
        return true;
    }


    //chequea si el item esta en el inv
    public bool ItemInInv(Item item)
    {
        for (int i = 0; i < items.Count; i++)
            if (items[i].ID == item.ID)
                return true;
        return false;
    }
    public int CheckItemAmount()
    {
        numberOfItemsInInv = 0;
        foreach (var number in items)
        {
            if (number.ID != -1)
            {

                numberOfItemsInInv++;
            }
        }
       
        return numberOfItemsInInv;
    }

    public void Close()
    {
        if (this.gameObject.activeSelf)
        {
            CursorPointer.instance.InventoryClosed();
            CursorPointer.instance.ChangeToDefault();
            inventoryPanel.SetActive(false);
            tooltip.SetActive(false);
            //Hero.inputBlock = false;
            pauseGame.screens.Pop();
        }
    }

}


