using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class Trader : MonoBehaviour
{
    public GameObject shop;
    public GameObject Inventory;
    public Inventory inv;
    private InventoryForge _InventoryForge;
    //private Inventory _Inventory;
    public GameObject popUpText;
    public GameObject ForgeLayout;
    public GameObject ShopLayout;
    public GameObject ButtonForgeItem;
    public List<ItemShop> lista = new List<ItemShop>();
    public HeroModel hero;
    [HideInInspector]
    public Button ActualButtonForgeItem;
    public List<GameObject> turnOffWhenToForge;
    public List<GameObject> turnOffOtherShops;
    public List<GameObject> traders;
    public GameObject trader;
    public GameObject shopMiniButtons;
    //public GameObject trader1;
    //public GameObject trader2;
   
    public GameObject items;
    //public GameObject items1;
    //public GameObject items2;
    public bool isActive;
    private bool _close;
    //private int toForge; // 1 equals 1 slot gems, 2 equals 2 slot gems.


    void Start ()
    {
        inv = Finder.Instance.inventory.GetComponent<Inventory>();
        _InventoryForge = Finder.Instance.inventory.GetComponent<InventoryForge>();
        ActualButtonForgeItem = ButtonForgeItem.GetComponent<Button>();
        shop.SetActive(false);
        //Chequeo si se repite un item dentro del shop
        bool check = lista.GroupBy(x => x.GetComponent<ItemShop>().id).Any(t => t.Count() > 1);
        if (check)
        {
            throw new System.Exception("El shop no puede contener dos items iguales!");
        }
    }
    
    void OnMouseDown()
    {
        if(!shop.gameObject.activeSelf)
        {
            //Hero.inputBlock = true;
            foreach (var item in traders)
            {
                item.gameObject.SetActive(false);
            }
            foreach (var item in turnOffOtherShops)
            {
                item.gameObject.SetActive(false);
            }
            shop.SetActive(true);
            ShopLayout.SetActive(true);
            pauseGame.screens.Push(shop.gameObject);
            if (!Inventory.activeSelf)
            {
                Inventory.SetActive(true);
                inv.iconInventory.SetActive(true);
                pauseGame.screens.Push(Inventory.gameObject);
            }
            isActive = true;
            pauseGame.pauseGameInstance.Pause();
        }
        else
        {
           /* //Hero.inputBlock = false;
            shop.SetActive(false);
            pauseGame.screens.Pop();
            if (Inventory.activeSelf)
            {
                Inventory.SetActive(false);
                pauseGame.screens.Pop();
            }
            if(shop.GetComponent<TooltipShop>().tooltip.activeSelf)
            {
                shop.GetComponent<TooltipShop>().Deactivate();
            }
            if (textNoMoreSpace.activeSelf)
            {
                textNoMoreSpace.SetActive(false);
            }
            pauseGame.pauseGameInstance.UnPause();*/
        }
        
    }
    void Update()
    {
        if(shop.gameObject.activeSelf)
        {
            //Hero.inputBlock = true;
            foreach (var item in lista)
            {
                if(item.item == null)
                {
                    continue;
                }
                if ((item.item != null && hero.gold < item.item.Value) || (inv.ItemInInv(item.item) && item.item.IsSkill))
                {
                    item.img.color = Color.red;
                }
                else if(item.item != null)
                {
                    item.img.color = Color.white;
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(shop.gameObject.activeSelf && (!InvSwitch.instance.shopHelp.activeSelf && !InvSwitch.instance.forgeHelp.activeSelf))
            {
                closeThis();
            }
            else if(InvSwitch.instance.shopHelp.activeSelf || InvSwitch.instance.forgeHelp.activeSelf)
            {
                InvSwitch.instance.shopHelp.SetActive(false);
                InvSwitch.instance.forgeHelp.SetActive(false);
            }
            
        }   

      
    }

    public void closeThis()
    {
        _close = true;
        GoToShop();
        _close = false;
        shop.SetActive(false);
        //Hero.inputBlock = false;
        _InventoryForge.QuitShopReset();
        InvSwitch.instance.shopHelp.SetActive(false);
        InvSwitch.instance.forgeHelp.SetActive(false);
        foreach (var item in traders)
        {
            item.gameObject.SetActive(true);
        }
        if (Inventory.activeSelf)
        {
            Inventory.SetActive(false);
            inv.iconInventory.SetActive(false);
            pauseGame.screens.Pop();
        }
        if (shop.GetComponent<TooltipShop>().tooltip.activeSelf)
        {
            shop.GetComponent<TooltipShop>().Deactivate();
        }
        /*if(textNoMoreSpace.activeSelf)
        {
            textNoMoreSpace.SetActive(false);
        }*/
        pauseGame.screens.Pop();
        pauseGame.pauseGameInstance.UnPause();
    }
    public void GoToForge()
    {
        if (!_close)
        {
            AudioManager.instance.PlaySound(SoundsEnum.FORGE_SHOP_TAB_SWITCH);
        }

        ForgeLayout.SetActive(true);
        ButtonForgeItem.SetActive(true);
        ShopLayout.SetActive(false);
        foreach (var item in turnOffWhenToForge)
        {
            item.SetActive(false);
        }
        InvSwitch.instance.goToForge();
        shopMiniButtons.SetActive(false);
    }
    public void GoToShop()
    {
        if (!_close)
        {
            AudioManager.instance.PlaySound(SoundsEnum.FORGE_SHOP_TAB_SWITCH);
        }
        ForgeLayout.SetActive(false);
        ButtonForgeItem.SetActive(false);
        shopMiniButtons.SetActive(true);

        //ShopLayout.SetActive(true);

        if (trader.activeSelf)
        {
            //ShopLayout.SetActive(true);
            items.SetActive(true);
            //items1.SetActive(false);
            //items2.SetActive(false);
        }
       /* else if(trader1.activeSelf)
        {
            items.SetActive(false);
            items1.SetActive(true);
            items2.SetActive(false);
        }
        else if (trader2.activeSelf)
        {
            items.SetActive(false);
            items1.SetActive(false);
            items2.SetActive(true);
        }*/
        foreach (var item in turnOffWhenToForge)
        {
            item.SetActive(true);
        }
        InvSwitch.instance.goToShop();

    }
    public void ForgeItem()
    {
        if(gameObject.activeSelf)
        {
            AudioManager.instance.PlaySound(SoundsEnum.FORGE_ONCLICK_HAMMER);
            _InventoryForge.ForgeItem();
        }
    }
    public void Clear()
    {
        if (gameObject.activeSelf)
        {
            _InventoryForge.emptyInventory();
        }
    }
}
