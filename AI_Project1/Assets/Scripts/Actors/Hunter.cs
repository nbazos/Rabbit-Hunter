using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : Actor
{
    public override List<KeyValuePair<string, object>> SetGoal()
    {
        List<KeyValuePair<string, object>> goal = new List<KeyValuePair<string, object>>();

        goal.Add(new KeyValuePair<string, object>("killRabbit", true));
       
        return goal;
    }
}
