using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class GoalController : MonoBehaviour
{

    public ScoringObject.goalState StateToSet = ScoringObject.goalState.OUTSIDE;

    void Awake()
    {
    }

    // Use this for initialization
    void Start()
    {
        //create doorway automatically
        if (this.StateToSet == ScoringObject.goalState.INSIDE)
        {
            //create frame
            GameObject doorFrame = Instantiate(this.gameObject);
            doorFrame.GetComponent<GoalController>().StateToSet = ScoringObject.goalState.DOORFRAME;
            doorFrame.transform.localScale = this.transform.localScale * 1.3f;
            doorFrame.GetComponent<SpriteRenderer>().enabled = false;
            //create reset frame
            GameObject resetFrame = Instantiate(doorFrame);
            resetFrame.GetComponent<GoalController>().StateToSet = ScoringObject.goalState.OUTSIDE;
            resetFrame.transform.localScale = doorFrame.transform.localScale * 1.3f;
            resetFrame.GetComponent<SpriteRenderer>().enabled = false;

        }
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
