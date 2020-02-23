using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface GOAP_Interface 
{
    Dictionary<string, object> RetrieveWorldState();

    Dictionary<string, object> SetGoal();

    bool IsAgentAtTarget(Action followingAction);
}
