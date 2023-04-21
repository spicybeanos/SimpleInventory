using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEditorInternal;

public class InventoryBrowser : MonoBehaviour
{
    // Start is called before the first frame update
    public Inventory inventory;
    public int Capacity = 10;
    public int CurrentSlot = 0;
    public Item CurrentItem = new Item();
    public TextMeshProUGUI itemName;
    public bool AutoCreateComponents = true;

    void Start()
    {
        inventory = new Inventory(Capacity);
        if(itemName == null)
        {
            if (AutoCreateComponents)
            {
                GameObject c = new GameObject("AutoCreate_Canvas");
                Canvas canvas = c.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                CanvasScaler cscaler = c.AddComponent<CanvasScaler>();
                cscaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                c.AddComponent<GraphicRaycaster>();
                GameObject txt = new GameObject("AutoCreate_Item Name");
                txt.transform.parent = c.transform;
                itemName = txt.AddComponent<TextMeshProUGUI>();
            }
            else
            {
                Debug.LogWarning($"{nameof(itemName)} is null and trying to change it will give a null ref exception! Enable {nameof(AutoCreateComponents)} if you want to make the game objects automatically.");
            }
        }
        inventory.AddRange(
            new List<Item>()
            {
                new Item() { ID = 0, Name = "Rock", Size = 1 },
                new Item() { ID = 1, Name = "Stick", Size = 1 },
                new Item() { ID = 2, Name = "Ore", Size = 1 },
                new Item() { ID = 3, Name = "Sword", Size = 5 },
            }
            ) ;
        CurrentItem = inventory.GetAt(0).Success ? inventory.GetAt(0).Value : new Item();
    }

    private void Update()
    {
        itemName.text = CurrentItem.Name;
        if(Input.mouseScrollDelta.y > 0)
        {
            CurrentItem = ScrollAhead();
        }
        if(Input.mouseScrollDelta.y < 0)
        {
            CurrentItem = ScrollBehind();
        }
    }

    public Item ScrollAhead()
    {
        Result<Item> r = inventory.GetAt(++CurrentSlot);
        return r.Success ? r.Value : inventory.GetAt(--CurrentSlot).Value;
    }
    public Item ScrollBehind()
    {
        Result<Item> r = inventory.GetAt(--CurrentSlot);
        return r.Success ? r.Value : inventory.GetAt(++CurrentSlot).Value;
    }
}
