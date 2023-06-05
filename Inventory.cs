using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;

public class Inventory
{
    
    public int Capacity { get; private set; }
    public List<Item> Items { get;protected set; }

    public Inventory(int capacity)
    {
        Capacity = capacity;
        Items = new List<Item>();
    }

    /*
    public bool TryAdd(Item item)
    {
        return StackAdd(item).Success;
    }
    public bool TryGet(string itemName,out Item item)
    {
        foreach (var i in Items)
        {
            if(i.Name == itemName)
            {
                item = i;
                return true;
            }
        }
        item = new Item();
        return false;
    }
    public bool TryGet(int itemID, out Item item)
    {
        foreach (var i in Items)
        {
            if (i.ID == itemID)
            {
                item = i;
                return true;
            }
        }
        item = new Item();
        return false;
    }
    
    public bool TryRemoveFirst(int itemID)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].ID == itemID)
            {
                Items.RemoveAt(i);
                return true;
            }
        }
        return false;
    }
    public bool TryRemoveFirst(Item item)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].Equals(item))
            {
                Items.RemoveAt(i);
                return true;
            }
        }
        return false;
    }
    public bool TryRemoveFirst(string itemName)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].Name == itemName)
            {
                Items.RemoveAt(i);
                return true;
            }
        }
        return false;
    }
    */
    /// <summary>
    ///  Tries to add the item to the inventory
    /// </summary>
    /// <param name="i">Item to be added</param>
    /// <returns>Whether the item was completely added and the item size amount that has not been added if the item wasn't completely added</returns>
    private Result<int> StackAdd(Item i)
    {
        int sizeAdd = 0;
        bool addedComplete = false;
        if(SpaceOccupied() + i.Size <= Capacity)
        {
            sizeAdd = i.Size;
            addedComplete = true;
        }
        else if(SpaceOccupied() <= Capacity)
        {
            sizeAdd = Capacity - SpaceOccupied();
            addedComplete = false;
        }
        else
        {
            return new Result<int>(false,i.Size);
        }

        //method in complete have to still impliment
        // if item isnt stackable or does not exist
        Result<Item> r = Get(i.ID);
        if (r.Success)
        {
            if (r.Value.ID == i.ID && r.Value.Data == i.Data)
            {
                if (r.Value.Stackable)
                {
                    ChangeSize(r.Value, sizeAdd);
                    return new Result<int>(addedComplete,i.Size - sizeAdd);
                }
                else if (addedComplete)
                {
                    Items.Add(i);
                    return new Result<int>(true,0);
                }
                else
                {
                    return new Result<int>(false,i.Size);
                }
            }
            else
            {
                Items.Add(new Item(i,sizeAdd));
                return new Result<int>(addedComplete,i.Size - sizeAdd);
            }
        }
        else
        {
            Items.Add(new Item(i, sizeAdd));
            return new Result<int>(addedComplete, i.Size - sizeAdd);
        }
    }
    
    /// <summary>
    /// Add item to the inventory
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public Result<int> Add(Item item)
    {
        return StackAdd(item);
    }
    /// <summary>
    /// Tries to add <paramref name="items"/> to inventory.
    /// If is unsuccesful, the has the amount of items successfully added
    /// </summary>
    /// <param name="items">items to be added to inventory</param>
    /// <returns>status of oppperation and the amount of items successfully added</returns>
    public Result<int> AddRange(ICollection<Item> items)
    {
        int ctr = 0;
        foreach (var item in items)
        {
            var res = Add(item);
            if (res.Success)
            {
                ctr++;
            }
            else
            {
                return new Result<int>() { Success = false, Value = ctr };
            }
        }
        return new Result<int>() { Success = true ,Value = items.Count};
    }
    public Result<Item> GetAt(int index)
    {
        if (index >= 0 && index < Items.Capacity)
        {
            return new Result<Item>() { Success = true, Value = Items[index] };
        }
        else
        {
            return new Result<Item>() { Success = false, Value = new Item() };
        }
    }
    public Result<Item> Get(int itemID){
        foreach (var i in Items)
        {
            if (i.ID == itemID)
            {
                return new Result<Item>(){Success = true,Value = i};
            }
        }
        return new Result<Item>(){Success = false};
    }
    public Result<Item> Get(int itemID,string data)
    {
        foreach (var i in Items)
        {
            if (i.ID == itemID && i.Data == data)
            {
                return new Result<Item>() { Success = true, Value = i };
            }
        }
        return new Result<Item>() { Success = false };
    }
    public Result<Item> Get(string itemName)
    {
        foreach (var i in Items)
        {
            if (i.Name == itemName)
            {
                return new Result<Item>() { Success = true,Value = i };
            }
        }
         
        return new Result<Item>() { Success = false,Value = new Item() };
    }
    public Result<bool> ChangeSize(Item item, int change)
    {
        if (Items.Contains(item))
        {
            if(Items[Items.IndexOf(item)].Size + change <= Capacity)
            {
                Items[Items.IndexOf(item)].Size += change;
                return new Result<bool>(true, true);
            }
            else
            {
                return new Result<bool>(false, false);
            }
        }
        return new Result<bool>(false,false);
    }

    public int SpaceOccupied()
    {
        int space = 0;
        foreach (var item in Items)
        {
            space += item.Size;
        }
        return space;
    }
}
public class Result<T>
{
    public bool Success {get;set;}
    public T Value {get;set;}
    public Result(bool success, T value)
    {
        Success = success;
        Value = value;
    }
    public Result()
    {

    }
}
