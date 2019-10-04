using UnityEngine;

public class SteeringBehavior
{
    public static Vector3 Seek(Vector3 source, Vector3 target, Vector3 currentVelocity, float speed, float mass)
    {
        Vector3 directionToTarget = target - source;
        directionToTarget = directionToTarget.normalized * speed;
        Vector3 steering = directionToTarget - currentVelocity;
        steering = steering / mass;
        steering.Normalize();
        return steering;
    }

    public static Vector3 Pursue(Vector3 source, Vector3 target, Vector3 targetVelocity, Vector3 currentVelocity, float speed, float mass, float T)
    {
        float dynamic_T = Vector3.Distance(target, source) * T;
        Vector3 futurePosition = target + targetVelocity * dynamic_T;
        Debug.DrawLine(source, futurePosition, Color.red);
        return Seek(source, futurePosition, currentVelocity, speed, mass);
    }

    public static Vector3 Avoid(GameObject source, Vector3 currentVelocity, float distanceObstacleCheck, float avoidStrength, int layerMask)
    {
        RaycastHit hit;
        if (Physics.Raycast(source.transform.position, currentVelocity.normalized, out hit, distanceObstacleCheck, layerMask))
        {
            Vector3 avoidForce = hit.point - hit.collider.gameObject.transform.position;
            currentVelocity = avoidForce.normalized * avoidStrength;
        }
        return currentVelocity;
    }
}
