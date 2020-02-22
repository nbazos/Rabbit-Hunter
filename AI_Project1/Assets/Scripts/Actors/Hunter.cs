using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : Actor
{
    public override Dictionary<string, object> SetGoal()
    {
        Dictionary<string, object> goal = new Dictionary<string, object>
        {
            { "killRabbit", true }
        };

        return goal;
    }
}
