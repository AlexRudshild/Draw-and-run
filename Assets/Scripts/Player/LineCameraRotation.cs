using UnityEngine;

public class LineCameraRotation : MonoBehaviour
{
    public bool OnlyOnce = false;

    private Transform _mainCamera;

    void Start()
    {
        _mainCamera = Camera.main.transform;

        transform.rotation = _mainCamera.rotation;

        if (OnlyOnce)
            Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = _mainCamera.rotation;
    }
}
