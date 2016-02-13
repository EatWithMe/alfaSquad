using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class NetSmoothMove : NetworkBehaviour {

    public int syncRate = 15;
    public float syncTime =     1f;


    [SyncVar]
    private Vector3 realPosition = Vector3.zero;
    [SyncVar]
    private Quaternion realRotation;

    private Vector3 sendPosition = Vector3.zero;
    private Quaternion sendRotation;

    private float nextSyncUpdateTime = 0;
    void Start()
    {
        
    }

    //reading from the stream
    public override bool OnSerialize(NetworkWriter writer, bool initialState)
    {

        Debug.Log("sadddsdadsssadasds WRTITE");

        writer.Write(this.transform.position);
        writer.Write(this.transform.rotation);
       


        return true;
    }



    //writing to the stream
    public override void OnDeserialize(NetworkReader reader, bool initialState)
    {
        //base.OnDeserialize(reader, initialState);
        Debug.Log("sadddsdadss");
        realPosition = reader.ReadVector3();
        realRotation = reader.ReadQuaternion();


        /* 
        Debug.Log("sadddsdadss");

        if (stream.isWriting)
        {//this is your information you will send over the network
            sendPosition = this.transform.position;
            sendRotation = this.transform.rotation;
            stream.Serialize(ref sendPosition);//im pretty sure you have to use ref here, check that
            stream.Serialize(ref sendRotation);//same with the ref here...
        }
        else if (stream.isReading)
        {//this is the information you will recieve over the network
            stream.Serialize(ref realPosition);//Vector3 position
            stream.Serialize(ref realRotation);//Quaternion postion
        }
        */
    }

    void FixedUpdate()
    {
        
        if (!isServer)
        {
            //if (!hasAuthority)
            {
                transform.position = Vector3.Lerp(this.transform.position, realPosition, Time.fixedDeltaTime / syncTime);
                transform.rotation = Quaternion.Lerp(this.transform.rotation, realRotation, Time.fixedDeltaTime / syncTime);
            }
        }
        else
        {
            //Debug.Log("aaaaaaaaaaaaaaa");
            SyncedCoordUpdate();        
        }

    }

    void SyncedCoordUpdate()
    {
        if (Time.time > nextSyncUpdateTime)
        {
            realPosition = this.transform.position;
            realRotation = this.transform.rotation;
            nextSyncUpdateTime = Time.time + syncTime;
        }
    }



    void Update()
    {
    }
}
