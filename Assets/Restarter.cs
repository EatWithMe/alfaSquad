using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Restarter : NetworkBehaviour
{

    private PlayerController squad;

	// Use this for initialization
	void Start ()
    {
        if (isLocalPlayer)
        {
            squad = GetComponent<PlayerController>();
            squad.OnSelection += OnUnitSelected;
        }
        else
        {
            enabled = false;
        }
    }

    
    // Update is called once per frame
	void Update ()
    {
        if (isLocalPlayer)
        {
            ManuralRespawn("r");
        }
	}


    IEnumerator CheckForDeathEvent()
    {
        bool weAreDead = false;

        do
        {
            if (squad.GetNumberOfUnits() <= 0) weAreDead = true;

            if (!weAreDead) yield return new WaitForSeconds(0.2f);
        }
        while (weAreDead == false);


        MakeOnDeathAction();

    }


    /// <summary>
    /// we need to get event for frist unit creation - so we now knows that players have first unit and he can die
    /// </summary>
    /// <param name="unit"></param>
    void OnUnitSelected(GameObject unit)
    {
        Debug.Log("Restarter : OnUnitSelection");
        squad.OnSelection -= OnUnitSelected;
        StartCoroutine(CheckForDeathEvent());
    }






    void ManuralRespawn(string key)
    {
        if (Input.GetKeyDown(key))
        {
            //Debug.Log("respawn");
            CmdRespawn();
        }
    }



    void MakeOnDeathAction()
    {
        //todo add delay here 
        CmdRespawn();
    }

    [Command]
    void CmdRespawn()
    {

        //Transform spawn = NetworkManager.singleton.GetStartPosition();
        Transform spawn = this.transform;

        GameObject newPlayer = Instantiate(NetworkManager.singleton.playerPrefab, spawn.position, spawn.rotation) as GameObject;
        NetworkServer.Destroy(this.gameObject);
        NetworkServer.ReplacePlayerForConnection(this.connectionToClient, newPlayer, this.playerControllerId);

    }


}
