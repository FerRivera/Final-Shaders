using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using System;

public class Slot : MonoBehaviour, IDropHandler
{
    public int id;
    public ItemData skillInInv;
    private Inventory inv;
    private InventoryForge invForge;
    public Slot next;
    public Slot previous;
    public bool locked;
    Image _raycast;

    float _cdTime;
    bool onCD;
    float _totalCdTime;

    public bool isSkill;
    public bool consumableOnly;
    public bool EquipableOnly;

    public int _HealthAdded;
    public int _DefenceAdded;
    public int _StrenghtAdded;
    public int _StaminaAdded;

    void Start()
    {
        inv = Finder.Instance.inventory;
        invForge = Finder.Instance.inventory.GetComponent<InventoryForge>();
        _raycast = GetComponent<Image>();
        if(!isSkill)
        {
            /*if (id != 0 && !inv.slots[id - 1].GetComponent<Slot>().isSkill)
            {
                previous = inv.slots[id - 1].GetComponent<Slot>();
            }
            if (inv.slots[id + 1] != null && !inv.slots[id + 1].GetComponent<Slot>().isSkill)
            {
                next = inv.slots[id + 1].GetComponent<Slot>();
            }*/
        }
        
    }
    void Update()
    {
        if(inv.items[id].ID != -1)
        {
            _raycast.raycastTarget = false;
        }
        else
        {
            _raycast.raycastTarget = true;
        }

        CDS();
    }

    public void Cast()
    {
        if (!locked)
        {
            if(skillInInv.item == null)
            {
                return;
            }
            if(skillInInv.item.IsConsumable)
            {
                ConsumeConsumable();
            }
            else
            {
                Instantiate(skillInInv.item.prefabSkill);
                _cdTime = _totalCdTime = skillInInv.item.CDTime;
                onCD = true;
                locked = true;
            }
        }
    }
    public void ConsumeConsumable()
    {
        if (skillInInv.item.IsConsumable)
        {
            Instantiate(skillInInv.item.prefabSkill);
            var skill = skillInInv;
            AudioManager.instance.PlaySound(SoundsEnum.HERO_DRINK_POTION);
            if (skill.amount >= 2)
            {
                skill.amount--;
                skill.transform.GetChild(0).GetComponent<Text>().text = skill.amount.ToString();
                _cdTime = _totalCdTime = skillInInv.item.CDTime;
                onCD = true;
                locked = true;
            }
            else if (skill.amount == 1)
            {
                skill.amount--;
                skill.transform.GetChild(0).GetComponent<Text>().text = skill.amount.ToString();
                skill.transform.GetChild(0).GetComponent<Text>().color = Color.red;
                inv.RemoveItem(skill);
            }
        }
    }
    void CDS()
    {
        if (onCD)
        {
            Sprite imgSkill = transform.GetChild(1).GetComponent<Image>().sprite;
            Image img = transform.GetChild(1).transform.GetChild(1).GetComponent<Image>();
            img.sprite = imgSkill;
            img.enabled = true;
            stuffForCDHud(img);

            _cdTime -= Time.deltaTime;

            img.fillAmount -= 1.0f / _totalCdTime * Time.deltaTime;

            if (_cdTime <= 0)
            {
                onCD = false;
                img.fillAmount = 100;
                img.color = Color.white;
                img.enabled = false;
                locked = false;
            }
        }
    }
    void stuffForCDHud(Image img)
    {
        img.type = Image.Type.Filled;
        img.fillOrigin = 90;
        img.fillClockwise = false;
        img.color = Color.red;
    }
    public void OnDrop(PointerEventData eventData)
    {
        ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData>();


        if (droppedItem == null || inv.slots[droppedItem.slot].GetComponent<Slot>().locked)
        {
            return;
        }
        else if (droppedItem.wasInForge)
        {
            if (id < inv.slotAmount || isSkill || consumableOnly)
            {
                Debug.Log("Not to this inventory");
                return;
            }
            inv.AddItem(id, droppedItem.item.ID);
            invForge.RemoveItem(droppedItem);
            return;
        }

        else if (droppedItem.item.SlotsUsed == 2 && (id == inv.slotAmount - 1 || id == 5 || id == 11 || id == 17))
        {
            for (int i = 5; i < inv.slotAmount + inv.slotAmountSecondaryInv; i += 6)
            {
                if (id == i)
                {
                    returnItemToPlace(droppedItem.gameObject);
                    return;
                }
                else if (i > id)
                    break;
            }
        }
        else if (droppedItem.item.SlotsUsed == 4 /*&& (id == inv.slotAmount - 1 || id == 5 || id == 11 || id == 17)*/)
        {
            /*while(inv.items.ElementAtOrDefault(id) != null)
            {

            }*/
            for (int i = 5; i < inv.slotAmount + inv.slotAmountSecondaryInv; i += 6)
            {
                if (id == i)
                {
                    returnItemToPlace(droppedItem.gameObject);
                    return;
                }
                else if (i > id)
                    break;
            }
            /* returnItemToPlace(droppedItem.gameObject);
             return;*/
        }

        if (droppedItem != null && inv.items[id].ID == -1)
        {
            if (inv.items[id].ID == -1)
            {
                if (!isSkill && droppedItem.item.IsSkill && id > inv.slotAmount)
                {
                    if (!consumableOnly && !droppedItem.item.IsConsumable)
                    {
                        return;
                    }
                }
                if (isSkill && droppedItem.item.IsSkill == true && !droppedItem.item.IsConsumable)
                {
                    inv.items[droppedItem.slot] = new Item();
                    inv.items[id] = droppedItem.item;
                    droppedItem.slot = id;
                    skillInInv = droppedItem;
                    AudioManager.instance.PlaySound(SoundsEnum.GENERAL_PICKUP_ITEM);

                }
                else if (consumableOnly && droppedItem.item.IsConsumable)
                {
                    inv.items[droppedItem.slot] = new Item();
                    inv.items[id] = droppedItem.item;
                    droppedItem.slot = id;
                    skillInInv = droppedItem;
                    AudioManager.instance.PlaySound(SoundsEnum.GENERAL_PICKUP_ITEM);
                }
                else if (EquipableOnly && droppedItem.item.IsGem)
                {
                    if (droppedItem.slot < inv.slotAmount + inv.slotAmountSecondaryInv + 4)
                    {
                        CalcHeroNewStats(droppedItem, true, droppedItem.slot,false);
                    }
                    else
                    {
                        CalcHeroNewStats(droppedItem, false, droppedItem.slot, true);
                    }
                    inv.items[droppedItem.slot] = new Item();
                    inv.items[id] = droppedItem.item;
                    droppedItem.slot = id;
                    skillInInv = droppedItem;
                    AudioManager.instance.PlaySound(SoundsEnum.GENERAL_PICKUP_ITEM);
                }
                else if (!droppedItem.item.IsSkill && isSkill || droppedItem.item.IsConsumable && id < inv.slotAmount && !consumableOnly || droppedItem.item.IsGem && id < inv.slotAmount)
                {
                    return;
                }
                else if (this.id <= inv.slotAmountSecondaryInv + inv.slotAmount + 2)
                {
                    if (droppedItem.item.SlotsUsed == 1)
                    {
                        if(droppedItem.slot >= inv.slotAmount + inv.slotAmountSecondaryInv +4)
                        {
                            CalcHeroNewStats(droppedItem, false,droppedItem.slot,false);
                        }
                        inv.items[droppedItem.slot] = new Item();
                        inv.items[id] = droppedItem.item;
                        droppedItem.slot = id;
                        skillInInv = droppedItem;
                        AudioManager.instance.PlaySound(SoundsEnum.GENERAL_PICKUP_ITEM);
                        //HeroModel.allSkills.Remove(skillInInv.gameObject);
                    }
                    #region item de 2 y 4
                    else if (droppedItem.item.SlotsUsed == 2)
                    {
                        if (inv.items[id + 1].ID == -1)
                        {
                            itemDeDosEMPTY(droppedItem);
                        }
                        else if (inv.items[id - 1].ID == -1 && !(id - 1 == inv.slotAmount - 1 || id - 1 == 5 || id - 1 == 11 || id - 1 == 17))
                        {
                            itemDeDosEMPTYReposition(droppedItem);
                        }
                    }
                    else if (droppedItem.item.SlotsUsed == 4)/*2x2*/
                    {
                        //if(id >= 18 && id <= 23 )
                        if (id >= inv.slotAmount - 6 && id <= inv.slotAmount - 1)
                        {
                            returnItemToPlace(droppedItem.gameObject);
                        }
                        else
                        {
                            if (inv.items.ElementAtOrDefault(id + 1) != null && inv.items.ElementAtOrDefault(id + 6) != null && inv.items.ElementAtOrDefault(id + 7) != null)
                            {
                                if (inv.items[id + 1].ID == -1 && inv.items[id + 6].ID == -1 && inv.items[id + 7].ID == -1)
                                {
                                    itemDeCuatroEMPTY(droppedItem);
                                }
                                else
                                {
                                    returnItemToPlace(droppedItem.gameObject);
                                }
                            }
                            else
                            {
                                returnItemToPlace(droppedItem.gameObject);
                            }

                        }
                    }
                    #endregion
                }
            }

        }

        //droppedItem = ITEM QUE ARRASTRO Y SUELTO
        //item = ITEM QUE ESTABA EN EL SLOT EN EL QUE SOLTE
        //SWAPEA ITEMS
        else if (droppedItem.slot != id)
        {
            if (this.id >= inv.slotAmount && droppedItem.item.IsSkill == false)
            {
                return;
            }

            Transform item;

            if (this.transform.childCount > 1)
            {
                item = this.transform.GetChild(1);
            }
            else if (this.transform.childCount > 0)
            {

                item = this.transform.GetChild(0);
            }
            else
            {
                return;
                //returnItemToPlace(droppedItem.gameObject);
            }


            //EL ITEM QUE SOLTE USA 1 SLOT?
            if (droppedItem.item.SlotsUsed == 1)
            {
                // var asd = item.GetComponent<ItemData>();
                //EL ITEM EN EL CUAL SOLTE, USA 1 SLOT?
                if (item.GetComponent<ItemData>().item.SlotsUsed == 1)
                {
                    if (!item.GetComponent<ItemData>().item.IsSkill && droppedItem.slot >= inv.slotAmount || inv.slots[item.GetComponent<ItemData>().slot].GetComponent<Slot>().locked
                        || droppedItem.item.IsSkill && EquipableOnly || droppedItem.item.IsConsumable && EquipableOnly || droppedItem.item.IsGem && !EquipableOnly)
                    {
                        //return;
                        returnItemToPlace(droppedItem.gameObject);
                    }
                    else if(droppedItem.item.IsSkill && id >= inv.slotAmount+inv.slotAmountSecondaryInv || droppedItem.item.IsSkill && id < inv.slotAmount)
                    {
                        inv.slots[droppedItem.slot].GetComponent<Slot>().skillInInv = item.GetComponent<ItemData>();
                        item.GetComponent<ItemData>().slot = droppedItem.slot;

                        item.transform.SetParent(inv.slots[droppedItem.slot].transform);
                        item.transform.position = inv.slots[droppedItem.slot].transform.position;

                        droppedItem.slot = id;
                        droppedItem.transform.SetParent(this.transform);
                        droppedItem.transform.position = this.transform.position;

                        inv.items[droppedItem.slot] = item.GetComponent<ItemData>().item;
                        inv.items[id] = droppedItem.item;
                        skillInInv = droppedItem;
                        AudioManager.instance.PlaySound(SoundsEnum.GENERAL_PICKUP_ITEM);
                    }
                    else if(droppedItem.item.IsSkill && id > inv.slotAmount || droppedItem.item.IsConsumable && id < inv.slotAmount || droppedItem.item.IsGem && id < inv.slotAmount)
                    {
                        returnItemToPlace(droppedItem.gameObject);
                    }
                    else
                    {
                        CalcHeroNewStats(droppedItem, true, droppedItem.slot,true);
                        inv.slots[droppedItem.slot].GetComponent<Slot>().skillInInv = skillInInv;
                        item.GetComponent<ItemData>().slot = droppedItem.slot;

                        item.transform.SetParent(inv.slots[droppedItem.slot].transform);
                        item.transform.position = inv.slots[droppedItem.slot].transform.position;

                        droppedItem.slot = id;
                        droppedItem.transform.SetParent(this.transform);
                        droppedItem.transform.position = this.transform.position;

                        inv.items[droppedItem.slot] = item.GetComponent<ItemData>().item;
                        inv.items[id] = droppedItem.item;
                        skillInInv = droppedItem;
                        AudioManager.instance.PlaySound(SoundsEnum.GENERAL_PICKUP_ITEM);
                    }


                }
                #region 1 en 2 y 4
                //EL ITEM EN EL CUAL SOLTE, USA 2 SLOTS?


                else if (item.GetComponent<ItemData>().item.SlotsUsed == 2 && inv.items[droppedItem.slot + 1].ID == -1 && inv.slots[droppedItem.slot].GetComponent<Slot>().id != 23)
                {
                    if (!item.GetComponent<ItemData>().item.IsSkill && droppedItem.slot >= inv.slotAmount)
                    {
                        returnItemToPlace(droppedItem.gameObject);

                        //return;
                    }
                    else
                    {
                        itemDe1en2(droppedItem, item);
                    }


                }
                //EL ITEM EN EL CUAL SOLTE, USA 4 SLOTS (2x2)?

                else if (item.GetComponent<ItemData>().item.SlotsUsed == 4)
                {
                    //var temp = inv.items[droppedItem.slot];
                    if (droppedItem.slot + 7 > inv.items.Count || droppedItem.slot >= 18 && droppedItem.slot <= 23 || droppedItem.slot == 5 || droppedItem.slot == 11 || droppedItem.slot == 17 || droppedItem.slot == 23)
                    {
                        return;
                    }
                    else if (inv.items[droppedItem.slot + 1].ID == -1 && inv.items[droppedItem.slot + 6].ID == -1 && inv.items[droppedItem.slot + 7].ID == -1 /*(2x2)*/)
                    {
                        itemDe1en4(droppedItem, item);
                    }

                }
                #endregion
            }

            #region old multiple slots
            //EL ITEM QUE SOLTE, USA 2 SLOTS?
            else if (droppedItem.item.SlotsUsed == 2)
            {
                //Lo solte en un item de 1 slot?
                if (item.GetComponent<ItemData>().item.SlotsUsed == 1)
                {
                    var temp = item.GetComponent<ItemData>().slot;
                    if (droppedItem.slot == temp + 1)
                    {
                        returnItemToPlace(droppedItem.gameObject);
                    }
                    else if (inv.items[id + 1].ID == -1)
                    {
                        itemDe2en1(droppedItem, item);
                    }
                    else
                    {
                        returnItemToPlace(droppedItem.gameObject);
                    }
                }
                //Lo solte en un item de 4 slots?
                else if (item.GetComponent<ItemData>().item.SlotsUsed == 4)
                {
                    var temp = item.GetComponent<ItemData>().slot;
                    if (droppedItem.slot == temp + 1 || droppedItem.slot == temp + 6 || droppedItem.slot == temp + 7)
                    {
                        returnItemToPlace(droppedItem.gameObject);
                    }
                    else if (inv.items[droppedItem.slot + 6].ID == -1 && inv.items[droppedItem.slot + 7].ID == -1)
                    {
                        itemDe2en4(droppedItem, item);
                    }
                    else //devolvelo donde estaba y queda  todo igual
                    {
                        returnItemToPlace(droppedItem.gameObject);
                    }
                }

                //Lo solte en un item de 2 slots?
                else if (item.GetComponent<ItemData>().item.SlotsUsed == 2)
                {
                    itemDe2en2(droppedItem, item);

                }
                else
                {
                    returnItemToPlace(droppedItem.gameObject);
                }




            }
            //EL ITEM QUE SOLTE, USA 4 SLOTS 2x2?
            else if (droppedItem.item.SlotsUsed == 4)
            {
                //solte el de 4 en uno de 1?
                if (item.GetComponent<ItemData>().item.SlotsUsed == 1)
                {
                    var temp = item.GetComponent<ItemData>().slot;
                    if (droppedItem.slot == temp + 1 || droppedItem.slot == temp + 6 || droppedItem.slot == temp + 7)
                    {
                        returnItemToPlace(droppedItem.gameObject);
                    }
                    //estan libres los slots que precisa?
                    else if (inv.items[id + 1].ID == -1 && inv.items[id + 6].ID == -1 && inv.items[id + 7].ID == -1)
                    {
                        itemDe4en1(droppedItem, item);
                    }
                    else //devolvelo donde estaba y queda  todo igual
                    {
                        returnItemToPlace(droppedItem.gameObject);
                    }
                }
                //Lo solte el de 4 en uno de 2?
                else if (item.GetComponent<ItemData>().item.SlotsUsed == 2 && inv.items[id + 6].ID == -1 && inv.items[id + 7].ID == -1)
                {
                    itemDe4en2(droppedItem, item);
                }
                //Lo solte el de 4 en uno de 4?
                else if (item.GetComponent<ItemData>().item.SlotsUsed == 4)
                {
                    itemDe4en4(droppedItem, item);
                }
                else //devolvelo donde estaba y queda  todo igual
                {
                    returnItemToPlace(droppedItem.gameObject);
                }

            }
            #endregion
        }

    }
    public void CalcHeroNewStats(ItemData item, bool add, int slotID, bool changeWithinGear)
    {
        if(add)
        {

            _HealthAdded = (int)(item.item.Health * Finder.Instance.hero.maxHealth) / 100;
            Finder.Instance.hero.maxHealth += _HealthAdded;

            //_DefenceAdded = (int)(item.item.Defence * Finder.Instance.hero.defence) / 100;
            _DefenceAdded = (int)item.item.Defence;
            Finder.Instance.hero.defence += _DefenceAdded;

            //_StrenghtAdded = (int)(item.item.Strength * Finder.Instance.hero.damage) / 100;
            _StrenghtAdded = item.item.Strength;
            Finder.Instance.hero.damageFirstHit += _StrenghtAdded;
            Finder.Instance.hero.damageSecondHit += _StrenghtAdded;
            Finder.Instance.hero.damageThirdHit += _StrenghtAdded;


            _StaminaAdded = (int)(item.item.HitDamage * Finder.Instance.hero.maxStamina) / 100;
            Finder.Instance.hero.maxStamina += _StaminaAdded;

        }
        else if(changeWithinGear)
        {
            var slot = inv.slots[slotID].GetComponent<Slot>();
            _HealthAdded = slot._HealthAdded;
            _StrenghtAdded = slot._StrenghtAdded;
            _DefenceAdded = slot._DefenceAdded;
            _StaminaAdded = slot._StaminaAdded;

            slot._HealthAdded = 0;
            slot._StrenghtAdded = 0;
            slot._DefenceAdded = 0;
            slot._StaminaAdded = 0;
        }
        else
        {
            var slot = inv.slots[slotID].GetComponent<Slot>();
            Finder.Instance.hero.maxHealth -= slot._HealthAdded;
            Finder.Instance.hero.defence -= slot._DefenceAdded;
            Finder.Instance.hero.damageFirstHit -= slot._StrenghtAdded;
            Finder.Instance.hero.damageSecondHit -= slot._StrenghtAdded;
            Finder.Instance.hero.damageThirdHit -= slot._StrenghtAdded;
            Finder.Instance.hero.maxStamina -= slot._StaminaAdded;
            slot._HealthAdded = 0;
            slot._StrenghtAdded = 0;
            slot._DefenceAdded = 0;
            slot._StaminaAdded = 0;

        }
    }
    void itemDeCuatroEMPTY(ItemData droppedItem)
    {
        inv.items[droppedItem.slot] = new Item();
        inv.items[droppedItem.slot + 1] = new Item();
        inv.items[droppedItem.slot + 6] = new Item();
        inv.items[droppedItem.slot + 7] = new Item();

        inv.items[id] = droppedItem.item;
        inv.items[id + 1].ID = droppedItem.item.ID;
        inv.items[id + 6].ID = droppedItem.item.ID;
        inv.items[id + 7].ID = droppedItem.item.ID;

        droppedItem.slot = id;
    }
    void itemDeDosEMPTY(ItemData droppedItem)
    {
        inv.items[droppedItem.slot] = new Item();
        inv.items[droppedItem.slot + 1] = new Item();
        inv.items[id] = droppedItem.item;
        inv.items[id + 1].ID = droppedItem.item.ID;
        droppedItem.slot = id;
    }
    void itemDeDosEMPTYReposition(ItemData droppedItem)
    {
        inv.items[droppedItem.slot] = new Item();
        inv.items[droppedItem.slot + 1] = new Item();
        inv.items[id] = droppedItem.item;
        inv.items[id - 1].ID = droppedItem.item.ID;
        droppedItem.slot = id-1;
    }
    void itemDe1en2(ItemData droppedItem, Transform item)
    {
        item.GetComponent<ItemData>().slot = droppedItem.slot;
        inv.items[id + 1] = new Item();
        inv.items[droppedItem.slot + 1].ID = droppedItem.item.ID;

        item.transform.SetParent(inv.slots[droppedItem.slot].transform);
        item.transform.position = inv.slots[droppedItem.slot].transform.position + new Vector3(33, 0);


        droppedItem.slot = id;
        droppedItem.transform.SetParent(this.transform);
        droppedItem.transform.position = this.transform.position;

        inv.items[droppedItem.slot] = item.GetComponent<ItemData>().item;
        inv.items[id] = droppedItem.item;
    }
    void itemDe1en4(ItemData droppedItem, Transform item)
    {
        item.GetComponent<ItemData>().slot = droppedItem.slot;
        inv.items[id + 1] = new Item();
        inv.items[id + 6] = new Item();
        inv.items[id + 7] = new Item();
        inv.items[droppedItem.slot + 1].ID = droppedItem.item.ID;
        inv.items[droppedItem.slot + 6].ID = droppedItem.item.ID;
        inv.items[droppedItem.slot + 7].ID = droppedItem.item.ID;

        item.transform.SetParent(inv.slots[droppedItem.slot].transform);
        item.transform.position = inv.slots[droppedItem.slot].transform.position + new Vector3(33, -30);


        droppedItem.slot = id;
        droppedItem.transform.SetParent(this.transform);
        droppedItem.transform.position = this.transform.position;

        inv.items[droppedItem.slot] = item.GetComponent<ItemData>().item;
        inv.items[id] = droppedItem.item;
    }
    void itemDe2en1(ItemData droppedItem,Transform item)
    {
        inv.items[droppedItem.slot + 1] = new Item();

        inv.items[droppedItem.slot] = item.GetComponent<ItemData>().item;
        item.GetComponent<ItemData>().slot = droppedItem.slot;
        item.transform.SetParent(inv.slots[droppedItem.slot].transform);
        item.transform.position = inv.slots[droppedItem.slot].transform.position;

        droppedItem.slot = id;
        droppedItem.transform.SetParent(this.transform);
        droppedItem.transform.position = this.transform.position;

        inv.items[droppedItem.slot] = item.GetComponent<ItemData>().item;
        inv.items[id] = droppedItem.item;

        inv.items[id + 1].ID = droppedItem.item.ID;
    }
    void itemDe2en2(ItemData droppedItem, Transform item)
    {
        inv.items[droppedItem.slot] = new Item();
        inv.items[droppedItem.slot + 1] = new Item();

        inv.items[droppedItem.slot] = item.GetComponent<ItemData>().item;
        inv.items[droppedItem.slot+1].ID = item.GetComponent<ItemData>().item.ID;
        item.GetComponent<ItemData>().slot = droppedItem.slot;
        item.transform.SetParent(inv.slots[droppedItem.slot].transform);
        item.transform.position = inv.slots[droppedItem.slot].transform.position + new Vector3(33, 0); 

        droppedItem.slot = id;
        droppedItem.transform.SetParent(this.transform);
        droppedItem.transform.position = this.transform.position + new Vector3(33, 0); 

        
        inv.items[id] = droppedItem.item;

        inv.items[id + 1].ID = droppedItem.item.ID;
    }
    void itemDe2en4(ItemData droppedItem, Transform item)
    {
        inv.items[droppedItem.slot] = new Item();
        inv.items[droppedItem.slot + 1] = new Item();
        inv.items[droppedItem.slot + 6] = new Item();
        inv.items[droppedItem.slot + 7] = new Item();

        inv.items[droppedItem.slot] = item.GetComponent<ItemData>().item;
        inv.items[droppedItem.slot + 1].ID = item.GetComponent<ItemData>().item.ID;
        inv.items[droppedItem.slot + 6].ID = item.GetComponent<ItemData>().item.ID;
        inv.items[droppedItem.slot + 7].ID = item.GetComponent<ItemData>().item.ID;

        item.GetComponent<ItemData>().slot = droppedItem.slot;
        item.transform.SetParent(inv.slots[droppedItem.slot].transform);
        item.transform.position = inv.slots[droppedItem.slot].transform.position + new Vector3(33, -30);

        droppedItem.slot = id;
        droppedItem.transform.SetParent(this.transform);
        droppedItem.transform.position = this.transform.position;



        inv.items[id] = droppedItem.item;
        inv.items[id + 1].ID = droppedItem.item.ID;
        inv.items[id + 6] = new Item();
        inv.items[id + 7] = new Item();

    }
    void itemDe4en1(ItemData droppedItem, Transform item)
    {
        inv.items[droppedItem.slot] = new Item();
        inv.items[droppedItem.slot + 1] = new Item();
        inv.items[droppedItem.slot + 6] = new Item();
        inv.items[droppedItem.slot + 7] = new Item();

        inv.items[droppedItem.slot] = item.GetComponent<ItemData>().item;
        item.GetComponent<ItemData>().slot = droppedItem.slot;
        item.transform.SetParent(inv.slots[droppedItem.slot].transform);
        item.transform.position = inv.slots[droppedItem.slot].transform.position; 

        droppedItem.slot = id;
        droppedItem.transform.SetParent(this.transform);
        droppedItem.transform.position = this.transform.position;

        

        inv.items[id] = droppedItem.item;       
        inv.items[id + 1].ID = droppedItem.item.ID;
        inv.items[id + 6].ID = droppedItem.item.ID;
        inv.items[id + 7].ID = droppedItem.item.ID;
    }
    void itemDe4en2(ItemData droppedItem, Transform item)
    {
        inv.items[droppedItem.slot] = new Item();
        inv.items[droppedItem.slot + 1] = new Item();
        inv.items[droppedItem.slot + 6] = new Item();
        inv.items[droppedItem.slot + 7] = new Item();


        inv.items[droppedItem.slot] = item.GetComponent<ItemData>().item;
        inv.items[droppedItem.slot+1].ID = item.GetComponent<ItemData>().item.ID;
        item.GetComponent<ItemData>().slot = droppedItem.slot;
        item.transform.SetParent(inv.slots[droppedItem.slot].transform);
        item.transform.position = inv.slots[droppedItem.slot].transform.position + new Vector3(33, 0);

        droppedItem.slot = id;
        droppedItem.transform.SetParent(this.transform);
        droppedItem.transform.position = this.transform.position;



        inv.items[id] = droppedItem.item;
        inv.items[id + 1].ID = droppedItem.item.ID;
        inv.items[id + 6].ID = droppedItem.item.ID;
        inv.items[id + 7].ID = droppedItem.item.ID;

    }
    void itemDe4en4(ItemData droppedItem, Transform item)
    {
        inv.items[droppedItem.slot] = new Item();
        inv.items[droppedItem.slot + 1] = new Item();
        inv.items[droppedItem.slot + 6] = new Item();
        inv.items[droppedItem.slot + 7] = new Item();


        inv.items[droppedItem.slot] = item.GetComponent<ItemData>().item;
        inv.items[droppedItem.slot + 1].ID = item.GetComponent<ItemData>().item.ID;
        inv.items[droppedItem.slot + 6].ID = item.GetComponent<ItemData>().item.ID;
        inv.items[droppedItem.slot + 7].ID = item.GetComponent<ItemData>().item.ID;
        item.GetComponent<ItemData>().slot = droppedItem.slot;
        item.transform.SetParent(inv.slots[droppedItem.slot].transform);
        item.transform.position = inv.slots[droppedItem.slot].transform.position + new Vector3(33, -30);

        droppedItem.slot = id;
        droppedItem.transform.SetParent(this.transform);
        droppedItem.transform.position = this.transform.position;



        inv.items[id] = droppedItem.item;
        inv.items[id + 1].ID = droppedItem.item.ID;
        inv.items[id + 6].ID = droppedItem.item.ID;
        inv.items[id + 7].ID = droppedItem.item.ID;

    }

    public void returnItemToPlace(GameObject item)
    {
        //Debug.Log("entre");
        var data = item.GetComponent<ItemData>();
        if (data.item.SlotsUsed == 2)
        {         
            inv.items[data.slot] = data.item;
            inv.items[data.slot + 1] = data.item;
        }
        else if (data.item.SlotsUsed == 4)
        {
            inv.items[data.slot] = data.item;
            inv.items[data.slot + 1] = data.item;
            inv.items[data.slot + 6] = data.item;
            inv.items[data.slot + 7] = data.item;
        }

    }
}


