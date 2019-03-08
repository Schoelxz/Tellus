
using UnityEngine;

public class OverseerCameraController : MonoBehaviour
{
    private Vector3 mouseOriginPoint;
    private Vector3 offset;
    private bool dragging;

    private Camera cam;

    // TODO : Confine navigational space

    private void Awake()
    {
        cam = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize -= Input.GetAxis("Mouse ScrollWheel")
            * (Camera.main.orthographicSize), 2.5f, 75f);

        if (Input.GetMouseButton(2))
        {
            offset = (cam.ScreenToWorldPoint(Input.mousePosition) - transform.position);
            if (!dragging)
            {
                dragging = true;
                mouseOriginPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        else
        {
            dragging = false;
        }
        if (dragging)
            transform.position = mouseOriginPoint - offset;

        Debug.DrawLine(cam.transform.position, CameraRaycastPosition(), Color.red);
    }

    public Vector3 CameraRaycastPosition()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if(Physics.Raycast(ray, out hitInfo))
        {
           
            return hitInfo.point;
        }

        return Vector3.one;

    }
}
