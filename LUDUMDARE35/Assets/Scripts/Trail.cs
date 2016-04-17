using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TrailRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Trail : MonoBehaviour
{
    private TrailRenderer tr;
    Rigidbody2D rd;
    private float startTime;
    private static float maxSpeed = 40.0f;
    // Use this for initialization
    void Start()
    {
        tr = this.GetComponent<TrailRenderer>();
        rd = this.gameObject.GetComponent<Rigidbody2D>();
        if (tr != null)
        {
            tr.sortingOrder = -100;
            startTime = tr.time;
        }
    }

    // Update is called once per frame
    /*void Update()
    {
        if ((tr != null) && (rd != null))
        {
            float speed = rd.velocity.magnitude;
            //tr.time = Mathf.Lerp(0, startTime, Mathf.Min(maxSpeed, speed / maxSpeed));
                tr.enabled = (speed > maxSpeed);
        }
    }*/
}
