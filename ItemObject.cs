using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public Item item;
    public void PickMeUp(Inventory pickersInventory)
    {
        Result<int> res = pickersInventory.Add(item);
    }
}
