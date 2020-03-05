using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    private void Start()
    {
        //transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
    }

    void LateUpdate()
    {
        //Debug.Log(target.position);
        //Vector3 desiredPosition = target.position + offset;
        //Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        //Camera.main.transform.position = smoothedPosition;
        transform.position = new Vector3(target.position.x, 0, transform.position.z);
        //Debug.Log("Moving");
    }
}