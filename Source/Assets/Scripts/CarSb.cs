using UnityEngine;

public class CarSb : MonoBehaviour
{
    public float speed;
    public float distanceCheck;
    public float avoideStrengh;
    public float tollerance;
    private Vector3 velocity = Vector3.zero;
    private int targetPoint = 0;
    private PointList pointList;

    private void Start()
    {
        pointList = FindObjectOfType<PointList>();
        FindObjectOfType<CameraHandler>().SetMainCameraOnObject(gameObject);
    }

    private void Update()
    {
        //move from point to point
        if (Vector3.Distance(transform.position, pointList.points[targetPoint].transform.position) < tollerance)
        {
            targetPoint++;
            if (targetPoint >= pointList.points.Length)
            {
                targetPoint = 0;
            }
        }
        Debug.DrawLine(transform.position, pointList.points[targetPoint].transform.position, Color.blue);
        velocity += SteeringBehavior.Seek(transform.position, pointList.points[targetPoint].transform.position, velocity, speed, GetComponent<Rigidbody>().mass);
        Debug.DrawRay(transform.position, velocity.normalized * distanceCheck, Color.red);
        velocity += SteeringBehavior.Avoid(gameObject, velocity, distanceCheck, avoideStrengh, LayerMask.GetMask("Obstacle"));
        velocity.Normalize();
        velocity *= speed;
        velocity *= Time.deltaTime;
        velocity = new Vector3(velocity.x, 0, velocity.z); //remove Y to bind on ground
        transform.position += velocity;
        transform.LookAt(transform.position + velocity);
    }
}
