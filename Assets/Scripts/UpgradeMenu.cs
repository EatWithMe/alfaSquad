using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(SquadExp))]
public class UpgradeMenu : NetworkBehaviour {


    public bool showGui = false;

    public GUISkin skin;
    public GUIStyle cardCostStyle;
    public GUIStyle nameStyle;
    public GUIStyle itemDescription;
    public GUIStyle itemVal;
    public GUIStyle separators;
    public GUIStyle closeMenu;

    private WeaponList weaponPrefabs;

    private Vector2 scrollPosition = Vector2.zero;
    private float scroolBarValue;

    private SquadExp squadExp;


    // Use this for initialization
    void Start()
    {
        if (!isLocalPlayer) enabled = false;

        initWeaponList();
        squadExp = GetComponent<SquadExp>();
    }

    void initWeaponList()
    {
        GameObject tmp = GameObject.FindGameObjectWithTag("WeaponList");
        weaponPrefabs = tmp.GetComponent<WeaponList>();
    }

    /*
    void OnGUI()
    {
        scrollPosition = GUI.BeginScrollView(new Rect(10, 10, 100, 50), scrollPosition, new Rect(0, 0, 220, 10));
        if (GUI.Button(new Rect(0, 0, 100, 20), "Go Right"))
            GUI.ScrollTo(new Rect(120, 0, 100, 20));

        if (GUI.Button(new Rect(120, 0, 100, 20), "Go Left"))
            GUI.ScrollTo(new Rect(0, 0, 100, 20));

        GUI.EndScrollView();
    }

    */
    
    void OnGUI()
    {

        if (showGui)
        {

            if (skin) GUI.skin = skin;

            int creenWidth = Screen.width;
            int creenHeight = Screen.height;


            GUI.Box(new Rect(50, 50, creenWidth - 100, creenHeight - 100), "Unit upgrade");


            

            
            scrollPosition = GUI.BeginScrollView(new Rect(50 +10 , 50+ 10, creenWidth - 120, creenHeight - 120), scrollPosition, new Rect(50 + 10, 50 + 10, creenWidth - 120, creenHeight - 120), false, true);
            
            

            for (int i=0; i < weaponPrefabs.Length; i++)
            {
                CreateButtonFOrWeapon(60 + i * 210, 60 ,  i);
            }

            GUI.EndScrollView();

            if (GUI.Button(new Rect( creenWidth/2 -25 , creenHeight-120, 50, 20), "Close" , closeMenu) )
            {
                showGui = false;
            }

        }
    }
    

    void CreateButtonFOrWeapon( int topLeftX, int topLeftY, int weaponIndex)
    {
        WeaponTemplate weapon = weaponPrefabs.GetWeaponPrefub(weaponIndex).GetComponent<WeaponTemplate>();

        const int cardWidth = 200;
        const int cardHeight = 300;
        //int topLeftX = 60 + index * 210 ;
        //int topLeftY = 60;

        int itemX = 110;

        int weapCost = weapon.weaponCost;

        if (GUI.Button(new Rect(topLeftX, topLeftY, cardWidth, cardHeight), ""))
        {
            //todo onWeaponSelection
            if ( squadExp.spendMoney(weapCost) )
            {
                SendMessage("SetWeaponForSelectedUnit", weaponIndex);
                ShowUpgradeMenu(false);
            }
        }

        GUI.Label(new Rect(topLeftX + 12, topLeftY + 26, 60, 25), weapon.weaponCost.ToString(), cardCostStyle);

        GUI.Label(new Rect(topLeftX + 70, topLeftY + 70, 60, 25), weapon.gameObject.name, nameStyle);


        GUI.Label(new Rect(topLeftX + 20, topLeftY + 130, 60, 25), "Damage:", itemDescription);
        GUI.Label(new Rect(topLeftX + itemX, topLeftY + 130, 60, 25), weapon.damage.ToString() , itemVal);

        GUI.Label(new Rect(topLeftX + 20, topLeftY + 155, 60, 25), "Clip size:", itemDescription);
        GUI.Label(new Rect(topLeftX + itemX, topLeftY + 155, 60, 25), weapon.clipSize.ToString() , itemVal);

        GUI.Label(new Rect(topLeftX + 20, topLeftY + 180, 60, 25), "Accuracy:", itemDescription);
        GUI.Label(new Rect(topLeftX + itemX, topLeftY + 180, 20, 25), weapon.accuracyMin.ToString("0.#") + " to " + weapon.accuracyMax.ToString("0.#"), itemVal);
        //GUI.Label(new Rect(topLeftX + 140, topLeftY + 160, 20, 25), "to", separators);
        //GUI.Label(new Rect(topLeftX + 160, topLeftY + 160, 20, 25), weapon.accuracyMax.ToString("0.#"), itemVal);

        GUI.Label(new Rect(topLeftX + 20, topLeftY + 205, 60, 25), "Acc. penalty:", itemDescription);
        GUI.Label(new Rect(topLeftX + itemX, topLeftY + 205, 20, 25), weapon.accuracyPenaltyPerShot.ToString("0.#"), itemVal);

        GUI.Label(new Rect(topLeftX + 20, topLeftY + 230, 60, 25), "Shoots delay:", itemDescription);
        GUI.Label(new Rect(topLeftX + itemX, topLeftY + 230, 20, 25), weapon.shotsDelay.ToString("0.#"), itemVal);


    }

    public void ShowUpgradeMenu(bool newState)
    {
        showGui = newState;
    }
}
