using UnityEngine;
using System.Collections;



public class Pickable : MonoBehaviour {

    public ItemType itemType = ItemType.exp;
    public int itemValue = 0;

    private Item item;

    // Use this for initialization
    void Start ()
    {
        item = new Item(itemType, itemValue);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void InitPickableItem(ItemType type, int val)
    {
        item.type = type;
        item.value = val;
    }


    void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.tag == "Unit")
        {
            //other.gameObject.SendMessage("TakeItem",itemType, itemValue);
            other.gameObject.SendMessage("TakeItem", item);
            KillThis();
        }
    }

    void KillThis()
    {
        Destroy(this.gameObject);
    }
}
