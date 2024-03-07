using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private enum Mode
    {
        Camera,
        CameraInverted,
        Forward,
        ForwardInverted
    }

    [SerializeField] private Mode mode;

    private void LateUpdate()
    {
        switch (mode)
        {
            case Mode.Camera:
                transform.LookAt(Camera.main.transform);
                break;
            case Mode.CameraInverted:
                Vector3 dirFromCam = transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position + dirFromCam);
                break;
            case Mode.Forward:
                transform.forward = Camera.main.transform.forward;
                break;
            case Mode.ForwardInverted:
                transform.forward = -Camera.main.transform.forward;
                break;
        }
    }
}
