using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class PickupItem : MonoBehaviour
{
    private Inventory _inv;
    public Color reliquiasColor;
    public Color skillColor;
    public int myItem;
    public int goldAmount;
    private int stackCount;
    bool _isOn;
    bool _destroyed;
    HeroModel _hero;
    [HideInInspector]
    public float pickupDist;
    public List<Collider> lColl;
    public GameObject saveRRelease;
    //private GameObject _miniInv;
    //public bool isSkillSave;
    //public string titleSave;
    public GameObject popupText;
    TextMesh _text;
    public GameObject particle;
    public bool isDropSkillRoom;


    public PickupItem(int item)
    {
        myItem = item;       
    }

    void Start()
    {
        lColl = new List<Collider>();  

        _hero = Finder.Instance.hero;
        _inv = Finder.Instance.inventory;
        _text = _inv.textNoMoreInv;
        var l = Physics.OverlapSphere(transform.position, 2, 1 << 26);
        lColl.AddRange(l);
        lColl.Remove(GetComponent<Collider>());
        stackCount = lColl.Count;

      //  EventsManager.TriggerEvent(EventType.suscribePickUpItem,this);      

       /* if (myItem == 11)
        {
            myItem++;
            throw new System.Exception("DIEGO ESTE ITEM ESTA PROHIBIDO DE USAR! (ID 13)");
        }*/
    }
    void Update()
    {
        /*if(Input.GetKey(KeyCode.LeftAlt) && !_destroyed)
        {
            if(!_isOn)
            {

                _isOn = true;
                if (!_inv.database.FetchItemByID(myItem).IsSkill)
                {

                    EventsManager.TriggerEvent(EventsType.spawnNamesText, reliquiasColor, _inv.database.FetchItemByID(myItem).Title, stackCount, gameObject, this);

                }
                else
                {
                    EventsManager.TriggerEvent(EventsType.spawnNamesText, skillColor, _inv.database.FetchItemByID(myItem).Title, stackCount, gameObject, this);

                }
            }

          

        }*/
        if(Input.GetKeyUp(KeyCode.LeftAlt))
        {
            _isOn = false;
        }
        if(Vector3.Distance(transform.position,_hero.transform.position) < pickupDist && !_destroyed && _inv.hasSpaceEnoughSkill())
        {
            PickUpItem();
        }


    }
    void OnMouseEnter()
    {
        /*if (!_isOn)
        {
            _isOn = true;
            if (!_inv.database.FetchItemByID(myItem).IsSkill)
            {

                EventsManager.TriggerEvent(EventsType.spawnNamesText, reliquiasColor, _inv.database.FetchItemByID(myItem).Title, 0, gameObject,this);

            }
            else
            {
                EventsManager.TriggerEvent(EventsType.spawnNamesText, skillColor, _inv.database.FetchItemByID(myItem).Title, 0, gameObject, this);

            }
        }*/

    }
    void OnMouseExit()
    {
        PickeablesNamesCanvas.instance.ReturnTextNamesToPool(saveRRelease);
        _isOn = false;
    }
    #region Deprecated
    public void OnMouseDown()
    {
        /*if (Vector3.Distance(_hero.transform.position, transform.position) <= 2)
        {
            // print(_inv.database.FetchItemByID(myItem).SlotsUsed);
            // print(_inv.spaceEnoughForItem(_inv.database.FetchItemByID(myItem).SlotsUsed));
            // if (_inv.spaceEnoughForItem(_inv.database.FetchItemByID(myItem).SlotsUsed))
            // {
            //if(_inv.database.FetchItemByID(myItem).SlotsUsed == 1)
            // {
            if (_inv.spaceEnoughForItem(_inv.database.FetchItemByID(myItem).SlotsUsed) || _inv.ItemInInv(_inv.database.FetchItemByID(myItem)) && _inv.database.FetchItemByID(myItem).Stackable)
            {
                _destroyed = true;
                //Hero.inputBlock = true;
                GetComponent<BoxCollider>().enabled = false;
                _inv.AddItem(myItem);
                GetComponent<MeshRenderer>().enabled = false;
               
                PickeablesNamesCanvas.instance.ReturnTextNamesToPool(saveRRelease);
                _isOn = false;
                if(myItem == 7)
                {
                    _inv.ReorganizeGoldInInv();
                }
                Destroy(gameObject, 1);
                StartCoroutine(MouseUpCoroutine());

            }
            else
            {

                StartCoroutine(ShowMessage("Not enough space", 2, this.transform));
                StartCoroutine(MouseUpCoroutine());

                return;
            }
            

        }*/
    }

        public void OnMouseDown(GameObject gm)
        {
        if (Vector3.Distance(_hero.transform.position, transform.position) <= 2)
        {
           
            if (_inv.spaceEnoughForItem(_inv.database.FetchItemByID(myItem).SlotsUsed) || _inv.ItemInInv(_inv.database.FetchItemByID(myItem)) && _inv.database.FetchItemByID(myItem).Stackable)
            {
                _destroyed = true;
                //Hero.inputBlock = true;
                GetComponent<BoxCollider>().enabled = false;
                _inv.AddItem(myItem);
                GetComponent<MeshRenderer>().enabled = false;

                PickeablesNamesCanvas.instance.ReturnTextNamesToPool(saveRRelease);
                _isOn = false;

                PickeablesNamesCanvas.instance.ReturnTextNamesToPool(gm);
                gm.SetActive(false);
                if (myItem == 7)
                {
                    _inv.ReorganizeGoldInInv();
                }
                gm.GetComponent<ReleaseAlt>().toFollow = null;
                gm.GetComponent<ReleaseAlt>().canvasToParent = null;
                gm.GetComponent<ReleaseAlt>().offSetStack = 0;
                Destroy(gameObject, 1);

                StartCoroutine(MouseUpCoroutine());

            }
            else
            {
                StartCoroutine(ShowMessage("Not enough space", 2, this.transform));
                StartCoroutine(MouseUpCoroutine());

                return;
            }

         

        }
    }
    public IEnumerator ShowMessage(string message, float delay, Transform data)
    {
        _text.gameObject.SetActive(true);
        _text.transform.position = data.transform.position- new Vector3(2.7f, -1);
        _text.text = message;
        yield return new WaitForSeconds(delay);
        _text.gameObject.SetActive(false);
    }
    #endregion

    IEnumerator MouseUpCoroutine()
    {
        yield return new WaitForEndOfFrame();
        //Hero.inputBlock = false;
    }
    public void OnMouseUp()
    {
        //Hero.inputBlock = false;
    }
    public void PickUpItem()
    {
        _destroyed = true;
        if (isDropSkillRoom)
        {
            var part = Instantiate(particle);
            part.transform.position = this.transform.position;
            part.GetComponent<ParticleAt>().target = _hero.gameObject;
            foreach (Transform item in part.transform)
            {
                item.GetComponent<ParticleAt>().target = _hero.gameObject;
            }
        }
        if(_inv.database.FetchItemByID(myItem).Title.Contains("Coins"))
        {
            AudioManager.instance.PlaySound(SoundsEnum.PICKUP_GOLD);
        }
        else
        {
            AudioManager.instance.PlaySound(SoundsEnum.PICKUP_GEM);

        }
        var asd = Instantiate(popupText);
        asd.GetComponent<PopupText>().textToShow =/* "You picked up " + */_inv.database.FetchItemByID(myItem).Title;
        asd.transform.position = this.transform.position - new Vector3(1, 0, 0);
        GetComponent<BoxCollider>().enabled = false;
        if(myItem == 3)
        {
            _hero.gold += goldAmount;
        }
        else
        {
            _inv.AddItem(myItem);
        }
        GetComponent<MeshRenderer>().enabled = false;

        PickeablesNamesCanvas.instance.ReturnTextNamesToPool(saveRRelease);
        _isOn = false;

        Destroy(gameObject, 1);
    }
    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.layer == (int)LayersEnum.HERO)
        {
            if (/*_inv.spaceEnoughForItem(_inv.database.FetchItemByID(myItem).SlotsUsed)*/  _inv.database.FetchItemByID(myItem).Slug == "Coins" || (_inv.hasSpaceEnoughItem() && !_inv.database.FetchItemByID(myItem).IsSkill) || (_inv.hasSpaceEnoughSkill() && _inv.database.FetchItemByID(myItem).IsSkill) || _inv.ItemInInv(_inv.database.FetchItemByID(myItem)) && _inv.database.FetchItemByID(myItem).Stackable)
            {
                
                PickUpItem();
            }
            else
            {

                //StartCoroutine(ShowMessage("Not enough space", 2, this.transform));
                // StartCoroutine(MouseUpCoroutine());
                var asd = Instantiate(popupText);
                asd.GetComponent<PopupText>().textToShow ="Not enough space!";
                asd.transform.position = this.transform.position - new Vector3(1, 0, 0);
                return;
            }
        }
        
    }


}
