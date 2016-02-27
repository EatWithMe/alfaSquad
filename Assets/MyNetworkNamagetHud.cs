using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using System.Collections;

public class MyNetworkNamagetHud : MonoBehaviour {


    private bool showGui = true;

    void Start()
    {
        NetworkManager.singleton.StartMatchMaker();
    }

    //call this method to request a match to be created on the server
    public void CreateInternetMatch(string matchName)
    {
        CreateMatchRequest create = new CreateMatchRequest();
        create.name = matchName;
        create.size = 32;
        create.advertise = true;
        create.password = "";

        NetworkManager.singleton.matchMaker.CreateMatch(create, OnInternetMatchCreate);
    }

    //this method is called when your request for creating a match is returned
    private void OnInternetMatchCreate(CreateMatchResponse matchResponse)
    {
        if (matchResponse != null && matchResponse.success)
        {
            //Debug.Log("Create match succeeded");

            Utility.SetAccessTokenForNetwork(matchResponse.networkId, new NetworkAccessToken(matchResponse.accessTokenString));
            MatchInfo hostInfo = new MatchInfo(matchResponse);
            NetworkServer.Listen(hostInfo, 9000);

            NetworkManager.singleton.StartHost(hostInfo);
        }
        else
        {
            Debug.LogError("Create match failed");
            SetOnShowGui(true);
        }
    }

    //call this method to find a match through the matchmaker
    public void FindInternetMatch(string matchName)
    {
        NetworkManager.singleton.matchMaker.ListMatches(0, 20, matchName, OnInternetMatchList);
    }

    //this method is called when a list of matches is returned
    private void OnInternetMatchList(ListMatchResponse matchListResponse)
    {
        if (matchListResponse.success)
        {
            if (matchListResponse.matches.Count != 0)
            {
                //Debug.Log("A list of matches was returned");

                //join the last server (just in case there are two...)
                NetworkManager.singleton.matchMaker.JoinMatch(matchListResponse.matches[matchListResponse.matches.Count - 1].networkId, "", OnJoinInternetMatch);
            }
            else
            {
                Debug.Log("No matches in requested room!");
                SetOnShowGui(true);
            }
        }
        else
        {
            Debug.LogError("Couldn't connect to match maker");
            SetOnShowGui(true);
        }
    }

    //this method is called when your request to join a match is returned
    private void OnJoinInternetMatch(JoinMatchResponse matchJoin)
    {
        if (matchJoin.success)
        {
            //Debug.Log("Able to join a match");

            if (Utility.GetAccessTokenForNetwork(matchJoin.networkId) == null)
                Utility.SetAccessTokenForNetwork(matchJoin.networkId, new NetworkAccessToken(matchJoin.accessTokenString));

            MatchInfo hostInfo = new MatchInfo(matchJoin);
            NetworkManager.singleton.StartClient(hostInfo);
        }
        else
        {
            Debug.LogError("Join match failed");
            SetOnShowGui(true);
        }
    }

    void OnGUI()
    {
        if (showGui)
        {
            int screenHeight = Screen.height;
            int screenWidth = Screen.width;

            int buttonHeight = 40;
            int buttonWidth = 100;


            int outterBorder = 5;
            int boxWidth = outterBorder * 2 + buttonWidth;
            int boxHeight = 3 * buttonHeight  + (2 +2 ) * outterBorder;
            int boxTopLeftX = (screenWidth - boxWidth) / 2;
            int boxTopLeftY = (screenHeight - boxHeight) / 2;

            GUI.Box(new Rect(boxTopLeftX, boxTopLeftY, boxWidth, boxHeight), "MainMenu");

            if (GUI.Button(new Rect(boxTopLeftX + outterBorder, boxTopLeftY+ 25 + outterBorder,buttonWidth, buttonHeight), "Play"))
            {
                FindInternetMatch("Default");
                SetOnShowGui(false);
            }

            if (GUI.Button(new Rect(boxTopLeftX + outterBorder, boxTopLeftY + 25 + buttonHeight + outterBorder*2, buttonWidth, buttonHeight), "Create") )
            {
                CreateInternetMatch("Default");
                SetOnShowGui(false);
            }
        }
    }

    void SetOnShowGui(bool val)
    {
        showGui = val;
    }
}
