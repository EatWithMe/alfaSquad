using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class NetSpawnRotation : NetworkBehaviour {

    [SyncVar]
    public Quaternion startRotation;

    /// <summary>
    /// we need this to initialize starting rotation for Network Spawn
    /// </summary>
    public override void OnStartClient()
    {
        if (startRotation != null)
        {
            this.transform.rotation = startRotation;
        }
        else
        {
            Debug.LogError("You Forgot set start rotation for object [" + this.gameObject.name + "]");
        }
    }

    public void SetNetStartingRotation(Quaternion rot)
    {
        startRotation = rot;
    }
}

