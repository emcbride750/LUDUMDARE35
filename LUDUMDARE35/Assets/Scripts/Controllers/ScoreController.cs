using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public interface IScoreEvent : IEventSystemHandler
{
    void AddPoint(ScoringObject.goalState gs);
    void RemovePoint(ScoringObject.goalState gs);
}

public class ScoreController : MonoBehaviour, IScoreEvent
{
    //Sprites
    public Texture goodSprite;
    public Texture badSprite;

    //Track global counts
    private Dictionary<ScoringObject.goalState, int> counts = new Dictionary<ScoringObject.goalState, int>();

    public void ModifyPoint(ScoringObject.goalState gs, int mod)
    {
        if (this.counts != null)
        {
            if (!this.counts.ContainsKey(gs))
            {
                this.counts.Add(gs, 0);
            }
            this.counts[gs] = this.counts[gs] + mod;
        }
        //update image
        int winCount = WinningCount;
        var targetSprite = badSprite;
        if (winCount > 0)
        {
            targetSprite = goodSprite;
        }
        GameObject.Find("ScoreGood").GetComponent<RawImage>().texture = targetSprite;
        //update score text
        GameObject.Find("ScoreText").GetComponent<Text>().text = "Score: " + winCount +". Good: "+ this.counts[ScoringObject.goalState.INSIDE] + ". Bad: " + this.counts[ScoringObject.goalState.DOORFRAME];
    }

    public void AddPoint(ScoringObject.goalState gs)
    {
        ModifyPoint(gs, 1);
    }
    public void RemovePoint(ScoringObject.goalState gs)
    {
        ModifyPoint(gs, -1);
    }

    public int WinningCount
    {
        get
        {
            if (this.counts != null)
            {
                //can't win if any are in the doorframe
                int frame = 0;
                if (this.counts.ContainsKey(ScoringObject.goalState.DOORFRAME))
                {
                    frame = this.counts[ScoringObject.goalState.DOORFRAME];
                }
                int inside = 0;

                if (this.counts.ContainsKey(ScoringObject.goalState.INSIDE))
                {
                    inside = this.counts[ScoringObject.goalState.INSIDE];
                }

                if (frame > inside)
                {
                    return 0;
                } else
                {
                    return inside;
                }
            } else
            {
                return 0;
            }
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
