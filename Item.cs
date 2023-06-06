using System;
using System.Collections.Generic;
using System.Linq;

public class Item
{
    public string Name { get; set; }
    public int ID { get; set; }
    public int Size { get; set; }
    public bool Stackable { get; set; }
    public string Data { get; set; }
    public Item()
    {

    }
    public Item(Item refItem, int size)
    {
        Name = refItem.Name;
        ID = refItem.ID;
        Size = size;
        Stackable = refItem.Stackable;
        Data = refItem.Data;
    }
    public Item(string name,int id,int size,bool stackable,string data)
    {
        Name =name; ID = id; Size = size; Stackable = stackable; Data = data;
    }
    public Item(string name, ItemIDs id, int size, bool stackable, string data)
    {
        Name = name; ID = (int)id; Size = size; Stackable = stackable; Data = data;
    }

    public static readonly Dictionary<ItemIDs, Item> DefaultItem = new Dictionary<ItemIDs, Item>()
    {
        {ItemIDs.Rock,new Item("Rock",ItemIDs.Rock,1,true,"") },
        {ItemIDs.Stick,new Item("Stick",ItemIDs.Stick,1,true,"") },
        {ItemIDs.Ore,new Item("Ore",ItemIDs.Ore,2,true,"{\"type\":\"default\"}") },
        {ItemIDs.Sword,new Item("Sword",ItemIDs.Sword,4,false,"{\"type\":\"default\"}") },
    };
    /// <summary>
    /// Creates and gives and Item based on the proper name
    /// </summary>
    /// <param name="ItemProperName">the name which exists in <see cref="ItemIDs"/></param>
    /// <returns></returns>
    public static Item NewItem(string ItemProperName)
    {
        List<string> items = new List<string>();
        items.AddRange(Enum.GetNames(typeof(ItemIDs)));
        if(items.Contains(ItemProperName))
        {
            object result;
            Enum.TryParse(typeof(ItemIDs), ItemProperName,out result);
        }
    }
}
public enum ItemIDs
{
    Rock,
    Stick,
    Ore,
    Sword
}