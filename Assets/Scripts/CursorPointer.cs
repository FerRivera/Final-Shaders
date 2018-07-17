using UnityEngine;
using System.Collections;

public class CursorPointer : MonoBehaviour
{
    CursorMode _mouse = CursorMode.Auto;
    public Texture2D defaultCursor;
    public Texture2D inventoryCursor;
    public Texture2D enemyCursor;
    bool _inventoryOpened;

    public static CursorPointer instance;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }
    void Start ()
    {
        ChangeToDefault();
    }
	
	void Update ()
    {
        
    }

    public void ChangeToInventory()
    {
        Cursor.SetCursor(inventoryCursor, Vector2.zero, _mouse);
        _inventoryOpened = true;
    }

    public void InventoryClosed()
    {
        _inventoryOpened = false;
    }

    public void ChangeToDefault()
    {
        if (!_inventoryOpened)
        {
            Cursor.SetCursor(defaultCursor, Vector2.zero, _mouse);
            //_inventoryOpened = false;
        }        
    }

    public void ChangeToAttack()
    {
        if (!_inventoryOpened)
        {
            Cursor.SetCursor(enemyCursor, Vector2.zero, _mouse);
        }        
    }

    //void OnMouseEnter()
    //{
    //    Debug.Log("asdasdasd");
    //    if (gameObject.layer == 12 && !_inventoryOpened)
    //    {
    //        Cursor.SetCursor(enemyCursor, Vector2.zero, _mouse);
    //    }
    //}

    //void OnMouseOver()
    //{
    //    Debug.Log("asdasdasd");

    //    if (gameObject.layer == 12 && !_inventoryOpened)
    //    {
    //        Cursor.SetCursor(enemyCursor, Vector2.zero, _mouse);
    //    }
    //}
}
