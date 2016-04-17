using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour {

    public Transform target;
    public float damping = 0.2f;

    //private float m_OffsetZ;
    private float m_Size;
    private Camera cam;
    //public float zoomSpeed = 0.2f;
    //public float maxZoomOutRelative = 1.05f;
    private Vector3 m_CurrentVelocity;
    //private float maxAxisLookAhead = 5.0f;
    // Use this for initialization
    private void Start()
    {
        //m_OffsetZ = (transform.position - target.position).z;
        m_Size = this.cam.orthographicSize;
        //transform.parent = null;
    }

    private void Awake()
    {
        this.cam = this.GetComponent<Camera>();
        cam.orthographic = true;
    }

    private static float velocityDamp(float v)
    {
        if (v >= 0.0f)
        {
            return Mathf.Sqrt(Mathf.Abs(v));
        }
        else
        {
            return -1.0f * Mathf.Sqrt(Mathf.Abs(v));
        }
    }

    // Update is called once per frame
    private void Update()
    {
        Vector2 velocity = target.GetComponent<Rigidbody2D>().velocity;
        float vDamp = velocityDamp(velocity.magnitude);
        Vector3 aheadTargetPos = new Vector3(target.position.x + velocityDamp(velocity.x), target.position.y + velocityDamp(velocity.y), 0);
        Vector3 currentPos = new Vector3(transform.position.x, transform.position.y, 0);
        Vector3 midPos = Vector3.SmoothDamp(currentPos, aheadTargetPos, ref m_CurrentVelocity, damping);
        
        //float zoomOut = Mathf.Max(m_Size + m_Size * vDamp * zoomSpeed, maxZoomOutRelative);


        //float zoom = Mathf.Lerp(m_Size, zoomOut, Time.time);
        //this.cam.orthographicSize = zoom;


        transform.position = midPos + (transform.position.z * Vector3.forward);
    }
}
