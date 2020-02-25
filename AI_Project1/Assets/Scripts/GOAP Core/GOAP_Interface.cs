using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interface to communicate data between actors and planning/state machine, all actors must implement this
public interface I_GOAP 
{
    Dictionary<string, object> RetrieveWorldState();

    Dictionary<string, object> SetGoal();

    bool IsActorAtTarget(Action actionToOccur);
}
