using UnityEngine;

public class SbExecutor : MonoBehaviour
{
    [SerializeField]
    public GameObject carPrefab;
    [SerializeField]
    public GameObject startPosition;

    public void StartCar()
    {
        GameObject newCar = Instantiate(carPrefab, startPosition.transform.position, startPosition.transform.rotation);
        newCar.GetComponent<CarController>().enabled = false;
        newCar.GetComponent<CarSb>().enabled = true;
        newCar.SetActive(true);
    }
}
