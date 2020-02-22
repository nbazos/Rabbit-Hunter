using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : Actor
{
    public override Dictionary<string, object> SetGoal()
    {
        Dictionary<string, object> goal = new Dictionary<string, object>
        {
            { "findCarrot", true }
        };

        return goal;
    }
}
