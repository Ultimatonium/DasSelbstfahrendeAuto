using UnityEngine;
using SharpNeat.Phenomes;
using System.Collections.Generic;

public class CarController : UnitController
{
    [HideInInspector]
    public List<GameObject> checkpoints = new List<GameObject>();
    [HideInInspector]
    public int laps = 0;
    [HideInInspector]
    public bool cheater = false;
    [HideInInspector]
    public float totalDuration;
    [HideInInspector]
    public float startTime;
    private float totalDistance = 0;

    private Optimizer optimizer;
    private Rigidbody rigidbody;
    private bool IsRunning;
    private IBlackBox blackBox;
    private const float rayDistance = 10;
    private float turningCircle;
    private float maxAcceleration;


    private void Start()
    {
        optimizer = FindObjectOfType<Optimizer>();
        rigidbody = GetComponent<Rigidbody>();
        startTime = Time.time;
        turningCircle = 200;
        maxAcceleration = 20;
    }
    void FixedUpdate()
    {
        if (IsRunning)
        {
            ISignalArray inputSignal = blackBox.InputSignalArray;
            inputSignal[0] = GetRayDistanceNormalized(Quaternion.Euler(0, 0, 0));
            inputSignal[1] = GetRayDistanceNormalized(Quaternion.Euler(0, -45, 0));
            inputSignal[2] = GetRayDistanceNormalized(Quaternion.Euler(0, 45, 0));
            inputSignal[3] = rigidbody.velocity.magnitude;

            blackBox.Activate();

            ISignalArray outputSignal = blackBox.OutputSignalArray;
            float steer = (float)outputSignal[0] * 2 - 1;
            float gas = (float)outputSignal[1] * 2 - 1;

            float acceleration = gas * maxAcceleration;
            totalDistance += acceleration;
            transform.Rotate(new Vector3(0, steer * turningCircle * Time.fixedDeltaTime, 0));
            rigidbody.AddForce(transform.forward * acceleration * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
    }

    public override void Stop()
    {
        IsRunning = false;
    }

    public override void Activate(IBlackBox box)
    {
        blackBox = box;
        IsRunning = true;
    }

    public override float GetFitness()
    {
        if (cheater)
        {
            return 0;
        }

        float totalFitness;
        float checkpointFitness = ((laps * 12) + checkpoints.Count) * maxAcceleration * 10000; //phase 1
        float fitnessDistance = totalDistance * 1.0f; //phase 2
        float timeFitness = TimeBonus(); // phase 3
        //Debug.Log(fitnessDistance + "¦" + checkpointFitness + "¦" + timeFitness);
        totalFitness = fitnessDistance + checkpointFitness + timeFitness;
        if (totalFitness > 0)
        {
            return totalFitness;
        }
        return 0;
    }

    private float TimeBonus()
    {
        if (laps == 0) return 0;
        return optimizer.TrialDuration - totalDuration;
    }

    private float GetRayDistanceNormalized(Quaternion delta)
    {
        RaycastHit hit;
        Vector3 direction = delta * transform.forward;
        float distance = rayDistance;
        Debug.DrawRay(transform.position, direction, Color.red);
        if (Physics.Raycast(transform.position, direction, out hit, rayDistance, LayerMask.GetMask("Obstacle")))
        {
            distance = Vector3.Distance(transform.position, hit.point);
        }
        distance /= rayDistance;
        //Debug.Log("Ray " + delta + ":" + distance);
        return distance;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor")) return;
        //Debug.Log("Collided with " + collision.gameObject.name);
        if (rigidbody == null) return; //hack for collision is enabled even script is deactivated
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        IsRunning = false;
    }
}
