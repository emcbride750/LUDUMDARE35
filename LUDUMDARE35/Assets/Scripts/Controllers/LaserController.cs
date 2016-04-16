using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class LaserController : MonoBehaviour {

    void OnCollisionEnter2D(Collision2D coll)
    {
        PixelCollisionHandler aPixel = coll.gameObject.GetComponent<PixelCollisionHandler>();
        if (aPixel != null)
        {
            Destroy(aPixel);
        }
    }
}
