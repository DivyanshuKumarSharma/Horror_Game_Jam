using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public int maxItems = 5;

    public bool AddItem(Item item)
    {
        if (items.Count < maxItems)
        {
            items.Add(item);
            item.PickUp();
            return true;
        }
        return false;
    }

    public void RemoveItem(Item item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            item.Drop(transform.position);
        }
    }

    public Item GetItem(int index)
    {
        if (index >= 0 && index < items.Count)
        {
            return items[index];
        }
        return null;
    }
}
