using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : Actor
{
    private void Start()
    {
        speed = 2.0f;
    }

    public override Dictionary<string, object> SetGoal()
    {
        Dictionary<string, object> goal = new Dictionary<string, object>
        {
            // { "findCarrot", true },
            {"storeCarrot", true }
            // {"stayAlive", true }
        };

        return goal;
    }
}
