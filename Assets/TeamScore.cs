using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic; //for list
using System; // for IComparable





public class TeamScore : NetworkBehaviour {


	// Use this for initialization
	

    //---------------------------------------------------------------------------
    //-----------                                                     -----------
    //-----------                                                     -----------
    public struct ScoreElementS
    {
        public string name;
        public float score;
    }


    public class SyncListScore : SyncListStruct<ScoreElementS>
    {

    }



    public class ScoreElement : IComparable<ScoreElement>
    {
        public string name;
        public float score;


        public ScoreElement(string newName, float newScore)
        {
            name = newName;
            score = newScore;
        }

        public int CompareTo(ScoreElement other)
        {
            if (other == null)
            {
                return 1;
            }

            return (int)(score - other.score);

        }

        public ScoreElementS GetStruct()
        {
            ScoreElementS sElem;
            sElem.name = name;
            sElem.score = score;
            return sElem;
        }
    }


    //-----------                                                     -----------
    //-----------                                                     -----------
    //---------------------------------------------------------------------------

    public float scoreUpdateDelaySec = 1f;
    public int maxPlayersInTop = 5;

    public SyncListScore topN = new SyncListScore();

    private float nextScoreUpdateTime = 0;

    TeamsController teamCtrl;

    private bool onShowGui = true;





    void Start()
    {

        

        if (isServer)
        {
            teamCtrl = GetComponent<TeamsController>();
        }
    }

    // Update is called once per frame; we dont need to run this all the time so FIxedUpdate
    void FixedUpdate()
    {

        if (isServer)
        {

            if (Time.time> nextScoreUpdateTime )
            {
                UpdateScore();
                nextScoreUpdateTime += scoreUpdateDelaySec;
            }
        }
    }



    void UpdateScore()
    {

        if ( teamCtrl!= null )
        {

            //local list to fill with Player name and player squad exp
            List<ScoreElement> allPlayers = new List<ScoreElement>();

            ArrayList teamsArray; // array of arrays filled from teamcontol
            teamsArray = teamCtrl.GetTeamsArray();
            if (teamsArray!= null)
            {
                if (teamsArray.Count > 0)
                {
                    for (int teamIndex = 0; teamIndex<= ( teamCtrl.numberOfTeams - 1); teamIndex++)
                    {
                        ArrayList team = (ArrayList)teamsArray[teamIndex];
                        if (team.Count>0)
                        {
                            for (int playerIndex = (team.Count-1);  playerIndex >=0; playerIndex--)
                            {
                                GameObject player = (GameObject)team[playerIndex];
                                if (player == null)
                                {
                                    team.RemoveAt(playerIndex);
                                    teamCtrl.numberOfPlayers[teamIndex]--;
                                }
                                else
                                {
                                    SquadExp  playerExp = player.GetComponent<SquadExp>();
                                    UnitOwner playerOwner  = player.GetComponent<UnitOwner>();

                                    allPlayers.Add(new ScoreElement(playerOwner.playerName, playerExp.GetTotalExp() ));
                                }
                            }
                        }
                    }
                }
            }


            PublishTopList(allPlayers); 


        }
    }

    void PublishTopList(List<ScoreElement> allPlayers)
    {
        allPlayers.Sort();

        topN.Clear();

        for (int i = 0; i <= maxPlayersInTop-1; i++)
        {
            if ( i >= allPlayers.Count)
            {
                // we neet to put empty element
                //if (i < topN.Count)
                //{
                //    topN.RemoveAt(i);
                //}
            }
            else
            {
                topN.Insert(i, allPlayers[i].GetStruct());
            }
        }

    }









    void OnGUI()
    {

        if (onShowGui)
        {
            if (topN.Count > 0)
            {

                int screenWidth = Screen.width;
                int screenHeight = Screen.height;

                int elemBorder = 5;
                int elemWidth = 160;
                int elemHeight = 23;


                //int boxNameHeight = 25;
                int boxNameHeight = 0;
                int boxWidth = elemWidth + 2 * elemBorder;
                int boxHeight = elemHeight * topN.Count + 2 * elemBorder + boxNameHeight;
                int boxTopLeftX = screenWidth - 10 - boxWidth;
                int boxTopLeftY = 10;

                //GUI.Box(new Rect(boxTopLeftX, boxTopLeftY, boxWidth, boxHeight), "Top" + maxPlayersInTop.ToString() );
                GUI.Box(new Rect(boxTopLeftX, boxTopLeftY, boxWidth, boxHeight), "");

                for (int elemIndex = 0; elemIndex < topN.Count; elemIndex++)
                {
                    int topLeftX = boxTopLeftX + elemBorder;
                    int topLeftY = boxTopLeftY + boxNameHeight + elemIndex * elemHeight;
                    DrowElement(topLeftX, topLeftY, elemIndex, topN[elemIndex].name, topN[elemIndex].score);
                }
            }
        }
    }


    void DrowElement(int topLeftX, int topLeftY, int elemIndex, string playerName, float playerScore)
    {
        GUI.Label(new Rect(topLeftX, topLeftY, 15, 23), (elemIndex + 1 ).ToString() + ":");
        GUI.Label(new Rect(topLeftX + 15, topLeftY, 100, 23), playerName );
        GUI.Label(new Rect(topLeftX + 15 + 100, topLeftY, 45, 23), playerScore.ToString("0") );
    }



}
