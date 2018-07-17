using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TooltipShop : MonoBehaviour
{
    private Item item;
    private string data;
    public GameObject tooltip;
    Vector3 offset;
    private string greenColor;
    private string blueColor;
    private string purpleColor;

    void Start()
    {
        offset = new Vector3(20, 0);
        tooltip = Finder.Instance.canvas.transform.FindChild("TooltipForShop").gameObject;
        tooltip.SetActive(false);

        greenColor = "<color=#00ff00><b>";
        blueColor = "<color=#0000ff><b>";
        purpleColor = "<color=#8B008B><b>";
    }

    void Update()
    {
        if(tooltip.activeSelf)
        {
            tooltip.transform.position = Input.mousePosition + offset;
        }
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
        //data ="<color=#0473f0><b>" + item.Title + "</b></color>\n\n" + item.Description + value(item)+power(item)+ defence(item) + health(item)+"\nSlots Used: " + item.SlotsUsed+ cd(item) + equip(item) ;
        //tooltip.transform.GetChild(0).GetComponent<Text>().text = data;
        if (!item.IsGem)
        {
            data = "<color=#0473f0><b>" + item.Title + "</b></color>\n\n" + item.Description + power(item) + defence(item) + health(item) + cd(item) + "\nValue: " + item.Value;
            tooltip.transform.GetChild(0).GetComponent<Text>().text = data;
        }
        else
        {
            data = title(item) + "</b></color>\n\n" + "<color=#ffffff>" + item.Description + "</color>\n" + "<color=#DCA105>Forge Chance: "
                + item.CDTime + "</color>\n" + "<color=#EED118>Value: " + item.Value + "</color>";
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
        else if (item.IsGem && item.SlotsUsed == 4)
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
        if (item.Strength > 0)
        {
            string pow = "\nStrength: " + item.Strength;
            return pow;
        }
        else
        {
            return "";
        }
            
    }
    string value(Item item)
    {
        if (item.Value > 0)
        {
            string pow = "\n<b>Value:</b> " + item.Value;
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
