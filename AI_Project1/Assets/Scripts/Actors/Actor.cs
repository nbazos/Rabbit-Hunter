using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour, I_GOAP
{
    // Speed of actor
    public float speed = 1.0f;
    Vector3 wayPoint;
    [HideInInspector] public bool wandering;

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

    public void StickToTerrain()
    {
        Vector3 tempPos = gameObject.transform.position;
        tempPos.y = Terrain.activeTerrain.SampleHeight(new Vector3(tempPos.x, 0, tempPos.z));
        gameObject.transform.position = tempPos;
    }

    public Vector3 CreateWanderPoint()
    {
        wayPoint = Random.insideUnitSphere * Terrain.activeTerrain.terrainData.size.x/2;
        wayPoint.y = Terrain.activeTerrain.SampleHeight(new Vector3(wayPoint.x, 0, wayPoint.z));

        return wayPoint;
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
