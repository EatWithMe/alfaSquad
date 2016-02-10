using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(UnitOwner))]
public class PlayerController : NetworkBehaviour {

    public GameObject[] unitList;
    //public GameObject testUnitPrefab;
    
    UnitsPrefubList unitPrefabs;
    public int selectedUnit = -1;

    //event for new unit seelction
    public delegate void OnSelectionAction(GameObject selected);
    public OnSelectionAction OnSelection;




    // Use this for initialization
    //public override void OnStartClient()
    void Start ()
    {
        
        //if (isLocalPlayer)
        {
            initUnitPrefabList();
            AddNewUnitToSquad();
            SelectAnyUnit();
        }
        

    }


    // Update is called once per frame

    void Update () {

        if (isLocalPlayer)
        {
            SquadCommandToMove();
            testPart();
        }

    }


    void testPart()
    {
        SpawnOnKeySpace();
        SwitchUnitOnKeyTab();
    }

    void SpawnOnKeySpace()
    {
        if (Input.GetKeyDown("space"))
        {
            Debug.Log("Space is Pressed");
            AddNewUnitToSquad();
        }
    }

    void SwitchUnitOnKeyTab()
    {
        if (Input.GetKeyDown("tab"))
        {
            SwitchSelectedUnit();
        }
    }

    void SwitchSelectedUnit()
    {
        int newIndex = selectedUnit;
        if (newIndex < 0) newIndex = 0;

        for ( int i = 1; i < ( unitList.Length ); i++)
        {
            newIndex ++;
            if (newIndex >= unitList.Length) newIndex -= unitList.Length;
            if (unitList[newIndex] != null)
            {
                selectUnit(newIndex);
                break;
                //we can return here
            }
        }
    }



    void initUnitPrefabList()
    {

        GameObject tmp = GameObject.FindGameObjectWithTag("UnitsPrefabList");
        unitPrefabs = tmp.GetComponent<UnitsPrefubList>();

        //unitPrefabs = GetComponent<UnitsPrefubList>();
        if (unitPrefabs == null) Debug.LogError("UnitsPrefubList DoesNot Found");
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

            GameObject selUnitl = getSelectedUnit();

            if (selUnitl != null)
            {

                int formationIndex = 0;
                int listIndex = selectedUnit;
                for ( int i = 0 ; i < unitList.Length; i++)
                {
                    


                    Vector3 moveTo = new Vector3(0,0,0);

                    GameObject unit = unitList[listIndex];

                    if (unit != null)
                    {
                        UnitMoovement movement;
                        movement = unit.GetComponent<UnitMoovement>();

                        if (movement != null)
                        {
                            if (foundNewFormatedPosition(formationIndex, selUnitl.transform.position, hitPos, out moveTo))
                            {
                                movement.SetTarget(moveTo);
                            }
                            else
                            {
                                movement.SetTarget(hitPos);
                            }
                        }
                        else
                        {
                            Debug.LogError("Cannot find movement at units");
                        }

                        formationIndex++;
                    }

                    listIndex++;
                    if (listIndex >= unitList.Length) listIndex -= unitList.Length;
                }

                /*
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
                */

            }
        }
    }


    /// <summary>
    /// true - if no point found
    /// </summary>
    /// <param name="formationIndex"></param>
    /// <param name="unitPos"></param>
    /// <param name="hitPos"></param>
    /// <param name="moveTo"></param>
    /// <returns></returns>
    bool foundNewFormatedPosition(int formationIndex, Vector3 unitPos, Vector3 hitPos, out Vector3 moveTo)
    {
        bool res = false;

        if ( formationIndex == 0)
        {
            res = true;
            moveTo = hitPos;
        }
        else
        {

            moveTo =  calculateNewPositionInFormation(formationIndex, unitPos, hitPos);
            res = true;
            /*
            NavMeshHit navMeshHit;
            if ( NavMesh.SamplePosition(hitPos , out navMeshHit , 5 , NavMesh.GetNavMeshLayerFromName("Walkable") ) )
            {
                
                moveTo = navMeshHit.position;
                res = true;
            }
            else
            {
                moveTo = hitPos;
                res = false;
            }
            */
        }

        return res;
    }


    /// <summary>
    /// perpendicular line formation
    /// </summary>
    /// <param name="index"></param>
    /// <param name="fromPos"></param>
    /// <param name="toPos"></param>
    /// <returns></returns>
    Vector3 calculateNewPositionInFormation(int index, Vector3 fromPos, Vector3 toPos)
    {

        const int DISTANSE_BETWEEN_UNITS = 2;
        Vector3  v = toPos - fromPos;

        Vector3 perpendic =  Vector3.Cross(v,Vector3.up);
        perpendic.Normalize();

        int koeff = index % 2;
        if (koeff == 0) koeff = -1;

        float c = (( index + 1) / 2) + 0.1f;
        int lenght = Mathf.RoundToInt (c)   * DISTANSE_BETWEEN_UNITS;
        
        Vector3 res = toPos + perpendic * koeff * lenght;
        return res;
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

                // WE CANNOT directy get spawned unit - he will communicate with us later
                //unitList[i] = CreateNewUnit();
                CmdCreateNewUnit();
                res = true;
                break;
            }
            i++;
        }


        return res;
    }

    [Command]
    public void CmdCreateNewUnit()
    {

       
        if (unitPrefabs == null) Debug.LogError("prefubList null");
        GameObject obj = Instantiate(unitPrefabs.unitPrefubs[0], this.transform.position + new Vector3(0, 1, 0), this.transform.rotation) as GameObject;
        //GameObject res = Instantiate(testUnitPrefab, this.transform.position + new Vector3(0, 1, 0), this.transform.rotation) as GameObject;


        if (obj == null)
        {
            Debug.LogError("CreateNewUnit: cannot instantiate");
            return ;
        }

        obj.SendMessage("SetSquadControler", this.gameObject);
        obj.SendMessage("setOwnerShip", GetComponent<UnitOwner>());

        // that will help created unit at clients to register at the squad
        obj.SendMessage("SetPanetNetId", this.netId);
        obj.SendMessage("SetParentRegistratorName", "AddUnitToSquad");
        obj.transform.parent = this.transform;


        //NetworkServer.SpawnWithClientAuthority(res, this.gameObject);

        NetworkServer.SpawnWithClientAuthority(obj, this.connectionToClient);
        //NetworkServer.Spawn(obj);


        return;
    }

    public void AddUnitToSquad(GameObject obj)
    {
        bool freeSlotIsFound = false;

        for (int i = 0; i < unitList.Length; i++)
        {
            if (unitList[i] == null)
            {
                unitList[i] = obj;
                freeSlotIsFound = true;
                break;
            }
        }

        if (!freeSlotIsFound) Debug.LogError("Cannot find free slot to assign new unit");

    }



    void SelectAnyUnit()
    {
        int i = 0;
        foreach (GameObject unitSlot in unitList)
        {
            if (unitSlot != null)
            {

                selectUnit( i);
                break;
            }
            i++;
        }
    }


    void selectUnit(int i)
    {
        if ( ( i < unitList.Length) && (i >=0 ) )
        {
            if (unitList[i] !=null)
            {
                selectedUnit = i;
                if (OnSelection !=null) OnSelection( unitList[i] );
            }
        }
    }


    GameObject getSelectedUnit()
    {
            if ( selectedUnit>=0 )
            {
                return unitList[selectedUnit];
            }
            else
            {
                return null;
            }
    }

    /// <summary>
    /// Message Handler - to remove dead units from the list
    /// </summary>
    /// <param name="obj"></param>
    public void UnitIsDead(GameObject obj)
    {
        Debug.Log("UnitIsDead");

        if ( obj!=null )
        {
            for (int i = 0; i < unitList.Length; i++)
            {
                if (unitList[i] == obj)
                {
                    unitList[i] = null;
                    ReportUnitIsDead(i);
                    return;
                }
            }
        }
    }

    //todo unitRemoved from squad
    void ReportUnitIsDead(int i)
    {
        if ( i== selectedUnit)
        {
            SelectAnyUnit();
        }
    }

}
