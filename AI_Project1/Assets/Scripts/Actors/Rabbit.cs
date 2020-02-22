using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : Actor
{
    public override List<KeyValuePair<string, object>> SetGoal()
    {
        List<KeyValuePair<string, object>> goal = new List<KeyValuePair<string, object>>
        {
            new KeyValuePair<string, object>("findCarrot", true),
            // new KeyValuePair<string, object>("storeCarrot", true),
            // new KeyValuePair<string, object>("escapeDanger", true)
        };

        return goal;
    }
}
