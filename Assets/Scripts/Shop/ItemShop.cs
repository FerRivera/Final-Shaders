using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Linq;

public class ItemShop : MonoBehaviour , IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public bool available;
    public ItemDatabase database;
    public int id;
    public Item item;
    public Inventory inv;
    public GameObject popUpText;
    public Image img;
    HeroModel _hero;
    public dropItemInv slotToBuy;
    private TooltipShop tooltip;
    
    void Start()
    {
        tooltip = Finder.Instance.canvas.transform.FindChild("Shop").GetComponent<TooltipShop>();
        database = Finder.Instance.itemDatabase;//GameObject.Find("Inventory").GetComponent<ItemDatabase>();
        item = database.FetchItemByID(id);
     
        img = GetComponent<Image>();
       // popUpText = GameObject.Find("PopUpText").GetComponent<Text>();

        inv = Finder.Instance.inventory;//GameObject.Find("Inventory").GetComponent<Inventory>();
        _hero = Finder.Instance.hero;
        GetComponent<Image>().sprite = item.Sprite;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.Activate(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.Deactivate();
    }

    public void OnPointerClick(PointerEventData eventdata)
    {
        
        if (!inv.hasSpaceEnoughSkill() && item.IsSkill || !inv.hasSpaceEnoughItem() && item.IsGem /*|| item.IsConsumable*/)
        {
            //StartCoroutine(ShowMessage("Not enough space!", 2, eventdata));
            var asd = Instantiate(popUpText);
            asd.GetComponent<PopupText>().textToShow ="No more space!";
            asd.transform.position = this.transform.position - new Vector3(1, 0, 0);
            return;
        }
        else if(item.IsSkill && inv.ItemInInv(item))
        {
            //StartCoroutine(ShowMessage("Already one in inventory!", 2, eventdata));
            Debug.Log("asd");
            var asd = Instantiate(popUpText);
            asd.GetComponent<PopupText>().textToShow = "No more space!";
            asd.transform.position = this.transform.position - new Vector3(1, 0, 0);
            return;
        }
        else if (_hero.gold >= item.Value)
        {
            slotToBuy.BuyItemSlot(id);
            //inv.AddItem(item.ID);
            //_hero.gold -= item.Value;
        }
        //else if (_hero.gold >= item.Value && inv.CheckItemAmount() > inv.slotAmount)
       // {
            //Debug.Log("no ai espazio wachin" + inv.CheckItemAmount() + " " + " " + inv.slotAmount);
       // }
        else if (_hero.gold <= item.Value)
        {
            StartCoroutine(ShowMessage("Not enough gold!", 2, eventdata));

        }
        else
        {
            StartCoroutine(ShowMessage("Not enough space available!", 2, eventdata));

        }


    }
 
    IEnumerator ReorgInv()
    {
        yield return new WaitForEndOfFrame();
        inv.ReorganizeGoldInInv();
    }

    IEnumerator ShowMessage(string message, float delay, PointerEventData data)
    {
        popUpText.gameObject.SetActive(true);
        popUpText.transform.position = data.position + new Vector2(150,0);
        //popUpText.text = message;       
        yield return new WaitForSeconds(delay);
        popUpText.gameObject.SetActive(false);
    }

}
