using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public GameObject[] unitList;
    //public GameObject selectedUnit;
    UnitsPrefubList unitPrefabs;

    // Use this for initialization
    void Start () {

        initUnitPrefabList();



    }
	
	// Update is called once per frame
	void Update () {


        SquadCommandToMove();
        if (Input.GetKeyDown("space"))
        {
            AddNewUnitToSquad();
        }


    }


    void initUnitPrefabList()
    {
        GameObject tmp = GameObject.FindGameObjectWithTag("UnitsPrefabList");
        unitPrefabs = tmp.GetComponent<UnitsPrefubList>();
        if (unitPrefabs == null) Debug.Log("UnitsPrefubList");
    }


    void SquadCommandToMove()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 hitPos;
            if (Surface.GetMouseHitPosition(out hitPos))
            {
                SquadFormationMoveTo(hitPos);
            }else
            {
                Debug.Log("Canoot find hit");
            }
            

        }
    }

    void SquadFormationMoveTo(Vector3 hitPos)
    {
        if ( unitList.Length > 0 )
        {
            foreach (GameObject unit in unitList)
            {
                if (unit != null)
                {
                    UnitMoovement movement;
                    movement = unit.GetComponent<UnitMoovement>();
                    if (movement != null)
                    {
                        movement.SetTarget(hitPos);
                    }
                }
            }
        }
    }



    /// <summary>
    /// false if list is full or some error accuared
    /// </summary>
    /// <returns></returns>
    bool AddNewUnitToSquad()
    {
        bool res = false;
        int i = 0;
        foreach (GameObject unitSlot in unitList)
        {
            if (unitSlot == null)
            {

                unitList[i] = CreateNewUnit();
                res = true;
                break;
            }
            i++;
        }


        return res;
    }

    GameObject CreateNewUnit()
    {
        return Instantiate(unitPrefabs.unitPrefubs[0], this.transform.position + new Vector3(0, 1, 0), this.transform.rotation) as GameObject;
    }

}
