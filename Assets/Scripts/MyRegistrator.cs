using UnityEngine;
using UnityEngine.Networking;

using System.Collections;

public class MyRegistrator
{

    public static void NetworkPrefubsRegistration(GameObject[] prefubList)
    {
        if (prefubList != null)
        {
            for (int i = 0; i <= (prefubList.Length - 1); i++)
            {
                if (prefubList[i] != null)
                {
                    ClientScene.RegisterPrefab( prefubList[i] );
                }
            }
        }
    }

    public static void NetworkPrefubRegistration(GameObject prefub)
    {
        if (prefub!= null)
        {
            ClientScene.RegisterPrefab(prefub);
        }
    }
}
