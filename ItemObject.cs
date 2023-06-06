using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public Item item;
    public void PickMeUp(Inventory pickersInventory)
    {
        Result<int> res = pickersInventory.Add(item);
        if (res.Success)
        {
            Destroy(gameObject);
        }
        else
        {
            item.Size = res.Value;
        }
    }
}
