using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(UnitOwner))]
public class InitUnitOwner : NetworkBehaviour {

    private bool showGui = true;
    private int teamNumber = 0;
    private TeamsController teamsController;
    UnitOwner owner;

    // Use this for initialization
    void Awake()
    {
        //if (!isLocalPlayer)
        //{
        //    enabled = false;
        //    return;
       //}

        owner = this.GetComponent<UnitOwner>();

        owner.playerName = "lalalal";
        owner.teamIndex = 0; // 0 - free for all; 
        owner.playerNetId = this.netId;

        InitTeamsNumber();


    }

    void InitTeamsNumber()
    {

        Debug.Log("InitTeamsNumber");

        GameObject obj =  GameObject.FindGameObjectWithTag("TeamsController");
        if (obj)
        {
            teamsController = obj.GetComponent<TeamsController>();
            if (teamsController)
            {
                teamNumber = teamsController.numberOfTeams;
            }
            else
            {
                Debug.LogError("Cannot GetComponent<TeamsController> ");
            }
        }
        else
        {
            Debug.LogError("Cannot FindGameObjectWithTag(TeamsController) ");
        }
    }

    void OnGUI()
    {
        if (showGui)
        {

            if ( (teamNumber) <=1 )
            {
                SubmitTeamSelection(0);
            }
            else
            {
                int screenHeight = Screen.height;
                int screenWidth = Screen.width;

                int buttonHeight = 100;
                int buttonWidth = 100;

                int numberOfTeams = teamsController.numberOfTeams;
                Debug.Log("numberOfTeams = " + numberOfTeams);

                int outterBorder = 5;
                int boxWidth = outterBorder * (numberOfTeams + 1 ) + numberOfTeams * buttonWidth;
                int boxHeight = buttonHeight + 30 + outterBorder;

                GUI.Box(new Rect((screenWidth - boxWidth) / 2, (screenHeight - boxHeight) / 2, boxWidth, boxHeight), "Select team");

                for ( int buttonIndex = 0; buttonIndex<numberOfTeams; buttonIndex++)
                {
                    int topLeftX = ((screenWidth - boxWidth) / 2) + buttonIndex * buttonWidth + outterBorder * buttonIndex + outterBorder;
                    int topLeftY = (screenHeight - boxHeight) / 2 + 30;


                    GuiTeamButton(topLeftX, topLeftY, buttonWidth, buttonHeight, buttonIndex );
                }


            }
        }
    }

    void GuiTeamButton(int topLeftX, int topLeftY, int width, int height, int teamIndex)
    {
        string buttonName = "";

        if (teamIndex == 0)
        {
            buttonName = "Madmans";
        }
        else
        {
            buttonName = "Team " + ((int)(teamIndex )).ToString();
        }

        if (GUI.Button(new Rect(topLeftX, topLeftY, width, height), "" ))
        {
            SubmitTeamSelection(teamIndex);
        }

        int numberOfPlayersInTheTeam = teamsController.numberOfPlayers[teamIndex];

        GUI.Label(new Rect(topLeftX + 5, topLeftY + 10, width - 10, 25), buttonName );
        GUI.Label(new Rect(topLeftX + 5, topLeftY + height/2, width - 10, 25), numberOfPlayersInTheTeam.ToString() );

    }



    void SubmitTeamSelection(int teamIndex)
    {

        owner.teamIndex = teamIndex;
        teamsController.RegisterPlayerSquadInTeam(this.gameObject);
        SetTeamSelectionGui(false);
        SendMessage("MsgTeamSelectionComplite");
    }
    
    void SetTeamSelectionGui(bool val)
    {
        showGui = val;
    }

}
