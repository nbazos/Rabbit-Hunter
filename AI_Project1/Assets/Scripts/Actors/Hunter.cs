using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : Actor
{
    [HideInInspector] public bool processingInterruption = false;
    [HideInInspector] public bool rabbitSeen = false;
    float hunterFieldOfVisionAngle = 80.0f;
    [HideInInspector] public GameObject rabbitDetected = null;

    public void Start()
    {
        speed = 1.0f;
    }

    public void Update()
    {
        StickToTerrain();

        // Accounting for interruptions in the "Move To" state depending if the rabbit is not hidden
        if (!processingInterruption)
        {
            if(GameObject.FindGameObjectWithTag("Rabbit") != null)
            {
                rabbitSeen = true;
            }
            else
            {
                rabbitSeen = false;
            }
        }

        //if (rabbitDetected == null /*&& rabbitSeen*/)
        //{
        //    Wander();
        //}

        // DrawLine(gameObject.transform.position, Quaternion.AngleAxis(40, Vector3.forward) * gameObject.transform.forward, gameObject.GetComponent<SphereCollider>().radius, Color.red);
    }

    // Set the goal of this specific actor
    public override Dictionary<string, object> SetGoal()
    {
        Dictionary<string, object> goal = new Dictionary<string, object>
        {
            { "huntRabbit", true }
        };

        return goal;
    }

    public IEnumerator WanderAndFindRabbit()
    {
        while(true)
        {
            Wander();

            if(rabbitDetected != null)
            {
                break;
            }

            // wait for the next frame
            yield return null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Rabbit" /*&& rabbitDetected == null*/)
        {
            Vector3 rabbitDir = other.transform.position - gameObject.transform.position;
            float angle = Vector3.Angle(rabbitDir, gameObject.transform.forward);

            if (angle < hunterFieldOfVisionAngle / 2.0f)
            {
                RaycastHit hit;

                if (Physics.Raycast(gameObject.transform.position + new Vector3(0, 0.1f, 0), rabbitDir.normalized, out hit, gameObject.GetComponent<SphereCollider>().radius))
                {
                    if (hit.collider.tag == "Rabbit")
                    {
                        rabbitDetected = hit.collider.gameObject;
                    }
                }
            }
        }
    }

    private void DrawLine(Vector3 startPos, Vector3 dir, float radius, Color color, float duration = 0.2f)
    {
        GameObject sightLine = new GameObject();
        sightLine.transform.position = startPos;
        sightLine.AddComponent<LineRenderer>();
        LineRenderer lineRenderer = sightLine.GetComponent<LineRenderer>();

        // lineRenderer.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lineRenderer.SetColors(color, color);
        lineRenderer.SetWidth(0.5f, 0.5f);
        lineRenderer.useWorldSpace = false;
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, dir * radius + startPos);

        GameObject.Destroy(sightLine, duration);
    }
}
