using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    private FSM fSM;
    private FSM.I_FSMState idle;
    private FSM.I_FSMState moveTo;
    private FSM.I_FSMState doAction;

    private List<Action> possibleActions;
    private Queue<Action> plannedActions;

    private I_GOAP iGoap;

    private Planner actionPlanner;

    // Start is called before the first frame update
    void Start()
    {
        // initialization of FSM on world start, push the idle state on to the stack so planning begins
        fSM = new FSM();
        possibleActions = new List<Action>();
        plannedActions = new Queue<Action>();
        actionPlanner = new Planner();
        RetrieveI_GOAP();
        IdleState();
        MoveToState();
        DoActionState();
        fSM.Push(idle);
        LoadPossibleActions();
    }

    // Update is called once per frame
    void Update()
    {
        fSM.Update(this.gameObject);
    }

    // Adds an action to the list of possible actions
    public void AddPossibleAction(Action action)
    {
        possibleActions.Add(action);
    }

    // Removes an action from the list of possible actions
    public void RemovePossibleAction(Action action)
    {
        possibleActions.Remove(action);
    }

    // Returns true if there is at least one action planned
    private bool ActionPlanActive()
    {
        if(plannedActions.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Create the "Idle" state, planning happens here
    private void IdleState()
    {
        idle = (fSM, gameObject) =>
        {
            // Get the world state and goal via the interface
            Dictionary<string, object> stateofWorld = iGoap.RetrieveWorldState();
            Dictionary<string, object> goal = iGoap.SetGoal();

            
            Queue<Action> planResult = actionPlanner.Plan(gameObject, possibleActions, stateofWorld, goal);

            // If there are actions remove this state and push the state machine to the "Do Action" state
            if(planResult != null)
            {
                plannedActions = planResult;
                fSM.Pop();
                fSM.Push(doAction);
            }
            // Otherwise continue planning
            else
            {
                fSM.Pop();
                fSM.Push(idle);
            }
        };
    }

    // Create the "Move To" state
    private void MoveToState()
    {
        moveTo = (fSM, gameObject) =>
        {
            // Get the first action from the queue
            Action action = plannedActions.Peek();

            // -------INTERUPPTIONS CAUSING RE-PLANNING-------

            // If rabbit is detected move wandering waypoint to the Hunter's position to immediately pursue rabbit...(does not cause re-planning, finishes action early instead)
            if(action.gameObject.tag == "Hunter")
            {
                if(action.gameObject.GetComponent<Hunter>().rabbitDetected != null)
                {
                    action.gameObject.GetComponent<SearchForRabbit>().wayPoint.transform.position = action.gameObject.transform.position;
                }
            }

            // Rabbit reevaluates plan if hunter is near
            if (action.gameObject.tag == "Rabbit")
            {
                if (action.gameObject.GetComponent<Rabbit>().inDanger)
                {
                    fSM.Pop();
                    fSM.Push(idle);
                    action.gameObject.GetComponent<Rabbit>().inDanger = false;
                    action.gameObject.GetComponent<Rabbit>().processingInterruption = true;
                    action.gameObject.GetComponent<RetrieveCarrot>().cost = 3.0f;
                    action.gameObject.GetComponent<StoreCarrot>().cost = 3.0f;
                    return;
                }
            }

            // Hunter cannot continue to pursue rabbit if hidden, reevaluates plan given new costs of actions
            if (action.target.tag == "Hidden")
            {
                fSM.Pop();
                fSM.Push(idle);
                action.target.gameObject.GetComponent<Rabbit>().processingInterruption = false;
                action.gameObject.GetComponent<Hunter>().processingInterruption = true;
                action.gameObject.GetComponent<CaptureRabbit>().cost = 4.0f;
                return;
            }

            // Hunter evaluates from resting what is the best plan given the new costs of actions if rabbit is seen
            if (action.gameObject.tag == "Resting")
            {
                if (action.gameObject.GetComponent<Hunter>().rabbitSeen)
                {
                    fSM.Pop();
                    fSM.Push(idle);
                    action.gameObject.tag = "Hunter";
                    action.gameObject.GetComponent<Hunter>().processingInterruption = false;
                    action.gameObject.GetComponent<CaptureRabbit>().cost = 1.0f;
                    return;
                }
            }

            // Rabbit evaluates from hiding what is the best plan given the new costs of actions if the hunter has returned to their shed 
            if (action.gameObject.tag == "Hidden" && !action.gameObject.GetComponent<Rabbit>().inDanger && GameObject.FindGameObjectWithTag("Hunter") == null)
            {
                fSM.Pop();
                fSM.Push(idle);
                action.gameObject.tag = "Rabbit";
                action.gameObject.GetComponent<RetrieveCarrot>().cost = 1.0f;
                action.gameObject.GetComponent<StoreCarrot>().cost = 1.0f;
                return;
            }

            // If you have to be within a certain range for the action and the target of the action is null, reevaluate plan 
            if (action.target == null && action.IsRangeBased())
            {
                fSM.Pop();
                fSM.Pop();
                fSM.Push(idle);
            }

            // -------INTERUPPTIONS CAUSING RE-PLANNING END-------

            // If agent succesfully reached action location, pop off this state and move to "Do Action" state
            if (iGoap.IsActorAtTarget(action))
            {
                fSM.Pop();
            }
        };
    }

    // Create the "Do Action" state
    private void DoActionState()
    {
        doAction = (fSM, gameObject) =>
        {
            // If there are not actions go back to planning
            if(!ActionPlanActive())
            {
                fSM.Pop();
                fSM.Push(idle);
                return;
            }

            // Store the action in the front of the queue
            Action action = plannedActions.Peek();

            // If the action is completed, remove it from the queue of planned actions
            if(action.ActionCompleted())
            {
                plannedActions.Dequeue();
            }

            if(ActionPlanActive())
            {
                action = plannedActions.Peek();

                // Action is only "in range" if it is range based and the agent is within range of it
                bool inRange = action.IsRangeBased() ? action.WithinRange() : true; 

                if(inRange)
                {
                    bool isActionSuccessful = action.DoAction(gameObject);

                    // If the action is not successful go back to planning
                    if(!isActionSuccessful)
                    {
                        fSM.Pop();
                        fSM.Push(idle);
                        IdleState();
                    }
                }
                // if the agent is not "in range" of the action, push the "Move To" state to process before the "Do Action" state
                else
                {
                    fSM.Push(moveTo);
                }
            }
            // If there is no action plan remove this state and go to planning
            else
            {
                fSM.Pop();
                fSM.Push(idle);
            }
        };
    }

    // Get the interface
    private void RetrieveI_GOAP()
    {
        iGoap = gameObject.GetComponent<I_GOAP>();
    }

    // Get all actions attached to the game object
    private void LoadPossibleActions()
    {
        foreach(Action a in gameObject.GetComponents<Action>())
        {
            possibleActions.Add(a);
        }
    }
}
