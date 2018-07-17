using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Vessel : MonoBehaviour
{
    private Inventory _inv;
    public GameObject broken;
    public GameObject particleHit;
    public GameObject droppedItem;
    public List<GameObject> particles;
    public int amountOfGold;
    [Header("General Drop Chance")]
    public int GoldDropChance;
    public int GemDropChance;
    public int PotionDropChance;
    public int NoneChance;
    [Space(2)]
    [Header("Gems Drop Chance")]
    public int Common;
    public int Great;
    public int Epic;
    [Space(2)]
    [Header("Gems Type Drop Chance")]
    public int healthGem;
    public int StaminaGem;
    public int attackGem;
    public int defenceGem;
    [Space(2)]
    [Header("Potions Drop Chance")]
    public int Health;
    public int Stamina;
    [Space(2)]
    [Header("IDS")]
    public int goldID;

    public Dictionary<string, int> dictGeneral = new Dictionary<string, int>();
    public Dictionary<string, int> dictGemGeneral = new Dictionary<string, int>();
    public Dictionary<string, int> dictGemType = new Dictionary<string, int>();
    public Dictionary<string, int> dictPotions = new Dictionary<string, int>();

    private int totalWeightGeneral;
    private int totalWeightGemsTypes;
    private int totalWeightGemsTypes2;
    private int totalWeightPotionsTypes;

    void Awake()
    {
        foreach (var item in Resources.LoadAll("ParticlesDrops/"))
        {
            particles.Add((GameObject)item);
        }
        var num = Random.Range(0, 360);
        transform.Rotate(Vector3.up * num);
        _inv = GameObject.Find("Inventory").GetComponent<Inventory>();

        dictGeneral.Add("Gold",GoldDropChance);
        dictGeneral.Add("Gem", GemDropChance);
        dictGeneral.Add("Potion", PotionDropChance);
        dictGeneral.Add("None", NoneChance);

        dictGemGeneral.Add("Common", Common);
        dictGemGeneral.Add("Great", Great);
        dictGemGeneral.Add("Epic", Epic);

        dictGemType.Add("Health", healthGem);
        dictGemType.Add("Stamina", StaminaGem);
        dictGemType.Add("AttackPower", attackGem);
        dictGemType.Add("Defence", defenceGem);

        dictPotions.Add("HealthRestore", Health);
        dictPotions.Add("StaminaRestore", Stamina);

        foreach (var action in dictGeneral)
        {
            totalWeightGeneral += action.Value;
        }
        foreach (var item in dictGemGeneral)
        {
            totalWeightGemsTypes += item.Value;
        }
        foreach (var item in dictGemType)
        {
            totalWeightGemsTypes2 += item.Value;
        }
        foreach (var item in dictPotions)
        {
            totalWeightPotionsTypes += item.Value;
        }
    }
    public GameObject findParticle(string name)
    {
        foreach (var item in particles)
        {
            if(name.Contains(item.name))
            {
                return item;
            }
        }
        return particles[0];
    }

    protected void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == (int)LayersEnum.SWORD || c.gameObject.layer == (int)LayersEnum.SKILL)
        {
            var toDrop = RandomItem(dictGeneral,totalWeightGeneral);
            if(toDrop != "None")
            {
                var gold = Instantiate(droppedItem);
                gold.GetComponent<PickupItem>().myItem = stringToId(toDrop);
                gold.GetComponent<PickupItem>().goldAmount = amountOfGold;
                gold.transform.position = this.transform.position;
               // Debug.Log(_inv.database.FetchItemByID(stringToId(toDrop)).Slug);
                var specificPart = Instantiate(findParticle(_inv.database.FetchItemByID(stringToId(toDrop)).Slug));
                specificPart.transform.position = new Vector3(transform.position.x,transform.position.y +1 ,transform.position.z);

            }

            //rotura vasija
            var part = Instantiate(particleHit);
            var random = Random.Range(1,4);
            if(random == 1)
            {
                 AudioManager.instance.PlaySound(SoundsEnum.VESSEL_BREAK_1);
            }
            else if (random == 2)
            {
                AudioManager.instance.PlaySound(SoundsEnum.VESSEL_BREAK_2);
            }
            else if (random == 3)
            {
                AudioManager.instance.PlaySound(SoundsEnum.VESSEL_BREAK_3);
            }
            else if (random == 4)
            {
                AudioManager.instance.PlaySound(SoundsEnum.VESSEL_BREAK_4);
            }
            part.transform.position = c.transform.position;
            var roto = Instantiate(broken, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);

            roto.transform.rotation = this.transform.rotation;
            Destroy(this.gameObject);

        }

    }
    public int stringToId(string toTransform)
    {
        if(toTransform == "Gold")
        {
            return goldID;
        }
        else if(toTransform == "Gem")
        {
            string type = RandomItem(dictGemGeneral,totalWeightGemsTypes);
            var gem = RandomItem(dictGemType, totalWeightGemsTypes2);
            return _inv.database.FetchItemByString(gem.ToLower() + "Gem" + type);

        }
        else if(toTransform == "Potion")
        {
            string potion = RandomItem(dictPotions,totalWeightPotionsTypes);
            return _inv.database.FetchItemByString(potion);
        }
        return 0;
    }
 
    public string RandomItem(Dictionary<string,int> dictToRandom, int weight)
    {
        float random = Random.Range(0, weight);

        foreach (var action in dictToRandom)
        {
            random -= action.Value;
            if (random < 0)
            {
                return action.Key;
            }
        }
        return RandomItem(dictToRandom,weight);
    }
}
