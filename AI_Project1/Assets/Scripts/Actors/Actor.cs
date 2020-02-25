using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour, I_GOAP
{
    // Speed of actor
    public float speed = 1.0f;

    // Move actor to an action's target 
    public bool IsActorAtTarget(Action followingAction)
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, followingAction.target.transform.position, speed * Time.deltaTime);

        // Close enough to do the action
        if (Vector3.Distance(gameObject.transform.position, followingAction.target.transform.position) < 0.1f)
        {
            followingAction.SetInRange(true);
            return true;
        }
        else
        {
            return false;
        }
    }

    // Track relevant data elements of the world
    public Dictionary<string, object> RetrieveWorldState()
    {
        Dictionary<string, object> worldData = new Dictionary<string, object>
        {
            { "huntRabbit", false },
            { "hasCarrot", false }
        };

        return worldData;
    }

    // Child classes will set their respective goals
    public abstract Dictionary<string, object> SetGoal();
}
