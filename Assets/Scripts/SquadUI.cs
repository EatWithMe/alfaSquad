using UnityEngine;
using UnityEngine.Networking;
using System.Collections;


[RequireComponent(typeof(SquadExp))]
public class SquadUI : NetworkBehaviour {

    SquadExp sqExp;
    private bool onGui = false;

	// Use this for initialization
	void Start ()
    {
        if (!isLocalPlayer) enabled = false;

        sqExp = GetComponent<SquadExp>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {

        if (onGui)
        {
            int screenWidth = Screen.width;
            int sreenHeight = Screen.height;

            //GUI.Box(new Rect(20, sreenHeight - 20 - 50 , 50, 50), "price");

            if (GUI.Button(new Rect(20, sreenHeight - 20 - 50, 50, 50), sqExp.money.ToString()))
            {
                OpenUpgradeMenu();
            }
        }


    }

    void OpenUpgradeMenu()
    {
        SendMessage("ShowUpgradeMenu", true);
    }

    public void ShowMoneyGui(bool val)
    {
        onGui = val;
    }
}
