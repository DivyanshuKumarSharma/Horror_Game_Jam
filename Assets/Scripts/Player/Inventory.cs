using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<Weapon> weapons;
    private List<Item> items;

    private void Start()
    {
        weapons = new List<Weapon>();
        items = new List<Item>();
    }

    public void AddWeapon(Weapon weapon)
    {
        weapons.Add(weapon);
    }

    public Weapon GetWeapon(int index)
    {
        if (index >= 0 && index < weapons.Count)
        {
            return weapons[index];
        }
        return null;
    }

    public void AddItem(Item item)
    {
        items.Add(item);
    }

    public bool HasItem(string itemName)
    {
        foreach (Item item in items)
        {
            if (item.itemName == itemName)
            {
                return true;
            }
        }
        return false;
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);
    }

    public int GetItemCount()
    {
        return items.Count;
    }

    public int GetWeaponCount()
    {
        return weapons.Count;
    }
}
