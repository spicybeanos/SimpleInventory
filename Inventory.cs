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
            Items.Add(item);
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
    public bool TryGet(Dictionary<string, object> properties, out Item item)
    {
        foreach (var i in Items)
        {
            if (PropertyMatch(properties,i))
            {
                item = i;
                return true;
            }
        }
        item = new Item();
        return false;
    }
    public bool TryGet(Property property, out Item item)
    {
        foreach (var i in Items)
        {
            if (i.Properties.ContainsKey(property.Name))
            {
                if (i.Properties[property.Name] == property.Value)
                {
                    item = i;
                    return true;
                }                
            }
        }
        item = new Item();
        return false;
    }
    public bool TryGet(int itemId ,Property property, out Item item)
    {
        foreach (var i in Items)
        {
            if(i.ID == itemId)
            {
                if (i.Properties.ContainsKey(property.Name))
                {
                    if (i.Properties[property.Name] == property.Value)
                    {
                        item = i;
                        return true;
                    }
                }
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

    public Result<bool> Add(Item item){
        if(SpaceOccupied() + item.Size <= Capacity)
        {
            Items.Add(item);
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
        if (index >= 0 && index < Capacity)
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
            if (PropertyMatch(properties, i))
            {
                return new Result<Item>() { Success = true,Value = i };
            }
        }
        return new Result<Item>() { Success = false,Value = new Item()};
    }
    public Result<Item> Get(Property property)
    {
        foreach (var i in Items)
        {
            if (i.Properties.ContainsKey(property.Name))
            {
                if (i.Properties[property.Name] == property.Value)
                {
                    return new Result<Item>() { Success = true,Value = i};
                }
            }
        }
        return new Result<Item>() { Success = false,Value = new Item()};
    }
    public Result<Item> Get(int itemId, Property property)
    {
        foreach (var i in Items)
        {
            if (i.ID == itemId)
            {
                if (i.Properties.ContainsKey(property.Name))
                {
                    if (i.Properties[property.Name] == property.Value)
                    {
                        return new Result<Item>() { Success = true,Value = i};
                    }
                }
            }
        }
        return new Result<Item>() { Success = false,Value = new Item()};
    }

    public bool PropertyMatch(Dictionary<string,object> match,Item test)
    {
        foreach (var key in match.Keys)
        {
            if (test.Properties.ContainsKey(key))
            {
                if (match[key] != test.Properties[key])
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        return true;
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
}
public struct Item
{
    public string Name { get; set; }
    public int ID { get; set; }
    public int Size { get; set; }
    public Dictionary<string, object> Properties { get; set; }
}
public struct Property
{
    public string Name { get; set; }
    public object Value { get; set; }
}
