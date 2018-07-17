using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GoldCheckingScript : MonoBehaviour
{
    public GameObject shop;
    public List<ItemShop> lista;
    public HeroModel hero;
   
    void Start()
    {
        //IA2-P1
        //Chequeo si se repite un item dentro del shop
        bool check = lista.GroupBy(x => x.GetComponent<ItemShop>().id).Any(t => t.Count() > 1);
        if(check)
        {
            throw new System.Exception("El shop no puede contener dos items iguales!");
        }
    }
    void Update()
    {
        if (shop.activeSelf)
        {
      
            foreach (var item in lista)
            {
                if (hero.gold < item.item.Value)
                {
                    item.img.color = Color.red;
                }
                else
                {
                    item.img.color = Color.white;
                }
            }
        }
    }
 


}