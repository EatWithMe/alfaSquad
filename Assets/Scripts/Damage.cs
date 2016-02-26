using UnityEngine;
using UnityEngine.Networking;
using System.Collections;


public struct Damage
{
    public float amount;
    public NetworkInstanceId ownderNetId;
}


/*
public class Damage
{

    public float amount;
    public NetworkInstanceId ownderNetId;

    public Damage(float newAmount, NetworkInstanceId newOwnderNetId)
    {
        amount = newAmount;
        ownderNetId = newOwnderNetId;
    }


}
*/
