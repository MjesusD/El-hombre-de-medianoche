using UnityEngine;

[ExecuteAlways]
public class Background : MonoBehaviour
{
    public Transform cameraTransform;

    void OnPreCull()
    {
        if (cameraTransform != null)
        {
            transform.position = new Vector3(cameraTransform.position.x, cameraTransform.position.y, transform.position.z);
        }
    }
}
