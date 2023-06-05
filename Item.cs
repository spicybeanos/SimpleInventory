using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}