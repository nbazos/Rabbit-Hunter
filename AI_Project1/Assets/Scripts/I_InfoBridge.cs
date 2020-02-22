using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_InfoBridge 
{
    // List<KeyValuePair<string, object>> RetrieveWorldState();
    Dictionary<string, object> RetrieveWorldState();

    // List<KeyValuePair<string, object>> SetGoal();
    Dictionary<string, object> SetGoal();

    //void PlanInvalid(List<KeyValuePair<string, object>> unsuccesfulGoal);

    //void PlanSuccess(List<KeyValuePair<string, object>> goal, Queue<Action> actionPlan);

    //void AllActionsPerformed();

    //void PlanTermination(Action terminatingAction);

    bool IsAgentAtTarget(Action followingAction);
}
