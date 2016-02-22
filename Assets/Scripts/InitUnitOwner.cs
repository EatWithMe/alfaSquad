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
        owner = this.GetComponent<UnitOwner>();

        owner.playerName = "lalalal";
        owner.teamIndex = -1;
        owner.playerNetId = this.netId;

        InitTeamsNumber();


    }

    void InitTeamsNumber()
    {
        GameObject obj =  GameObject.FindGameObjectWithTag("TeamsController");
        if (obj)
        {
            TeamsController teamsController = obj.GetComponent<TeamsController>();
            if (teamsController)
            {
                teamNumber = teamsController.numberOfTeams;
            }
        }
    }

    void OnGUI()
    {
        if (showGui)
        {

            if ( (teamNumber+1) <=1 )
            {
                SubmitTeamSelection(-1);
            }
            else
            {
                int screenHeight = Screen.height;
                int screenWidth = Screen.width;

                int buttonHeight = 150;
                int buttonWidth = 100;

                int numberOfTeams = teamsController.numberOfTeams;

                int outterBorder = 5;
                int boxWidth = outterBorder * (numberOfTeams + 1) + numberOfTeams * buttonWidth;
                int boxHeight = buttonWidth + 30 + outterBorder;

                GUI.Box(new Rect((screenWidth - boxWidth) / 2, (screenHeight - boxHeight) / 2, boxWidth, boxHeight), "Select team");

                for ( int buttonIndex = 0; buttonIndex<numberOfTeams; buttonIndex++)
                {
                    int topLeftX = ((screenWidth - boxWidth) / 2) + outterBorder * buttonIndex;
                    int topLeftY = (screenHeight - boxHeight) / 2 + 30;


                    GuiTeamButton(topLeftX, topLeftY, buttonWidth, buttonHeight, buttonIndex -1 );
                }


            }
        }
    }

    void GuiTeamButton(int topLeftX, int topLeftY, int width, int height, int teamIndex)
    {
        string buttonName = "";

        if (teamIndex <= -1)
        {
            buttonName = "Madmans";
        }
        else
        {
            buttonName = "Team " + ((int)(teamIndex + 1)).ToString();
        }

        if (GUI.Button(new Rect(topLeftX, topLeftY, width, height), buttonName ))
        {
            SubmitTeamSelection(teamIndex);
        }

        int numberOfPlayersInTheTeam = teamsController.numberOfPlayers[teamIndex];
        GUI.Label(new Rect(topLeftX + width/2, topLeftY + height/2, 60, 25), numberOfPlayersInTheTeam.ToString() );

    }



    void SubmitTeamSelection(int teamIndex)
    {

        owner.teamIndex = teamIndex;
        teamsController.RegisterPlayerSquadInTeam(this.gameObject);
        SetTeamSelectionGui(false);
    }
    
    void SetTeamSelectionGui(bool val)
    {
        showGui = val;
    }

}
