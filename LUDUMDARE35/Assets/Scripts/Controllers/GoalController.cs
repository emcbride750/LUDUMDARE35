using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class GoalController : MonoBehaviour
{

    public ScoringObject.goalState StateToSet = ScoringObject.goalState.OUTSIDE;

    void Awake()
    {
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is csalled once per frame
    void Update()
    {

    }

    //When colliding...
    void OnTriggerEnter2D(Collider2D coll)
    {
        //Are we colliding with a pixel
        ScoringObject scoringObject = coll.gameObject.GetComponentInChildren<ScoringObject>();

        //Was there one?
        if (scoringObject)
        {
            //We need to tell it that it is part of the goal
            scoringObject.pushState(StateToSet);
        }
    }

    //When leaving...
    void OnTriggerExit2D(Collider2D coll)
    {

        //Are we colliding with a pixel
        ScoringObject scoringObject = coll.gameObject.GetComponentInChildren<ScoringObject>();

        //Was there one?
        if (scoringObject)
        {
            //We need to tell it that it is part of the goal
            scoringObject.popState();

        }

    }
}
