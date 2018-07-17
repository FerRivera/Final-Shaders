using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;
using System.IO;

public class ItemDatabase : MonoBehaviour
{
    private List<Item> _database = new List<Item>();
    private JsonData _itemData;

    void Start()
    {
     
        _itemData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Items.json"));
        ConstructItemDatabase();        
    }

    public Item FetchItemByID(int id)
    {
        for (int i = 0; i < _database.Count; i++)
            if(_database[i].ID == id)
                return _database[i];
            
        return null;
    }
    public int FetchItemByString(string stringID)
    {
        for (int i = 0; i < _database.Count; i++)
            if (_database[i].Slug.Contains(stringID))
                return _database[i].ID;

        return 0;
    }

    void ConstructItemDatabase()
    {
       
        for (int i = 0; i < _itemData.Count; i++)
        {
            _database.Add(new Item((int)_itemData[i]["id"], _itemData[i]["title"].ToString(), (int)_itemData[i]["value"], (int)_itemData[i]["sellValue"],
             (int)_itemData[i]["stats"]["strength"], (int)_itemData[i]["stats"]["hitDamage"], (int)_itemData[i]["stats"]["defence"], (int)_itemData[i]["stats"]["health"],
             _itemData[i]["description"].ToString(), _itemData[i]["slug"].ToString(), (bool)_itemData[i]["stackable"], (int)_itemData[i]["slotsused"], (bool)_itemData[i]["isSkill"],
             (bool)_itemData[i]["isProjectile"], (int)_itemData[i]["stats"]["cd"], (bool)_itemData[i]["isAoe"], (bool)_itemData[i]["isSummon"], (bool)_itemData[i]["isConsumable"], (bool)_itemData[i]["isGem"]/*, (float)_itemData[i]["successRate"]*/));

        }
    }
    /*public JsonData dataFiltered(JsonData json)
    {
        if(_itemData.Keys)
        {

        }
    }*/
    
}


public class Item
{
    public int ID { get; set; }
    public string Title { get; set; }
    public int Value { get; set; }
    public int SellValue { get; set; }
    public int Strength { get; set; }
    public int Defence { get; set; }
    public int Health { get; set; }
    public string Description { get; set; }
    public string Slug { get; set; }
    public Sprite Sprite { get; set; }
    public bool Stackable { get; set; }
    public int SlotsUsed { get; set; }
    public bool IsSkill { get; set; }
    public GameObject prefabSkill { get; set; }
    public bool IsProjectile { get; set; }
    public int CDTime { get; set; }
    public int HitDamage { get; set; }
    public bool IsAoe { get; set; }
    public bool IsSummon { get; set; }
    public bool IsConsumable { get; set; }
    public bool Picked { get; set; }
    public bool IsGem { get; set; }
    public float SuccessRate { get; set; }


    public Item(int id, string title, int value, int sellValue, int strength, int hitdamage, int defence, int health, string description, string slug,
                bool stackable, int slotsused, bool isSkill, bool isProjectile, int cdTime, bool isAoe, bool isSummon, bool isConsumable, bool isGem/*, float successRate*/ )
    {
        this.ID = id;
        this.Title = title;
        this.Value = value;
        this.SellValue = sellValue;
        this.Strength = strength;
        this.Defence = defence;
        this.Health = health;
        this.Description = description;
        this.Slug = slug;
        this.Sprite = Resources.Load<Sprite>("Sprites/Items/" + slug);
        this.Stackable = stackable;
        this.SlotsUsed = slotsused;
        this.IsSkill = isSkill;
        this.prefabSkill = Resources.Load<GameObject>("PrefabSkills/" + slug);
        this.IsProjectile = isProjectile;
        this.CDTime = cdTime;
        this.HitDamage = hitdamage;
        this.IsAoe = isAoe;
        this.IsSummon = isSummon;
        this.IsConsumable = isConsumable;
        this.IsGem = isGem;
       
        //this.SuccessRate = successRate;
    }

    public Item()
    {
        this.ID = -1;
    }
}
