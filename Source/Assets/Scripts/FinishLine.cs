using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            CarController carController = other.gameObject.GetComponent<CarController>();
            if (carController == null)
            {
                Debug.LogError("carController is null of object " + other.gameObject.name);
                return;
            }
            if (carController.checkpoints.Count != 11)
            {
                Debug.LogWarning(carController.gameObject.name + " " + carController.gameObject.GetInstanceID() + " is a cheater");
                carController.cheater = true;
            }
            //Debug.Log("Finishline for Car " + other.gameObject.GetInstanceID());
            carController.laps++;
            carController.checkpoints.Clear();

            float lapTime = Time.time - carController.startTime;
            carController.totalDuration += lapTime;
            carController.startTime = Time.time;
            Debug.Log(carController.gameObject.name + " " + carController.gameObject.GetInstanceID() + " " + lapTime);
        }
    }
}
