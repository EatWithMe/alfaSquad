using UnityEngine;
using UnityEngine.Networking;
using System.Collections;



public class Pickable : NetworkBehaviour {

    public ItemType itemType = ItemType.exp;
    public int itemValue = 0;

    private Item item;

    // Use this for initialization
    void Start ()
    {
        item = new Item(itemType, itemValue);
        //if (!isServer) GetComponent<Rigidbody>().detectCollisions = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void InitPickableItem(ItemType type, int val)
    {
        item.type = type;
        item.value = val;
    }

    [Server]
    void OnCollisionEnter(Collision other)
    {
        if (isServer)
        {
            if (other.gameObject.tag == "Unit")
            {
                //other.gameObject.SendMessage("TakeItem",itemType, itemValue);
                other.gameObject.SendMessage("TakeItem", item);
                KillThis();
            }
        }
    }

    [Server]
    void KillThis()
    {
        //Destroy(this.gameObject);
        NetworkServer.Destroy(this.gameObject);
    }
}
