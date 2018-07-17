using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.UI;

public class dropItemInv : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    public bool isTrash;
    public bool isDrop;
    public bool isShopSell;
    public bool isShopBuy;
    public GameObject droppedItem;
    Inventory _inv;
    HeroModel _hero;
    public GameObject trashit;
    public GameObject canceltrash;
    ItemData data;
    public int id;
    public Sprite sprite;
    public Text amountText;
    public int actualAmount;
    public bool isStack;
 
    public bool hasItem;

    void Start()
    {
        _inv = Finder.Instance.inventory;
        _hero = Finder.Instance.hero;
    }
    void Update()
    {
        if (amountText != null && isStack)
        {
            amountText.text = actualAmount.ToString();
        }
        if (amountText != null && isStack && actualAmount == 0)
        {
            amountText.text = "";
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            actualAmount = 0;
            GetComponent<Image>().sprite = sprite;
            hasItem = false;
            InvSwitch.instance.amountToSell = 0;
            InvSwitch.instance.amountToBuy = 0;
        }
       
    }
    public void OnDrop(PointerEventData eventData)
    {

        if (eventData.pointerDrag.GetComponent<ItemData>() != null)
        {
            AudioManager.instance.PlaySound(SoundsEnum.GENERAL_PICKUP_ITEM);
            if (isTrash)
            {
                data = eventData.pointerDrag.GetComponent<ItemData>();
                trashit.SetActive(true);
                canceltrash.SetActive(true);

            }
            
            else if (isDrop)
            {


                var itemDropped = eventData.pointerDrag.GetComponent<ItemData>();
                if (itemDropped.item.Slug == "CoinBag")
                {
                    _hero.gold -= itemDropped.amount;
                    _inv.RemoveItem(eventData.pointerDrag.GetComponent<ItemData>());
                    var drop2 = Instantiate(droppedItem);
                    drop2.GetComponent<PickupItem>().myItem = itemDropped.item.ID;
                    drop2.transform.position = _hero.transform.position - new Vector3(1, 0, 0);
                    StartCoroutine(CoinBagDrop(itemDropped.amount));
                    //for (int i = 0; i < itemDropped.amount; i++)
                    //{
                    //var dropGold = Instantiate(droppedItem);
                    //dropGold.GetComponent<PickupItem0>().myItem = 8;
                    //dropGold.transform.position = hero.transform.position /*+ new Vector3(i-0.8f,0,i-0.8f)*/;

                    //}
                    return;

                }
                else if (itemDropped.item.Slug == "Coins")
                {
                    _hero.gold -= 1;

                }
                _inv.RemoveItem(eventData.pointerDrag.GetComponent<ItemData>());
                var drop = Instantiate(droppedItem);
                drop.GetComponent<PickupItem>().myItem = itemDropped.item.ID;
                drop.transform.position = _hero.transform.position;
            }
            else if (isShopSell && !hasItem)
            {
                data = eventData.pointerDrag.GetComponent<ItemData>();
                if (data.item.Stackable)
                {
                    isStack = true;
                    actualAmount = data.amount;
                    amountText.text = data.amount.ToString();
                }
                id = data.item.ID;
                //_hero.gold += data.item.Value;
                GetComponent<Image>().sprite = data.item.Sprite;
                _inv.RemoveItem(data);
                hasItem = true;
            }
            else if(isShopSell && hasItem)
            {
                data = eventData.pointerDrag.GetComponent<ItemData>();
                if (data.item.Stackable)
                {
                    isStack = true;
                    actualAmount = data.amount;                
                    amountText.text = data.amount.ToString();
                }
                _inv.RemoveItem(data);
                _inv.AddItem(id);
                id = data.item.ID;
                //_hero.gold += data.item.Value;
                GetComponent<Image>().sprite = data.item.Sprite;
            }           
        } 
        
    }
    //BUY FUNCIONANDO
    /*public void Buy()
    {
        if(_hero.gold >= _inv.database.FetchItemByID(id).Value && hasItem && _inv.hasSpaceEnoughItem())
        {
            var item = _inv.database.FetchItemByID(id);
            if (item.IsSkill && !_inv.ItemInInv(item) || item.IsGem || item.IsConsumable)
            {
                _inv.AddItem(id);
                _hero.gold -= _inv.database.FetchItemByID(id).Value;
            }
        }
    }*/
    public void Buy()
    {
        if(!hasItem || _hero.gold < _inv.database.FetchItemByID(id).Value || !_inv.hasSpaceEnoughItem())
        {
            AudioManager.instance.PlaySound(SoundsEnum.GENERAL_CLICK_BUTTON_1);
        }
      
        if (_hero.gold >= _inv.database.FetchItemByID(id).Value && hasItem && _inv.hasSpaceEnoughItem())
        {
            var item = _inv.database.FetchItemByID(id);
            
            if (item.IsSkill && !_inv.ItemInInv(item) || item.IsGem || item.IsConsumable)
            {
                if (!item.IsSkill)
                {
                    /*if (InvSwitch.instance.amountToBuy == 1)
                    {
                        _inv.AddItem(id);
                        _hero.gold -= _inv.database.FetchItemByID(id).Value;
                    }
                    else
                    {*/
                        for (int i = 0; i < InvSwitch.instance.amountToBuy; i++)
                        {
                            if (_inv.hasSpaceEnoughItem() && _hero.gold >= _inv.database.FetchItemByID(id).Value)
                            {
                                _inv.AddItem(id);
                                _hero.gold -= _inv.database.FetchItemByID(id).Value;
                            }
                        }

                   // }
                }
                else
                {
                    _inv.AddItem(id);
                    _hero.gold -= _inv.database.FetchItemByID(id).Value;

                }
                AudioManager.instance.PlaySound(SoundsEnum.SHOP_ON_BUY_SELL);

            }
        }
        if(_inv.database.FetchItemByID(id).IsSkill && _inv.ItemInInv(_inv.database.FetchItemByID(id)))
        {
            AudioManager.instance.PlaySound(SoundsEnum.GENERAL_CLICK_BUTTON_1);
        }
    }
    //
    //SELL FUNCIONANDO
    /*public void Sell()
    {
        if(hasItem && isShopSell)
        {
            _hero.gold += _inv.database.FetchItemByID(id).SellValue;
            GetComponent<Image>().sprite = sprite;
            hasItem = false;
        }
    }*/
    public void Sell()
    {
        if (!hasItem)
        {
            AudioManager.instance.PlaySound(SoundsEnum.GENERAL_CLICK_BUTTON_1);
            return;
        }
        if (!isStack && (hasItem && isShopSell /*&& InvSwitch.instance.amountToSell-1 <= _inv.CountItems(id) */|| InvSwitch.instance.amountToSell == 1))
        {
            if (InvSwitch.instance.amountToSell == 1)
            {
                _inv.RemoveItemByID(id, InvSwitch.instance.amountToSell - 1);
                _hero.gold += _inv.database.FetchItemByID(id).SellValue;
            }
            else
            {
                for (int i = 0; i < InvSwitch.instance.amountToSell - 1; i++)
                {
                    _inv.RemoveItemByID(id, InvSwitch.instance.amountToSell-1);
                    _hero.gold += _inv.database.FetchItemByID(id).SellValue;
                }
            }
            AudioManager.instance.PlaySound(SoundsEnum.SHOP_ON_BUY_SELL);
            GetComponent<Image>().sprite = sprite;
            hasItem = false;
        }
        else if(isStack && (hasItem && isShopSell /*&& InvSwitch.instance.amountToSell - 1 <= actualAmount */|| InvSwitch.instance.amountToSell == 1))
        {
            if (InvSwitch.instance.amountToSell == 1 && actualAmount > 1)
            {
                //_inv.RemoveItemByID(id, InvSwitch.instance.amountToSell - 1);
                actualAmount--;
                _hero.gold += _inv.database.FetchItemByID(id).SellValue;
            }
            else if(InvSwitch.instance.amountToSell > 1 && actualAmount - InvSwitch.instance.amountToSell >= 0)
            {
                for (int i = 0; i < InvSwitch.instance.amountToSell; i++)
                {
                    //_inv.RemoveItemByID(id, InvSwitch.instance.amountToSell - 1);
                    actualAmount--;
                    _hero.gold += _inv.database.FetchItemByID(id).SellValue;
                }
            }
            else if(InvSwitch.instance.amountToSell == 1 && actualAmount == 1)
            {
                actualAmount--;
                _hero.gold += _inv.database.FetchItemByID(id).SellValue;
            }
            AudioManager.instance.PlaySound(SoundsEnum.SHOP_ON_BUY_SELL);

            if (actualAmount == 0)
            {
                GetComponent<Image>().sprite = sprite;
                hasItem = false;
                isStack = false;
                amountText.text = "";

            }


        }
    }
    public void BuyItemSlot(int itemID)
    {
        GetComponent<Image>().sprite = _inv.database.FetchItemByID(itemID).Sprite;
        id = itemID;
        hasItem = true;
    }
   
    public void OnPointerClick(PointerEventData eventdata)
    {
        /* if(isShopSell && hasItem)
         {
             _inv.AddItem(id);
             GetComponent<Image>().sprite = sprite;
             hasItem = false;
         }*/
        if (eventdata.button == PointerEventData.InputButton.Right)
        {
            if(hasItem && isShopSell)
            {
                if(isStack)
                {
                    for (int i = 0; i < actualAmount; i++)
                    {
                        _inv.AddItem(id);
                    }
                }
                else
                {
                   _inv.AddItem(id);   
                }
                actualAmount = 0;          
                GetComponent<Image>().sprite = sprite;
                hasItem = false;
            }
            else if(hasItem && isShopBuy)
            {
                GetComponent<Image>().sprite = sprite;
                hasItem = false;
            }
        }
    }

    IEnumerator CoinBagDrop(int amountToDrop)
    {

        for (int i = 0; i < amountToDrop; i++)
        {
            var dropGold = Instantiate(droppedItem);
            dropGold.GetComponent<PickupItem>().myItem = 8;
            dropGold.transform.position = _hero.transform.position;
            yield return new WaitForEndOfFrame();

        }

        yield return null;

    }
    public void cancel()
    {
        trashit.SetActive(false);
        canceltrash.SetActive(false);
    }
    public void trashitem()
    {
        trashit.SetActive(false);
        canceltrash.SetActive(false);
        if (data.item.Slug == "CoinBag")
        {
            _hero.gold -= data.amount;

        }
        else if (data.item.Slug == "Coins")
        {
            _hero.gold -= 1;

        }
        _inv.RemoveItem(data);
    }
}

