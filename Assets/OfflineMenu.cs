using UnityEngine;
using System.Collections;

public class OfflineMenu : MonoBehaviour {

    public GUISkin skin;

    public string playerName = "Player";
    public bool onGui = true;

    private bool onCreateServer = false; // do we need to show CreateServer menu
    private string serverName = "ServerName";

    // Use this for initialization
    void Start () {
	
	}
	

    void OnGUI()
    {
        if (!onGui) return;
        GUI.skin = skin;
            
        int creenWidth = Screen.width;
        int creenHeight = Screen.height;


        int menuWidth = 200;
        int menuHeight = 200;

        int topLeftX = ( creenWidth - menuWidth )  / 2 ;
        int topLeftY = (creenHeight - menuHeight ) / 2 ;

        int elementBorder = 20;
        int elementWidth = menuWidth - 2 * elementBorder;
        int elementHeight = 20;


        GUI.Box(new Rect(topLeftX, topLeftY, menuWidth , menuHeight), "Main menu");

        //-vv-----------------line1-----------------
        GUI.Label(new Rect(topLeftX + elementBorder, topLeftY + 25, elementWidth/2 , elementHeight), "Player Name:");

        playerName = GUI.TextField(new Rect(topLeftX + elementBorder + elementWidth / 2, topLeftY + 25, elementWidth / 2, elementHeight), playerName, 25);
        //-^^-----------------line1-----------------

        //-vv-----------------line2-----------------
        //connect
        if (GUI.Button( new Rect( topLeftX + elementBorder, topLeftY + 50 , elementWidth, elementHeight), "Connect"))
        {
            Debug.Log("Connect button is pressed");
        }
        //-^^-----------------line2-----------------

        //-vv-----------------line3-----------------
        //exit
        if (GUI.Button(new Rect(topLeftX + elementBorder, topLeftY + 75, elementWidth, elementHeight), "Create server"))
        {
            onCreateServer = true;
        }

        if (onCreateServer)
        {
            CreateServerGui(topLeftX + menuWidth + 10, topLeftY + 75);
        }
        //-^^-----------------line3-----------------


        //-vv-----------------line4-----------------
        //exit
        if (GUI.Button(new Rect(topLeftX + elementBorder, topLeftY + 100, elementWidth, elementHeight ), "Exit"))
        {
            Debug.Log("Connect exit  is pressed");
        }
        //-^^-----------------line4-----------------

    }


    void CreateServerGui(int topLeftX, int topLeftY)
    {
        GUI.Box(new Rect(topLeftX, topLeftY, 100, 100), "Create Server");

        //GUI.Label(new Rect(topLeftX + 10 , topLeftY + 25, 80, 20), "Name:");

        serverName = GUI.TextField(new Rect(topLeftX + 10, topLeftY + 25, 80, 20), serverName, 20);

        if (GUI.Button(new Rect(topLeftX + 10, topLeftY + 50, 80, 20), "Create"))
        {
            Debug.Log("Create server");
        }



        if (GUI.Button(new Rect(topLeftX + 10, topLeftY + 75, 80, 20), "Cancel"))
        {
            onCreateServer = false;// close subpanel
        }


    }

}
