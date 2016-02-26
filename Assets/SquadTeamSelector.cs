using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SquadTeamSelector : NetworkBehaviour {

    private bool showGui = true;
    private int teamNumber = 0;
    private TeamsController teamsController;

    UnitOwner owner;


    // Use this for initialization
    void Start () //awake
    {
        if (!isLocalPlayer) enabled = false;
        owner = GetComponent<UnitOwner>();
        StartCoroutine (InitTeamsNumber() );
    }

    //public override void OnStartClient()
    //{
    //    base.OnStartClient();
    //}


    // Update is called once per frame
    void Update () {
	
	}

    //IEnumerator WaitAndPrint(float waitTime)
    IEnumerator InitTeamsNumber()
    {
        GameObject obj;
        do
        {
            obj = GameObject.FindGameObjectWithTag("TeamsController");

            if (obj != null)
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
                yield return new WaitForSeconds(0.1f); // contimue after 100ms
            }


        }
        while (obj == null);
    }

    void OnGUI()
    {
        if ( showGui && (teamsController != null ) )
        {

            if ((teamNumber) <= 1)
            {
                SubmitTeamSelection(0);
            }
            else
            {
                int screenHeight = Screen.height;
                int screenWidth = Screen.width;

                int buttonHeight = 100;
                int buttonWidth = 100;

                int numberOfTeams = teamNumber;


                int outterBorder = 5;
                int boxWidth = outterBorder * (numberOfTeams + 1) + numberOfTeams * buttonWidth;
                int boxHeight = buttonHeight + 30 + outterBorder;

                GUI.Box(new Rect((screenWidth - boxWidth) / 2, (screenHeight - boxHeight) / 2, boxWidth, boxHeight), "Select team");

                for (int buttonIndex = 0; buttonIndex < numberOfTeams; buttonIndex++)
                {
                    int topLeftX = ((screenWidth - boxWidth) / 2) + buttonIndex * buttonWidth + outterBorder * buttonIndex + outterBorder;
                    int topLeftY = (screenHeight - boxHeight) / 2 + 30;


                    GuiTeamButton(topLeftX, topLeftY, buttonWidth, buttonHeight, buttonIndex);
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
            buttonName = "Team " + ((int)(teamIndex)).ToString();
        }

        if (GUI.Button(new Rect(topLeftX, topLeftY, width, height), ""))
        {
            SubmitTeamSelection(teamIndex);
        }

        

        GUI.Label(new Rect(topLeftX + 5, topLeftY + 10, width - 10, 25), buttonName);

        //initialisation of teamsController.numberOfPlayers can be delayed - so we must ignore it for a while
        if (teamsController.numberOfPlayers.Count > 0)
        {
            int numberOfPlayersInTheTeam = teamsController.numberOfPlayers[teamIndex];
            GUI.Label(new Rect(topLeftX + 5, topLeftY + height / 2, width - 10, 25), numberOfPlayersInTheTeam.ToString());
        }
        else
        {
            Debug.LogError("teamsController.numberOfPlayers is not initialised in time" );
        }
    }


    void SubmitTeamSelection(int teamIndex)
    {
        CmdSubmitTeamSelection(teamIndex);
        SetTeamSelectionGui(false);
        SendMessage("MsgTeamSelectionComplite");
    }

    [Command]
    void CmdSubmitTeamSelection(int teamIndex)
    {

        owner.teamIndex = teamIndex;
        teamsController.RegisterPlayerSquadInTeam(this.gameObject);
    }

    void SetTeamSelectionGui(bool val)
    {
        showGui = val;
    }

}
