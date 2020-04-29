using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour, I_GOAP
{
    // Speed of actor
    [HideInInspector] public float speed = 1.0f;

    [HideInInspector] public LineRenderer detectionCircle;
    public Material lineMat;

    [HideInInspector] public bool processingInterruption = false;

    // Move actor to an action's target 
    public bool IsActorAtTarget(Action followingAction)
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, followingAction.target.transform.position, speed * Time.deltaTime);

        // Close enough to do the action
        if (Vector3.Distance(gameObject.transform.position, followingAction.target.transform.position) < 0.12f)
        {
            followingAction.SetInRange(true);
            return true;
        }
        else
        {
            return false;
        }
    }

    // Reference: Use Unity's LineRenderer to draw a circle on a GameObject by Loek van den Ouweland
    public void DrawDetectionCircle()
    {
        detectionCircle = gameObject.AddComponent<LineRenderer>();
        detectionCircle.material = lineMat;
        detectionCircle.useWorldSpace = false;

        int segments = 360;
        detectionCircle.startWidth = 0.02f;
        detectionCircle.endWidth = 0.02f;
        detectionCircle.positionCount = segments + 1;

        int pointCount = segments + 1;
        Vector3[] points = new Vector3[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            float rad = Mathf.Deg2Rad * (i * 360f / segments);

            // Rabbit prefab needs to be handled differently as the detection circle is for its sound collider
            if (gameObject.tag == "Rabbit")
            {
                points[i] = new Vector3(Mathf.Sin(rad) * gameObject.transform.GetChild(1).GetComponent<SphereCollider>().radius, gameObject.transform.position.y, Mathf.Cos(rad) * gameObject.transform.GetChild(1).GetComponent<SphereCollider>().radius);
            }
            else
            {
                points[i] = new Vector3(Mathf.Sin(rad) * gameObject.GetComponent<SphereCollider>().radius, gameObject.transform.position.y-0.5f, Mathf.Cos(rad) * gameObject.GetComponent<SphereCollider>().radius);
            }
        }

        detectionCircle.SetPositions(points);

        detectionCircle.enabled = false;
    }

    public void StickToTerrain()
    {
        // Set the correct y for the actor traversing the terrain
        Vector3 tempPos = gameObject.transform.position;
        tempPos.y = Terrain.activeTerrain.SampleHeight(new Vector3(tempPos.x, 0, tempPos.z));
        gameObject.transform.position = tempPos;
    }

    public Vector3 CreateWanderPoint()
    {
        // Place wander points within the area of the terrain
        Vector3 wayPoint = Random.insideUnitSphere * Terrain.activeTerrain.terrainData.size.x/2;
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
