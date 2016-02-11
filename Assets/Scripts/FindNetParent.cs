using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class FindNetParent : NetworkBehaviour {

    [SyncVar]
    public NetworkInstanceId parentNetId;
    [SyncVar]
    public string parentRegitstatorMetod = "";

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }


    public override void OnStartClient()
    {
        // When we are spawned on the client,
        // find the parent object using its ID,
        // and set it to be our transform's parent.
        GameObject parentObject = ClientScene.FindLocalObject(parentNetId);
        if (parentObject != null)
        {
            transform.SetParent(parentObject.transform);

            if (parentRegitstatorMetod != "")
            {
                parentObject.SendMessage(parentRegitstatorMetod, this.gameObject);
            }
        }
        else
        {
            Debug.LogError("Cannot find net parent for object = [" + this.name + "]");
        }
    }


    public void SetPanetNetId(NetworkInstanceId pNetID)
    {
        parentNetId = pNetID;
    }
    public void SetParentRegistratorName(string metodName)
    {
        parentRegitstatorMetod = metodName;
    }


}
