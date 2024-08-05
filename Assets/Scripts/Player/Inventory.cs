using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Weapon> weapons;
    public List<Item> items;
    public Weapon SelectedWeapon;

    private void Awake()
    {
        if (weapons == null)
        {
            weapons = new List<Weapon>();
        }
        if (items == null)
        {
            items = new List<Item>();
        }
    }

    public void AddWeapon(Weapon weapon)
    {
        if (!HasWeapon(weapon))
        {
            weapons.Add(weapon);
            Debug.Log("Weapon added: " + weapon.weaponName);
        }
        else
        {
            Debug.Log("Weapon already in inventory: " + weapon.weaponName);
        }
    }

    public Weapon GetWeapon(int index)
    {
        if (index >= 0 && index < weapons.Count)
        {
            Debug.Log("Retrieved weapon: " + weapons[index].weaponName);
            SelectedWeapon = weapons[index];
            return SelectedWeapon;
        }
        Debug.Log("No weapon found at index: " + index);
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

    public bool HasWeapon(Weapon weapon)
    {
        foreach (Weapon w in weapons)
        {
            if (w.weaponName == weapon.weaponName)
            {
                return true;
            }
        }
        return false;
    }
}
