using UnityEngine;
using System.Collections;


public enum ItemType
{
    exp = 0,
    weapon = 1,
    item = 2
}


public class Item {

    public ItemType type;
    public int value;

    public Item()
    {
        type = ItemType.exp;
        value = 0;
    }

    public Item(ItemType t, int val)
    {
        type = t;
        value = val;
    }


}
