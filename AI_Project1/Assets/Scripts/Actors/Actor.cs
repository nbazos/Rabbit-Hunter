using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour, I_InfoBridge
{
    public abstract bool IsAgentAtTarget(Action followingAction);

    public abstract List<KeyValuePair<string, object>> RetrieveWorldState();

    public abstract List<KeyValuePair<string, object>> SetGoal();
}
