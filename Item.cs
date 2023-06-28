using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class Item : ScriptableObject
{
    public string Name { get; set; }
    /*
    i might change the id from an int to guid (uuid) cuz then it would in uniformity 
    and get rid of the problem of having conflicting item ids for two different items
     */
    public int ID { get; set; }
    public int Size { get; set; }
    public bool Stackable { get; set; }
    public string Data { get; set; }
}
public enum ItemIDs
{
    Rock,
    Stick,
    Ore,
    Sword
}