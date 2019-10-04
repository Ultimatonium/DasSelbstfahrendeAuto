using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    private static Vector3 mainCameraInitialPosition;
    private static Quaternion mainCameraInitialRotation;
    private static bool mainCameraInitialOrthographic;

    private void Start()
    {
        mainCameraInitialPosition = Camera.main.transform.position;
        mainCameraInitialRotation = Camera.main.transform.rotation;
        mainCameraInitialOrthographic = Camera.main.orthographic;
    }

    public void ResetMainCamera()
    {
        Camera.main.transform.parent = null;
        Camera.main.transform.position = mainCameraInitialPosition;
        Camera.main.transform.rotation = mainCameraInitialRotation;
        Camera.main.orthographic = mainCameraInitialOrthographic;
    }

    public void SetMainCameraOnObject(GameObject obj)
    {
        CameraSettings cameraSettings = obj.GetComponent<CameraSettings>();
        if (cameraSettings == null)
        {
            Debug.LogError("Camera Settings missing on" + obj.name + " " + obj.GetInstanceID());
        }
        Camera.main.transform.parent = obj.transform;
        Camera.main.transform.localPosition = cameraSettings.position;
        Camera.main.transform.localRotation = cameraSettings.rotation;
        Camera.main.orthographic = cameraSettings.orthographic;
    }
}
