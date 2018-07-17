using UnityEngine;
using System.Collections;

public class RoomDropManager : MonoBehaviour
{
    public PickupItem DropObject;
    public GameObject dropPoint;
    public Item item;
    public int itemId;
	// Use this for initialization
	void Start () {
        
	}
    public void drop()
    {
        var drop = Instantiate(DropObject);
        drop.myItem = itemId;
        drop.transform.position = dropPoint.transform.position;
    }
	void Update () {
	    
	}
}
