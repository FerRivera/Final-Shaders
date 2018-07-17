using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tooltip : MonoBehaviour
{
    private Item item;
    private string data;
    private GameObject tooltip;
    private GameObject hud;
    Vector3 offset;
    private RectTransform rect;
    private string greenColor;
    private string blueColor;
    private string purpleColor;

    void Start()
    {
        hud = Finder.Instance.hud;
        rect = hud.GetComponent<RectTransform>();
        offset = new Vector3(20, 0);

        tooltip = Finder.Instance.tooltip;
        tooltip.SetActive(false);

        greenColor = "<color=#00ff00><b>";
        blueColor = "<color=#0000ff><b>";
        purpleColor = "<color=#8B008B><b>";

    }

    void Update()
    {
        if(tooltip.activeSelf)
        {
            if (isMouseOverHud())
            {
                offset = new Vector3(20, 100);
            }
            else
            {
                offset = new Vector3(20, 0);
            }
            tooltip.transform.position = Input.mousePosition + offset;
        }
        
    }

    public bool isMouseOverHud()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector3[] worldCorners = new Vector3[4];
        rect.GetWorldCorners(worldCorners);

        if (mousePosition.x >= worldCorners[0].x && mousePosition.x < worldCorners[2].x
           && mousePosition.y >= worldCorners[0].y && mousePosition.y < worldCorners[2].y)
        {
            return true;
        }
        return false;

    }


    public void Activate(Item item)
    {
        tooltip.transform.SetAsLastSibling();

        this.item = item;
        ConstructDataString();
        tooltip.SetActive(true);
    }

    public void Deactivate()
    {
        tooltip.SetActive(false);
    }

    public void ConstructDataString()
    {
        if(!item.IsGem)
        {
            data ="<color=#0473f0><b>" + item.Title + "</b></color>\n\n" + item.Description + power(item)+ defence(item) + health(item)+ cd(item) + "\nSell Value: " + item.SellValue;
            tooltip.transform.GetChild(0).GetComponent<Text>().text = data;
        }
        else
        {
            data = title(item) + "</b></color>\n\n" + "<color=#ffffff>" + item.Description + "</color>\n" + "<color=#DCA105>Forge Chance: "
                + item.CDTime + "</color>\n" + "<color=#EED118>Sell Value: " + item.SellValue + "</color>";
            tooltip.transform.GetChild(0).GetComponent<Text>().text = data;
        }
    }
    string title(Item item)
    {
        if (item.IsGem && item.SlotsUsed == 1)
        {
            return greenColor + item.Title;
        }
        else if (item.IsGem && item.Title.Contains("(Great)"))
        {
            return blueColor + item.Title;
        }
        else if(item.IsGem && item.SlotsUsed == 4)
        {
            return purpleColor + item.Title;
        }
        else
        {
            return item.Title;
        }
            
    }
    string power(Item item)
    {
        if(item.HitDamage != 0)
        {
            string pow = "\nHit Damage: " + item.HitDamage;
            return pow;
        }
       
        else if (item.Strength > 0)
        {
            string pow = "\nStrength: " + item.Strength;
            return pow;
        }
        else
        {
            return "";
        }
            
    }
   
    string defence(Item item)
    {
        if (item.Defence > 0)
        {
            string pow = "\nDefence: " + item.Defence;
            return pow;
        }
        else
        {
            return "";
        }

    }
    string health(Item item)
    {
        if (item.Health > 0)
        {
            string pow = "\nHealth: " + item.Health;
            return pow;
        }
        else
        {
            return "";
        }

    }
    string cd(Item item)
    {
        if (item.CDTime > 0)
        {
            string pow = "\nCooldown: " + item.CDTime;
            return pow;
        }
        else
        {
            return "";
        }

    }
    string equip(Item item)
    {
        if (item.IsSkill)
        {
            string pow = "\n<i>Equippable</i>";
            return pow;
        }
        else
        {
            return "\n<i>Non Equippable</i>";
        }

    }



}
