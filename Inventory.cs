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

    public bool TryAdd(Item item)
    {
        if(SpaceOccupied() + item.Size <= Capacity)
        {
            TryStackAdd(item);
            return true;
        }
        else
        {
            return false;
        }
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
    private bool TryStackAdd(Item i)
    {
        if(SpaceOccupied() + i.Size <= Capacity)
        {
            //method in complete have to still impliment
            // if item isnt stackable or does not exist
            var r = Get(i.ID);
            if (r.Success)
            {
                if(r.Value.ID == i.ID && r.Value.Data == i.Data)
                {
                    if(r.Value.Stackable)
                    {
                        ChangeSize(r.Value,i.Size);
                        return true;
                    }
                    else
                    {
                        Items.Add(i);
                        return true;
                    }
                }else
                {
                    Items.Add(i);
                    return true;
                }
            }
            else
            {
                Items.Add(i);
                return true;
            }
        }
        else
        {
            return false;
        }
    }

    public Result<bool> Add(Item item){
        if(SpaceOccupied() + item.Size <= Capacity)
        {
            TryStackAdd(item);
            return new Result<bool>(){Success = true,Value = true};
        }
        else
        {
            return new Result<bool>(){Success = false,Value = false};
        }
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
            if (TryAdd(item))
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
    public Result<Item> Get(Dictionary<string, object> properties)
    {
        foreach (var i in Items)
        {
            
        }
        return new Result<Item>() { Success = false,Value = new Item()};
    }
    public Result<bool> ChangeSize(Item item, int change)
    {
        if (Items.Contains(item))
        {
            Items[Items.IndexOf(item)].Size += change;
            return new Result<bool>(true, true);
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
public struct Result<T>
{
    public bool Success {get;set;}
    public T Value {get;set;}
    public Result(bool success, T value)
    {
        Success = success;
        Value = value;
    }
}
public class Item
{
    public string Name { get; set; }
    public int ID { get; set; }
    public int Size { get; set; }
    public bool Stackable { get; set; }
    public string Data { get; set; }
}
public struct Property
{
    public string Name { get; set; }
    public object Value { get; set; }
}
