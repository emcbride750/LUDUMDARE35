﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.EventSystems;



public class ScoringObject : MonoBehaviour {

    public enum goalState { OUTSIDE, INSIDE, DOORFRAME };

    private Stack<goalState> goalStack = new Stack<goalState>();

    public ScoreController sc;

    //Is it in the goal?
    public goalState State
    {
        get
        {
            if ((goalStack == null) || (goalStack.Count == 0))
            {
                return goalState.OUTSIDE;
            } else
            {
                return goalStack.Peek();
            }
        }
    }

    public void pushState(goalState gs)
    {
        if (goalStack != null)
        {
            if (gs != goalState.OUTSIDE)
            {
                goalStack.Push(gs);
                if (sc != null)
                {
                    ExecuteEvents.Execute<IScoreEvent>(sc.gameObject, null, (x, y) => x.AddPoint(gs));
                }
            } else
            {
                //empty stack
                while (true)
                {
                    try
                    {
                        this.popState();
                    }
                    catch
                    {
                        break;
                    }
                }
            }
        }
    }
    public goalState popState()
    {
        if ((goalStack != null) && (goalStack.Count > 0))
        {
            var gs = goalStack.Pop();
            if (sc != null)
            {
                ExecuteEvents.Execute<IScoreEvent>(sc.gameObject, null, (x, y) => x.RemovePoint(gs));
            }
            return gs;
        } else
        {
            throw new Exception("stack empty");
        }
    }

    public void OnDestroy()
    {
        while (true)
        {
            try {
                this.popState();
            } catch
            {
                break;
            }
        }
    }

    // Use this for initialization
    void Start () {
        if (sc == null)
        {
            try
            {
                sc = FindObjectOfType(typeof(ScoreController)) as ScoreController;
                //GameObject.Find("ScoreController").GetComponent<ScoreController>();
            }
            catch
            {
                print("can't find scorecontroller");
                throw;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
