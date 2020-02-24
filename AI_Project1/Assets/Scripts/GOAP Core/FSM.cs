using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM 
{
    private Stack<FSMState> states = new Stack<FSMState>();

    public delegate void FSMState(FSM fSM, GameObject obj);

    // Update is called once per frame
    public void Update(GameObject obj)
    {
        if (states.Peek() != null)
        {
            states.Peek().Invoke(this, obj);
        }
    }

    public void Push(FSMState state)
    {
        states.Push(state);
    }

    public void Pop()
    {
        states.Pop();
    }

}
