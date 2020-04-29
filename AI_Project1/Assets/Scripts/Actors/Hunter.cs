using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : Actor
{
    [HideInInspector] public bool rabbitSeen = false;
    float hunterFieldOfVisionAngle = 80.0f;
    [HideInInspector] public GameObject rabbitDetected = null;

    LineRenderer fovLine1;
    LineRenderer fovLine2;
    LineRenderer raycastLine;
    public Material fovLineMat;
    public Material raycastLineMat;
    float lineLength;

    public void Start()
    {
        speed = 2.0f;

        SetupRaycastLine();
        SetupFOVLines();
        DrawDetectionCircle();
        detectionCircle.enabled = true;
    }

    private void SetupRaycastLine()
    {
        // Initialize empty child object and attach LineRenderer component, set needed values
        GameObject raycastLineChildObj = new GameObject();
        raycastLineChildObj.transform.parent = transform;
        raycastLineChildObj.transform.position = Vector3.zero;
        raycastLine = raycastLineChildObj.AddComponent<LineRenderer>();
        raycastLine.material = raycastLineMat;
        raycastLine.startWidth = 0.05f;
        raycastLine.endWidth = 0.05f;
        raycastLine.enabled = false;
    }

    private void SetupFOVLines()
    {
        // Initialize empty child objects and attach LineRenderer components, set needed values

        lineLength = gameObject.GetComponent<SphereCollider>().radius;

        GameObject l1ChildObj = new GameObject();
        l1ChildObj.transform.parent = transform;
        l1ChildObj.transform.position = Vector3.zero;

        GameObject l2ChildObj = new GameObject();
        l2ChildObj.transform.parent = transform;
        l2ChildObj.transform.position = Vector3.zero;

        fovLine1 = l1ChildObj.AddComponent<LineRenderer>();
        fovLine1.material = fovLineMat;
        fovLine1.startWidth = 0.05f;
        fovLine1.endWidth = 0.05f;

        fovLine2 = l2ChildObj.AddComponent<LineRenderer>();
        fovLine2.material = fovLineMat;
        fovLine2.startWidth = 0.05f;
        fovLine2.endWidth = 0.05f;
    }

    private void UpdateFOVLines()
    {
        // Update FOV lines so that they are correctly associated with hunter facing direction

        Vector3 line1StartPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.25f, gameObject.transform.position.z);

        fovLine1.SetPosition(0, line1StartPos);
        fovLine1.SetPosition(1, (Quaternion.AngleAxis(hunterFieldOfVisionAngle / 2.0f, Vector3.up) * gameObject.transform.forward) * lineLength + line1StartPos);

        Vector3 line2StartPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.25f, gameObject.transform.position.z);

        fovLine2.SetPosition(0, line2StartPos);
        fovLine2.SetPosition(1, (Quaternion.AngleAxis(-hunterFieldOfVisionAngle / 2.0f, Vector3.up) * gameObject.transform.forward) * lineLength + line2StartPos);
    }

    private void TurnHunter()
    {
        // Have the hunter face waypoints or the rabbit depending if it is detected or not

        if (!gameObject.GetComponent<SearchForRabbit>().ActionCompleted() && gameObject.GetComponent<SearchForRabbit>().wayPoint != null)
        {
            gameObject.transform.LookAt(gameObject.GetComponent<SearchForRabbit>().wayPoint.transform.position);
        }
        else if (rabbitDetected != null)
        {
            gameObject.transform.LookAt(rabbitDetected.transform.position);
        }
    }

    public void Update()
    {
        StickToTerrain();

        TurnHunter();

        UpdateFOVLines();

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

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Rabbit")
        {
            Vector3 rabbitDir = other.transform.position - gameObject.transform.position;
            float angle = Vector3.Angle(rabbitDir, gameObject.transform.forward);
            
            // Check if rabbit is within field of view
            if (angle < hunterFieldOfVisionAngle / 2.0f)
            {
                RaycastHit hit;

                Vector3 raycastOffset = gameObject.transform.position + new Vector3(0, 0.2f, 0); // default position is on the bottom of obj for accurate terrain traversal, raise it to get better hits


                // Check for unobstructed sight line
                if (Physics.Raycast(raycastOffset, rabbitDir.normalized, out hit, gameObject.GetComponent<SphereCollider>().radius))
                {
                    StartCoroutine(VisualizeRaycast(raycastOffset, hit.collider.transform.position));

                    if (hit.collider.tag == "Rabbit")
                    {
                        rabbitDetected = hit.collider.gameObject;
                    }
                }
            }
        }
        if (other.name == "SoundCollider")
        {
            // place the hunters wander waypoint at the location of the last heard sound
            gameObject.GetComponent<SearchForRabbit>().wayPoint.transform.position = other.gameObject.transform.position;
        }
    }

    IEnumerator VisualizeRaycast(Vector3 raycastOffset, Vector3 rabbitPos)
    {
        raycastLine.SetPosition(0, raycastOffset);
        raycastLine.SetPosition(1, rabbitPos);
        raycastLine.enabled = true;
        
        // Erase the visualization if the rabbit escaped to the rabbit sanctuary
        yield return new WaitUntil(() => GameObject.Find("Rabbit(Clone)").tag == "Hidden");

        raycastLine.enabled = false;
    }
}
