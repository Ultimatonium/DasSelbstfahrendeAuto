using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            //add checkpoint only once
            foreach (GameObject checkoints in other.gameObject.GetComponent<CarController>().checkpoints)
            {
                if (checkoints == gameObject)
                {
                    return;
                }
            }
            other.gameObject.GetComponent<CarController>().checkpoints.Add(gameObject);
            //Debug.Log("Checkpoint for Car " + other.gameObject.GetInstanceID());
        }
    }
}
