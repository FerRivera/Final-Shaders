using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finder : MonoBehaviour
{
    [SerializeField]
    HeroModel _hero;
    [SerializeField]
    Inventory _inventory;
    ItemDatabase _itemDatabase;
    Trader _trader;
    [SerializeField]
    GameObject _hud;
    [SerializeField]
    GameObject _tooltip;
    [SerializeField]
    GameObject _inventoryPanel;
    [SerializeField]
    GameObject _canvas;
    [SerializeField]
    MinotaurBoss _boss;
    static Finder _finder;

    public static Finder Instance
    {
        get
        {
            return _finder;
        }
    }

    private void Awake()
    {
        if(_finder == null)
            _finder = this;

        if(_inventoryPanel == null)
            _inventoryPanel = GameObject.Find("Inventory Panel");
        if (_hero == null)
            _hero = GameObject.Find("Hero").GetComponent<HeroModel>();
        if (_inventory == null)
             _inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        _itemDatabase = _inventory.GetComponent<ItemDatabase>();
		if(_trader == null)
			_trader = GameObject.Find("Npc_Merchant").GetComponent<Trader>();					
        if(_hud == null)
            _hud = GameObject.Find("HUD");
        if(_tooltip == null)
            _tooltip = GameObject.Find("Tooltip");
        if (_canvas == null)
            _canvas = GameObject.Find("Canvas");
        if (_boss == null)
            _boss = GameObject.Find("Minotaur_Boss").GetComponent<MinotaurBoss>();
    }

    public GameObject inventoryPanel
    {
        get
        {
            return _inventoryPanel;
        }
    }

    public MinotaurBoss minotaurBoss
    {
        get
        {
            return _boss;
        }
    }

    public GameObject canvas
    {
        get
        {
            return _canvas;
        }
    }

    public HeroModel hero
    {
        get
        {
            return _hero;
        }
    }

    public Inventory inventory
    {
        get
        {
            return _inventory;
        }
    }

    public ItemDatabase itemDatabase
    {
        get
        {
            return _itemDatabase;
        }
    }
    public Trader trader
    {
        get
        {
            return _trader;
        }
    }

    public GameObject hud
    {
        get
        {
            return _hud;
        }
    }

    public GameObject tooltip
    {
        get
        {
            return _tooltip;
        }
    }
}
