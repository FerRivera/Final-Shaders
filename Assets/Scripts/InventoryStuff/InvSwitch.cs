using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvSwitch : MonoBehaviour
{
    private GameObject _slotPanelSkills;
    private GameObject _slotPanelSkillsButton;
    private GameObject _slotPanelSecondary;
    private GameObject _slotPanelSecondaryButton;
    private GameObject _inventoryPanel;
    private Inventory _inv;
    public static InvSwitch instance;
    public GameObject shopHelpButton;
    public GameObject forgeHelpButton;
    public GameObject shopHelp;
    public GameObject forgeHelp;
    public Text textAmountBuy;
    public Text textAmountSell;
    public int amountToBuy;
    public int amountToSell;

    void Start()
    {
        instance = this;
        _inv = Finder.Instance.inventory;
        _inventoryPanel = Finder.Instance.inventoryPanel;
        _slotPanelSkills = Finder.Instance.canvas.transform.FindChild("Slot Panel").gameObject;
        _slotPanelSecondary = _inventoryPanel.transform.FindChild("Slot Panel Secondary").gameObject;
        _slotPanelSkillsButton = _inventoryPanel.transform.FindChild("SkillsButton").gameObject;
        _slotPanelSecondaryButton = _inventoryPanel.transform.FindChild("ItemsButton").gameObject;
        amountToSell = 1;
        amountToBuy = 1;
        //SwitchToSkills();
        rewriteAmount();
    }
    void Update()
    {
        amountToBuy = (int)Mathf.Clamp(amountToBuy, 1, Mathf.Infinity);
        amountToSell = (int)Mathf.Clamp(amountToSell, 1, Mathf.Infinity);

        textAmountBuy.text = amountToBuy.ToString();
        textAmountSell.text = amountToSell.ToString();

    }
    public void goToForge()
    {
        shopHelpButton.SetActive(false);
        forgeHelpButton.SetActive(true);
    }
    public void goToShop()
    {
        shopHelpButton.SetActive(true);
        forgeHelpButton.SetActive(false);
    }
    public void ShowShopHelp()
    {
        shopHelp.SetActive(true);
    }
    public void ShowForgeHelp()
    {
        forgeHelp.SetActive(true);
    }
    public void AddBuy()
    {
        amountToBuy++;
    }
    public void DecreaseBuy()
    {
        amountToBuy--;
    }
    public void AddSell()
    {
        amountToSell++;
    }
    public void DecreaseSell()
    {
        amountToSell--;
    }
    public void SwitchToSkills()
    {     
        //_slotPanelSkillsButton.GetComponent<Button>().interactable = false;
       // _slotPanelSecondaryButton.GetComponent<Button>().interactable = true;
        _slotPanelSkills.SetActive(true);
        _slotPanelSecondary.SetActive(false);

    }

    public void SwitchToSecondary()
    {
        //_slotPanelSkillsButton.GetComponent<Button>().interactable = true;
        //_slotPanelSecondaryButton.GetComponent<Button>().interactable = false;

        _slotPanelSkills.SetActive(false);
        _slotPanelSecondary.SetActive(true);

    }

    public void rewriteAmount()
    {
        _slotPanelSkillsButton.transform.FindChild("Text").GetComponent<Text>().text = "Skills " + "(" + _inv.skillInvCount() + ")";
        _slotPanelSecondaryButton.transform.FindChild("Text").GetComponent<Text>().text = "Items " + "(" + _inv.itemInvCount() + ")";
    }
}
