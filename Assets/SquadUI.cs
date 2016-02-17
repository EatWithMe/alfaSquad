using UnityEngine;
using System.Collections;


[RequireComponent(typeof(SquadExp))]
public class SquadUI : MonoBehaviour {

    SquadExp sqExp;


	// Use this for initialization
	void Start ()
    {
        sqExp = GetComponent<SquadExp>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        Debug.Log("on guids");
        int screenWidth = Screen.width;
        int sreenHeight = Screen.height;

        


        //GUI.Box(new Rect(20, sreenHeight - 20 - 50 , 50, 50), "price");

        if (GUI.Button(new Rect(20, sreenHeight - 20 - 50, 50, 50), sqExp.money.ToString() ) )
        {
            OpenUpgradeMenu();
        }


    }

    void OpenUpgradeMenu()
    {
        SendMessage("ShowUpgradeMenu", true);
    }
}
