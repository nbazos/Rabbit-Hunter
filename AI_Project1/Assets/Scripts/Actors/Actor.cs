using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour, I_InfoBridge
{
    public float speed = 1.0f;

    public bool IsAgentAtTarget(Action followingAction)
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, followingAction.target.transform.position, speed * Time.deltaTime);

        if (gameObject.transform.position == followingAction.target.transform.position)
        {
            followingAction.setInRange(true);
            return true;
        }
        else
        {
            return false;
        }
    }

    public List<KeyValuePair<string, object>> RetrieveWorldState()
    {
        List<KeyValuePair<string, object>> worldData = new List<KeyValuePair<string, object>>();

        worldData.Add(new KeyValuePair<string, object>("killRabbit", false));

        return worldData;
    }

    public abstract List<KeyValuePair<string, object>> SetGoal();
}
