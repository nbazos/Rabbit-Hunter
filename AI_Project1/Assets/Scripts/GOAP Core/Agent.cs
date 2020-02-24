using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    private FSM fSM;

    private FSM.FSMState idle;
    private FSM.FSMState moveTo;
    private FSM.FSMState doAction;

    private List<Action> possibleActions;
    private Queue<Action> plannedActions;

    private GOAP_Interface iGoap;

    private Planner actionPlanner;

    // Start is called before the first frame update
    void Start()
    {
        fSM = new FSM();
        possibleActions = new List<Action>();
        plannedActions = new Queue<Action>();
        actionPlanner = new Planner();
        RetrieveInfoBridge();
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

    public void AddPossibleAction(Action action)
    {
        possibleActions.Add(action);
    }

    public void RemovePossibleAction(Action action)
    {
        possibleActions.Remove(action);
    }

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

    private void IdleState()
    {
        idle = (fSM, gameObject) =>
        {
            Dictionary<string, object> stateofWorld = iGoap.RetrieveWorldState();
            Dictionary<string, object> goal = iGoap.SetGoal();

            Queue<Action> plan = actionPlanner.Plan(gameObject, possibleActions, stateofWorld, goal);

            if(plan != null)
            {
                plannedActions = plan;
                fSM.Pop();
                fSM.Push(doAction);
            }
            else
            {
                fSM.Pop();
                fSM.Push(idle);
            }
        };
    }

    private void MoveToState()
    {
        moveTo = (fSM, gameObject) =>
        {
            Action action = plannedActions.Peek();

            // Rabbit reevaluates plan if hunter is near
            if (action.gameObject.tag == "Rabbit")
            {
                if (action.gameObject.GetComponent<Rabbit>().inDanger)
                {
                    fSM.Pop();
                    fSM.Push(idle);
                    action.gameObject.GetComponent<Rabbit>().inDanger = false;
                    action.gameObject.GetComponent<Rabbit>().processingNewPlan = true;
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
                action.target.gameObject.GetComponent<Rabbit>().processingNewPlan = false;
                action.gameObject.GetComponent<Hunter>().processingNewPlan = true;
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
                    action.gameObject.GetComponent<Hunter>().processingNewPlan = false;
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

            // If you have to be within a certain range for the action and the target of the action is null reevaluate plan 
            if (action.target == null && action.IsRangeBased())
            {
                fSM.Pop();
                fSM.Pop();
                fSM.Push(idle);
            }

            // If agent succesfully reached action location, pop off state and move to DoActionState
            if(iGoap.IsAgentAtTarget(action))
            {
                fSM.Pop();
            }
        };
    }

    private void DoActionState()
    {
        doAction = (fSM, gameObject) =>
        {
            if(!ActionPlanActive())
            {
                fSM.Pop();
                fSM.Push(idle);
                return;
            }

            Action action = plannedActions.Peek();

            if(action.ActionCompleted())
            {
                plannedActions.Dequeue();
            }

            if(ActionPlanActive())
            {
                action = plannedActions.Peek();

                bool inRange = action.IsRangeBased() ? action.WithinRange() : true; 

                if(inRange)
                {
                    bool isActionSuccessful = action.DoAction(gameObject);

                    if(!isActionSuccessful)
                    {
                        fSM.Pop();
                        fSM.Push(idle);
                        IdleState();
                    }
                }
                else
                {
                    fSM.Push(moveTo);
                }
            }
            else
            {
                fSM.Pop();
                fSM.Push(idle);
            }
        };
    }

    private void RetrieveInfoBridge()
    {
        iGoap = gameObject.GetComponent<GOAP_Interface>();
    }

    private void LoadPossibleActions()
    {
        foreach(Action a in gameObject.GetComponents<Action>())
        {
            possibleActions.Add(a);
        }
    }
}
