using UnityEngine;
using System.Collections;

public class SquadMessages : MonoBehaviour {



    public void MsgTeamSelectionComplite()
    {
        SendMessage("ShowMoneyGui", true);
        SendMessage("MsgAddNewUnitToSquad");
    }
}
