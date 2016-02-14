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

    void Update()
    {
        if (!isServer)
        {
            {
                transform.position = Vector3.Lerp(this.transform.position, realPosition, syncTime);
                transform.rotation = Quaternion.Lerp(this.transform.rotation, realRotation, syncTime);
            }

        }
    }


    void FixedUpdate()
    {
        
        if (!isServer)
        {
            /*
            {
                transform.position = Vector3.Lerp(this.transform.position, realPosition, syncTime);
                transform.rotation = Quaternion.Lerp(this.transform.rotation, realRotation,  syncTime);
            }
            */
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


}
