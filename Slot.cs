using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot
{
    public Item item { get; set; }
    public int Count { get; set; }
    public int Size { get {  return Count * item.Size; } }
    public bool Stackable { get {  return item.Stackable; } }
    public int ID { get { return item.ID; } }
    public string Data { get { return item.Data; } }  
    public Slot()
    {
        Count = 0;
    }
    public Slot(Item item_,int count_)
    {
        item = item_;
        Count = count_;
    }

}
