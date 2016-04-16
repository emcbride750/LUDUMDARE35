using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public Transform target;
    public float damping = 0.2f;

    public float m_OffsetZ;
    private Vector3 m_LastTargetPosition;
    private Vector3 m_CurrentVelocity;

    // Use this for initialization
    private void Start()
    {
        m_LastTargetPosition = target.position;
        m_OffsetZ = (transform.position - target.position).z;
        transform.parent = null;
    }


    // Update is called once per frame
    private void Update()
    {

        Vector3 aheadTargetPos = new Vector3(target.position.x, target.position.y, 0);
        Vector3 currentPos = new Vector3(transform.position.x, transform.position.y, 0);
        Vector3 midPos = Vector3.SmoothDamp(currentPos, aheadTargetPos, ref m_CurrentVelocity, damping);


        transform.position = midPos + (Vector3.forward * m_OffsetZ);

        m_LastTargetPosition = target.position;
    }
}
