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

[Serializable]
public class Inventory : MonoBehaviour 
{
    [HideInInspector]
    public ItemDatabase database;
    public GameObject inventoryPanel;
    public GameObject slotPanel;
    public GameObject slotPanel2;
    [HideInInspector]
    public GameObject slotPanelSecondary;
    public Text popUpText;
    public GameObject inventorySlot;
    public GameObject inventoryItem;
    public GameObject tooltip;
    public GameObject shop;
    public int slotAmount;
    public int slotAmountSecondaryInv;
    public List<Item> items = new List<Item>();
    public List<GameObject> slots = new List<GameObject>();
    public List<GameObject> slotsGear = new List<GameObject>();
    public GameObject q;
    public GameObject e;
    public GameObject r;
    public GameObject f;
    public List<Trader> traders = new List<Trader>();

    public Text damage;
    public Text gold;
    public Text health;
    public Text defence;
    public Text stamina;
    public Text staminaRegen;
    public TextMesh textNoMoreInv;
    
    public HeroModel hero;

    public int numberOfItemsInInv;
    public int numberOfItemsInInvSecondary;

    private Queue<int> reloadInv = new Queue<int>();
    public JsonData JCD;
    public GameObject iconInventory;
    public GameObject iconSkillsInventory;
    public HeroStaminaRegen stamRest;
    public bool isActive;
    void Start()
    {
        database = GetComponent<ItemDatabase>();
        stamRest = hero.GetComponent<HeroStaminaRegen>();

        //slotAmount = 24;
        inventoryPanel = Finder.Instance.inventoryPanel;
        inventoryPanel.SetActive(false);
        slotPanel.SetActive(false);
        //slotPanel = inventoryPanel.transform.FindChild("Slot Panel").gameObject;
        //slotPanel = inventoryPanel.transform.FindChild("Slot Panel").gameObject.transform.FindChild("Slot Panel Child").gameObject;
        slotPanelSecondary = inventoryPanel.transform.FindChild("Slot Panel Secondary").gameObject.transform.FindChild("Slot Panel Child Secondary").gameObject;

        for (int i = 0; i < slotAmount; i++)
        {
            items.Add(new Item());
            //slots.Add(Instantiate(inventorySlot));
            slots[i].GetComponent<Slot>().id = i;
            //slots[i].transform.SetParent(slotPanel.transform);
        }
        //for (int i = slotAmount +3; i < slotAmountSecondaryInv+slotAmount+3; i++)
        for (int i = slotAmount; i < slotAmountSecondaryInv + slotAmount; i++)
        {
            items.Add(new Item());
            //slots.Add(Instantiate(inventorySlot));
            slots[i].GetComponent<Slot>().id = i;
            //slots[i].transform.SetParent(slotPanelSecondary.transform,false);
        }
        items.Add(new Item());
        items.Add(new Item());
        items.Add(new Item());
        items.Add(new Item());

        q.GetComponent<Slot>().id = slotAmountSecondaryInv + slotAmount;
        e.GetComponent<Slot>().id = slotAmountSecondaryInv + slotAmount + 1;
        r.GetComponent<Slot>().id = slotAmountSecondaryInv + slotAmount + 2;
        f.GetComponent<Slot>().id = slotAmountSecondaryInv + slotAmount + 3;

        q.transform.SetParent(slotPanel2.transform,false);
        e.transform.SetParent(slotPanel2.transform,false);
        r.transform.SetParent(slotPanel2.transform,false);
        f.transform.SetParent(slotPanel2.transform,false);

        slots.Add(q);
        slots.Add(e);
        slots.Add(r);
        slots.Add(f);

        for (int i = 0; i < slotsGear.Count; i++)
        {
            items.Add(new Item());
            slots.Add(slotsGear[i]);
            slots[slotAmount + slotAmountSecondaryInv + 4 + i].GetComponent<Slot>().id = slotAmount + slotAmountSecondaryInv + 4 + i;
        }
        LoadInventory();

        if (reloadInv.Count > 0)
        {
            while (reloadInv.Count > 0)
            {
                AddItem(reloadInv.Dequeue(), reloadInv.Dequeue());
            }
        }

        //AddItem(5);
        //Pociones Healt
        AddItem(8);
        AddItem(8);
        AddItem(8);
        AddItem(8);
        AddItem(8);
        AddItem(27);

        //Skills
        //AddItem(0);
       // AddItem(4);
        
        //AddItem(4);
        //AddItem(5);
        //AddItem(25);
        //AddItem(26);
        //AddItem(27);
        //AddItem(29);
        //AddItem(28);

        


    }
    void OnApplicationQuit()
    {
        if (File.Exists(Application.dataPath + "/InvSave.json"))
        {
            File.Delete(Application.dataPath + "/InvSave.json");
            //Debug.Log("DELETEADO");
        }       
        
    }
    public void LoadInventory()
    {
        if(!File.Exists(Application.dataPath + "/InvSave.json"))
        {
            return;
        }
        string fromJson = File.ReadAllText(Application.dataPath + "/InvSave.json");
        if(fromJson.Length < 4)
        {
            print("Nothing saved!");
            return;
        }
        List<int> temp = GetNumbers(fromJson).Select(c => int.Parse(c)).ToList();

        foreach (var item in temp)
        {
            reloadInv.Enqueue(item);
        }

    }
    private List<string> GetNumbers(string input)
    {
        string x = input.Replace("[", string.Empty).Replace("]", string.Empty);
        return x.Split(',').ToList();

    }
    public int CountItems(int id)
    {
        var count = 0;
        foreach (var item in items)
        {
            if(item.ID == id)
            {
                count++;
            }
        }
        return count;
    }
    public void SaveInventory()
    {
        List<int> listaLugarSlot = new List<int>();  
        foreach (var item in slots)
        {
            if(item.GetComponent<Slot>().transform.childCount > 0 && item.GetComponent<Slot>() != null && !item.GetComponent<Slot>().isSkill)
            {
                listaLugarSlot.Add(item.GetComponent<Slot>().id);
                listaLugarSlot.Add(item.GetComponent<Slot>().transform.GetComponentInChildren<ItemData>().item.ID);
            }
            
        }

        JCD = JsonMapper.ToJson(listaLugarSlot);       
        File.WriteAllText(Application.dataPath + "/InvSave.json", JCD.ToString());

    }
    void Update()
    {
       // if(Input.GetKeyDown(KeyCode.H))
        //{
       //     AddItem(20);
       // }

        if (!pauseGame.paused)
        {
            damage.text = hero.damageFirstHit.ToString();
            health.text = hero.maxHealth.ToString();
            defence.text = hero.defence.ToString();
            gold.text = hero.gold.ToString();
            stamina.text = hero.maxStamina.ToString();
            staminaRegen.text = stamRest.staminaRegen.ToString();


           /* if (Input.GetKeyDown(KeyCode.T))
            {
                Debug.Log(CheckItemAmount() + " " + " " + slotAmount + spaceEnoughForItem(2));
               
               
            }*/
            if(Input.GetKeyDown(KeyCode.C))
            {
                if (!slotPanel.activeSelf)
                {
                    //CursorPointer.instance.ChangeToInventory();
                    AudioManager.instance.PlaySound(SoundsEnum.INVENTORY_SKILLS_OPEN);
                    slotPanel.SetActive(true);
                    iconSkillsInventory.SetActive(true);
                    pauseGame.screens.Push(this.gameObject);
                    pauseGame.pauseGameInstance.Pause();


                }
                else
                {
                    if(!inventoryPanel.activeSelf)
                    {
                        pauseGame.pauseGameInstance.UnPause();
                       // CursorPointer.instance.InventoryClosed();
                        //CursorPointer.instance.ChangeToDefault();
                    }
                    AudioManager.instance.PlaySound(SoundsEnum.INVENTORY_SKILLS_CLOSE);
                    slotPanel.SetActive(false);
                    iconSkillsInventory.SetActive(false);
                    tooltip.SetActive(false);

                    pauseGame.screens.Pop();
                }
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                if (!inventoryPanel.activeSelf)
                {
                    //CursorPointer.instance.ChangeToInventory();
                    AudioManager.instance.PlaySound(SoundsEnum.INVENTORY_OPEN);

                    inventoryPanel.SetActive(true);
                    isActive = true;
                    iconInventory.SetActive(true);
                    pauseGame.screens.Push(this.gameObject);
                    pauseGame.pauseGameInstance.Pause();
                  
                }
                else
                {
                    if (!slotPanel.activeSelf)
                    {
                        pauseGame.pauseGameInstance.UnPause();
                       // CursorPointer.instance.InventoryClosed();
                       // CursorPointer.instance.ChangeToDefault();
                    }           
                    AudioManager.instance.PlaySound(SoundsEnum.INVENTORY_CLOSE);
                    inventoryPanel.SetActive(false);
                    iconInventory.SetActive(false);
                    isActive = false;
                    tooltip.SetActive(false);

                    pauseGame.screens.Pop();
                   if (shop.activeSelf)
                    {
                        foreach (var item in traders)
                        {
                            if (item.GetComponent<Trader>().isActive)
                            {
                                item.GetComponent<Trader>().closeThis();
                                return;
                            }
                        }
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if(InvSwitch.instance.shopHelp.activeSelf || InvSwitch.instance.forgeHelp.activeSelf)
                {
                    return;
                }
                if (inventoryPanel.activeSelf)
                {
                    //CursorPointer.instance.InventoryClosed();
                    // CursorPointer.instance.ChangeToDefault();
                    AudioManager.instance.PlaySound(SoundsEnum.INVENTORY_CLOSE);
                    inventoryPanel.SetActive(false);
                    tooltip.SetActive(false);
                    iconInventory.SetActive(false);
                    pauseGame.screens.Pop();
                    pauseGame.pauseGameInstance.UnPause();
                    isActive = false;

                }
                if (slotPanel.activeSelf)
                {
                    //  CursorPointer.instance.InventoryClosed();
                    // CursorPointer.instance.ChangeToDefault();
                    AudioManager.instance.PlaySound(SoundsEnum.INVENTORY_SKILLS_CLOSE);
                    slotPanel.SetActive(false);
                    tooltip.SetActive(false);
                    iconSkillsInventory.SetActive(false);
                    pauseGame.screens.Pop();
                    pauseGame.pauseGameInstance.UnPause();

                }
                /*if(shop.activeSelf )
                {
                    foreach (var item in traders)
                    {
                        if(item.gameObject.activeSelf)
                        {
                            item.GetComponent<Trader>().closeThis();
                        }
                    }
                }*/

            }
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
                if (itemToAdd.Slug == "Coins")
                {
                    hero.gold += itemToAdd.Value;
                }
                GameObject itemObj = Instantiate(inventoryItem);
                var itemdata = itemObj.GetComponent<ItemData>();
                itemdata.item = itemToAdd;
                itemdata.amount = 1;
                itemdata.slot = toSlot;

                itemObj.transform.SetParent(slots[toSlot].transform,false);
                itemObj.transform.position = slots[toSlot].transform.position;
                itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
                itemObj.name = itemToAdd.Title;
               // itemObj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); 
                                          
            }
            else if (items[toSlot].SlotsUsed == 2)
            {
                stuffItem2ToSlot(itemToAdd, toSlot);                
            }
            else if (items[toSlot].SlotsUsed == 4)
            {
                stuffItem4ToSlot(itemToAdd, toSlot);
            }
        }
        InvSwitch.instance.rewriteAmount();

    }
    public void AddItemToMiniInv(int id, GameObject toSlot)
    {
        Item itemToAdd = database.FetchItemByID(id);
        if(toSlot.transform.childCount > 0)
        {
            foreach (Transform child in toSlot.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
        GameObject itemObj = Instantiate(inventoryItem);
        if (itemToAdd.SlotsUsed == 2)
        {
            RectTransform rt = itemObj.GetComponent(typeof(RectTransform)) as RectTransform;
            rt.sizeDelta = new Vector2(115, 56);
        }
        else if(itemToAdd.SlotsUsed == 4)
        {
            RectTransform rt = itemObj.GetComponent(typeof(RectTransform)) as RectTransform;
            rt.sizeDelta = new Vector2(120 , 120);
        }


        
        var itemdata = itemObj.GetComponent<ItemData>();
       
        itemdata.item = itemToAdd;
        itemdata.item.Picked = true;
        //itemdata.amount = 1;
        //itemdata.slot = toSlot;


        itemObj.transform.SetParent(toSlot.transform);
        itemObj.transform.position = toSlot.transform.position;
        itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
        itemObj.name = itemToAdd.Title;


    }
    public void AddItem(int id)
    {
        Item itemToAdd = database.FetchItemByID(id);
        if (itemToAdd.Stackable && ItemInInv(itemToAdd))
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].ID == id)
                {

                    if(itemToAdd.Slug == "Coins")
                    {
                        hero.gold += itemToAdd.Value;                      
                    }

                    //ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
                    ItemData data = slots[i].transform.GetComponentInChildren<ItemData>();
                    data.amount++;
                    data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();

                   /* if (!itemToAdd.IsSkill)
                    {
                        hero.defence += itemToAdd.Defence;
                        hero.health += itemToAdd.Health;
                        hero.damage += itemToAdd.Strength;
                    }
                    CheckItemAmount();*/
                    break;
                }
                  
            }
        }
        else
        {          
            for (var i = startIndex(itemToAdd.IsSkill); i < items.Count; i++)
            {
                if (items[i].ID == -1)
                {
                    //items[i] = itemToAdd;
                    if (itemToAdd.SlotsUsed == 1)
                    {
                        if (itemToAdd.Slug == "Coins")
                        {
                            hero.gold += itemToAdd.Value;
                            var coinbag = slots.Select(x => x.GetComponentInChildren<ItemData>()).Where(x => x != null && x.item.Slug == "CoinBag").FirstOrDefault();
                            if (coinbag != null)
                            {
                                coinbag.amount++;
                                coinbag.transform.GetChild(0).GetComponent<Text>().text = coinbag.amount.ToString();
                                return;
                            }
                        }
                      
                        items[i] = itemToAdd;
                        GameObject itemObj = Instantiate(inventoryItem);
                        var itemdata = itemObj.GetComponent<ItemData>();
                        itemdata.item = itemToAdd;                     
                        itemdata.amount = 1;                       
                        itemdata.slot = i;


                        itemObj.transform.SetParent(slots[i].transform,false);
                        itemObj.transform.position = slots[i].transform.position; 
                        itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
                        itemObj.name = itemToAdd.Title;
                        //itemObj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);



                        /* if (!itemToAdd.IsSkill)
                         {
                             hero.defence += itemToAdd.Defence;
                             hero.health += itemToAdd.Health;
                             hero.damage += itemToAdd.Strength;
                         }
                         CheckItemAmount();*/
                        break;
                    }
                    if(itemToAdd.SlotsUsed == 2)
                    {                       
                        stuffItem2(itemToAdd, i);
                        CheckItemAmount();
                        break;
                    }
                    if (itemToAdd.SlotsUsed ==4 /*(2x2)*/)
                    {
                     
                        stuffItem4(itemToAdd, i);
                        CheckItemAmount();
                        break;
                    }


                }
            }
        }
        InvSwitch.instance.rewriteAmount();


    }
    private int startIndex(bool check)
    {
        /*if (check.IsSkill && check.IsConsumable || !check.IsSkill && !check.IsConsumable)
        {
            return slotAmount;
        }
        else if (check.IsSkill && !check.IsConsumable)
        {
            return 0;
        }
        else
            return 0;*/
        return check ? 0 : slotAmount;
    }
    public void RemoveItemByID(int id, int quantity)
    {
        var items = slots.Select(x => x.GetComponentInChildren<ItemData>()).Where(x => x != null && x.item.ID == id).Take(quantity);
        foreach (var coin in items)
        {
            RemoveItem(coin);
        }
       // RemoveItem(SearchItemInInv(id));
    }
    public ItemData SearchItemInInv(int id)
    {
        foreach (var slot in slots)
        {
            if(slot.GetComponentInChildren<ItemData>() != null && slot.GetComponentInChildren<ItemData>().item.ID == id)
            {
                return slot.GetComponentInChildren<ItemData>();
            }
        }
        return null;
    }
    public void RemoveItem(ItemData item)
    {
        if (item.item.Slug == "Coins")
        {
            //hero.gold -= item.amount;
        }
        if(item.item.SlotsUsed == 1)
        {
            items[item.slot] = new Item();
        }
        else if (item.item.SlotsUsed == 2)
        {
            items[item.slot] = new Item();
            items[item.slot+1] = new Item();
        }
        else if (item.item.SlotsUsed == 4)
        {
            items[item.slot] = new Item();
            items[item.slot + 1] = new Item();
            items[item.slot + 6] = new Item();
            items[item.slot + 7] = new Item();
        }
       /* if (!item.item.IsSkill)
        {
            hero.defence -= item.item.Defence;
            hero.maxHealth -= item.item.Health;
            hero.damage -= item.item.Strength;
        }*/
        
        Destroy(item.gameObject);
        CheckItemAmount();
        InvSwitch.instance.rewriteAmount();

    }
    public void RemoveItemFromSlot(int slotID,int itemID, ItemData itemData)
    {
        var tempItem = database.FetchItemByID(itemID);
        if (tempItem.SlotsUsed == 1)
        {
            items[slotID] = new Item();
        }
       /* else if (tempItem.SlotsUsed == 2)
        {
            items[item.slot] = new Item();
            items[item.slot + 1] = new Item();
        }
        else if (tempItem.SlotsUsed == 4)
        {
            items[item.slot] = new Item();
            items[item.slot + 1] = new Item();
            items[item.slot + 6] = new Item();
            items[item.slot + 7] = new Item();
        }*/
        if (!tempItem.IsSkill)
        {
            /*hero.defence -= tempItem.Defence;
            hero.maxHealth -= tempItem.Health;
            hero.damage -= tempItem.Strength;*/
        }
        Destroy(itemData.gameObject);
        InvSwitch.instance.rewriteAmount();

    }
  
    public void ReorganizeGoldInInv()
    {
        //IA2-P1

        //List<Item> listOfCoins = items.Where(x => x.Slug == "Coins").ToList();
        List<ItemData> listOfCoins = slots.Select(x => x.GetComponentInChildren<ItemData>()).Where(x => x != null && x.item.Slug == "Coins").ToList();      
        var coinbag = slots.Select(x => x.GetComponentInChildren<ItemData>()).Where(x => x != null && x.item.Slug == "CoinBag").FirstOrDefault();
        coinbag.amount += listOfCoins.Count();      
        RemoveGoldItem(listOfCoins.Count(), false);     
        coinbag.transform.GetChild(0).GetComponent<Text>().text = coinbag.amount.ToString();

    }
    public void RemoveGoldItem(int qty, bool stack)
    {
        //IA2-P1
        if(stack)
        {
            var coins = slots.Select(x => x.GetComponentInChildren<ItemData>()).Where(x => x != null && x.item.Slug == "CoinBag").FirstOrDefault();
           
            coins.amount -= qty;
            coins.transform.GetChild(0).GetComponent<Text>().text = coins.amount.ToString();
        }
        else
        {
            var coins = slots.Select(x => x.GetComponentInChildren<ItemData>()).Where(x => x != null && x.item.Slug == "Coins").Take(qty);
            foreach (var coin in coins)
            {
                RemoveItem(coin);
            }
        }
        InvSwitch.instance.rewriteAmount();


    }
    void stuffItem2(Item itemToAdd, int i)
    {
        for (int j =i; j < items.Count; j++)
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
                /*if (!itemToAdd.IsSkill)
                {
                    hero.defence += itemToAdd.Defence;
                    hero.health += itemToAdd.Health;
                    hero.damage += itemToAdd.Strength;
                }*/
                break;
            }
            else
            {
                int temp = j + 1;           
                if ( temp>= slots.Count-1)
                {
                    if(shop.activeSelf)
                    {
                        hero.gold += itemToAdd.Value;
                    }
                    StartCoroutine(ShowMessage(popUpText,"Not enough space available!", 2, inventoryPanel.transform));
                    return;
                }
                continue;
            }
        }
            
    }
    void stuffItem2ToSlot(Item itemToAdd, int toSlot)
    {     
            if (items[toSlot].ID == -1 && items[toSlot + 1].ID == -1 && !slots[toSlot].GetComponent<Slot>().isSkill && slots[toSlot].GetComponent<Slot>().id != 5 && slots[toSlot].GetComponent<Slot>().id != 11 && slots[toSlot].GetComponent<Slot>().id != 17 && slots[toSlot].GetComponent<Slot>().id != 23)
            {
                items[toSlot] = itemToAdd;
                GameObject itemObj = Instantiate(inventoryItem);
                var itemdata = itemObj.GetComponent<ItemData>();
                itemdata.item = itemToAdd;
                itemdata.amount = 1;
                itemdata.slot = toSlot;


                itemObj.transform.SetParent(slots[toSlot].transform);
                itemObj.transform.position = slots[toSlot].transform.position + new Vector3(33, 0);
                RectTransform rt = itemObj.GetComponent(typeof(RectTransform)) as RectTransform;
                rt.sizeDelta = new Vector2(115, 56);
                itemObj.transform.SetAsLastSibling();

                itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;

                itemObj.name = itemToAdd.Title;

                items[toSlot + 1].ID = toSlot;
                /*if (!itemToAdd.IsSkill)
                {
                    hero.defence += itemToAdd.Defence;
                    hero.health += itemToAdd.Health;
                    hero.damage += itemToAdd.Strength;
                }*/

        }
            else
            {
                int temp = toSlot + 1;
                if (temp >= slots.Count - 1)
                {
                    if (shop.activeSelf)
                    {
                        hero.gold += itemToAdd.Value;
                    }
                    StartCoroutine(ShowMessage(popUpText, "Not enough space available!", 2, inventoryPanel.transform));
                    return;
                }
               
            }
        

    }
    void stuffItem4(Item itemToAdd, int i)
    {
        for (int j = i; j < items.Count; j++)
        {
            if(items[j].ID == -1 && items[j+1].ID == -1 && items[j+6].ID == -1 && items[j + 7].ID == -1 && !slots[j].GetComponent<Slot>().isSkill && !slots[j+6].GetComponent<Slot>().isSkill && !slots[j+7].GetComponent<Slot>().isSkill)
                /*&& slots[j].GetComponent<Slot>().id != 5 && slots[j].GetComponent<Slot>().id != 11 && slots[j].GetComponent<Slot>().id != 17 && slots[j].GetComponent<Slot>().id != 23*/
            {
                for (int a = 5; a < slotAmount + slotAmountSecondaryInv; a += 6)
                {
                    if (slots[j].GetComponent<Slot>().id == a)
                    {                     
                        return;
                    }
                    else if (a > slots[j].GetComponent<Slot>().id)
                        break;
                }
                items[j] = itemToAdd;
                GameObject itemObj = Instantiate(inventoryItem);
                var itemdata = itemObj.GetComponent<ItemData>();
                itemdata.item = itemToAdd;
                if (itemToAdd.Slug == "CoinBag")
                {
                    itemdata.amount = 0;
                }
                else
                {
                    itemdata.amount = 1;
                }
               
                itemdata.slot = j;


                itemObj.transform.SetParent(slots[j].transform);
                itemObj.transform.position = slots[j].transform.position + new Vector3(33, -30);
                RectTransform rt = itemObj.GetComponent(typeof(RectTransform)) as RectTransform;
                rt.sizeDelta = new Vector2(120, 120);


                itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;

                itemObj.name = itemToAdd.Title;

                items[j + 1].ID = j;
                items[j + 6].ID = j;
                items[j + 7].ID = j;
                /*if (!itemToAdd.IsSkill)
                {
                    hero.defence += itemToAdd.Defence;
                    hero.health += itemToAdd.Health;
                    hero.damage += itemToAdd.Strength;
                }*/
                break;
                
            }
            else
            {
                int temp = j + 7;
                if (temp >= slots.Count - 1)
                {
                    if(shop.activeSelf)
                    {
                        hero.gold += itemToAdd.Value;
                    }
                  
                    StartCoroutine(ShowMessage(popUpText,"Not enough space available!", 2, inventoryPanel.transform));
                    return;
                }
                continue;
            }
        }

        

    }
    void stuffItem4ToSlot(Item itemToAdd, int toSlot)
    {
                items[toSlot] = itemToAdd;
                GameObject itemObj = Instantiate(inventoryItem);
                var itemdata = itemObj.GetComponent<ItemData>();
                itemdata.item = itemToAdd;
                if (itemToAdd.Slug == "CoinBag")
                {
                    itemdata.amount = 0;
                }
                else
                {
                    itemdata.amount = 1;
                }

                itemdata.slot = toSlot;


                itemObj.transform.SetParent(slots[toSlot].transform);
                itemObj.transform.position = slots[toSlot].transform.position + new Vector3(33, -30);
                RectTransform rt = itemObj.GetComponent(typeof(RectTransform)) as RectTransform;
                rt.sizeDelta = new Vector2(120, 120);


                itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;

                itemObj.name = itemToAdd.Title;

                items[toSlot + 1].ID = toSlot;
                items[toSlot + 6].ID = toSlot;
                items[toSlot + 7].ID = toSlot;
            /*if (!itemToAdd.IsSkill)
            {
                hero.defence += itemToAdd.Defence;
                hero.health += itemToAdd.Health;
                hero.damage += itemToAdd.Strength;
            }*/
    }
    public IEnumerator ShowMessage(Text text,string message, float delay, Transform data)
    {
        text.gameObject.SetActive(true);
        text.transform.position = data.transform.position + new Vector3(150, 0);
        text.text = message;
        yield return new WaitForSeconds(delay);
        text.gameObject.SetActive(false);
    }
    /*public bool hasFreeSpaceForItems(int numberSlots)
    {
        for (int i = slotAmount; i < slotAmountSecondaryInv-1; i++)
        {
            return false;
        }
        return false;
    }*/
    public bool hasSpaceEnoughSkill()
    {
        for (int i = 0; i < slotAmount; i++)
        {
            if(items[i].ID == -1)
            {
                return true;
            }
        }
        return false;
    }
    public bool hasSpaceEnoughItem()
    {
        for (int i = slotAmount; i < slotAmountSecondaryInv + slotAmount; i++)
        {
            if (items[i].ID == -1)
            {
                return true;
            }
        }
        return false;
    }
    public bool spaceEnoughForItem(int numberSlots)
    {
        int h;
        if(numberSlots == 1)
        {
            h = 0;
        }
        else if(numberSlots == 2)
        {
            h = 1;
        }
        else if(numberSlots == 4)
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
            if(h == 0)
            {
                if(items[temp].ID == -1 && !slots[temp].GetComponent<Slot>().isSkill)
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
            else if (h==7)
            {
                if (items[temp].ID == -1 && !slots[temp].GetComponent<Slot>().isSkill && items[temp-1].ID == -1 && !slots[temp-1].GetComponent<Slot>().isSkill && items[j].ID == -1 && !slots[j].GetComponent<Slot>().isSkill && items[j+1].ID == -1 && !slots[j+1].GetComponent<Slot>().isSkill)
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
        for (int i = 0; i < items.Count ; i++)
            if(items[i].ID == item.ID)
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
        for (int j = 0; j < items.Count; j++)
        {
            if(slots[j].GetComponent<Slot>().isSkill && slots[j].GetComponentInChildren<ItemData>() != null)
            {
                numberOfItemsInInv--;
            }
        }
        for (int i = slotAmount +3 ; i < items.Count; i++)
        {
            if(items[i].ID != -1)
            {
                numberOfItemsInInvSecondary++;
            }
        }
        return numberOfItemsInInv;
    }
    public int itemInvCount()
    {
        int nOfItems = 0;
        if (slots.Count != 0 && items.Count > 0)
        {
            for (int i = slotAmount; i < slotAmountSecondaryInv; i++)
            {
                if (slots[i].GetComponent<Slot>().id == -1)
                {
                    continue;
                }
                if (items[i].SlotsUsed == 1)
                {
                    nOfItems++;
                }//asd
                else if (items[i].SlotsUsed == 2)
                {
                    nOfItems++;
                    i += 1;
                }
                else if (items[i].SlotsUsed == 4)
                {
                    nOfItems++;
                    i += 3;
                }
            }
        }

        return nOfItems;

    }
    public int skillInvCount()
    {
        int nOfItems = 0;
        if(slots.Count != 0 && items.Count > 0)
        {
            for (int i = 0; i < slotAmount-1; i++)
            {
                if (slots[i].GetComponent<Slot>().id == -1)
                {
                    continue;
                }
                if (items[i].SlotsUsed == 1)
                {
                    nOfItems++;
                }
                else if (items[i].SlotsUsed == 2)
                {
                    nOfItems++;
                    i += 1;
                }
                else if (items[i].SlotsUsed == 4)
                {
                    nOfItems++;
                    i += 3;
                }
            }
        }
        return nOfItems;
    }
    public void Close()
    {
        if(this.gameObject.activeSelf)
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


