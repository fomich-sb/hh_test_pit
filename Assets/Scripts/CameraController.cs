using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Zenject;

public class CameraController : MonoBehaviour
{
    [SerializeField] private int MinWidth;
    [SerializeField] private int MinHeight;

    private void Update()
    {
        FitCameraToArea();
    }

    private void FitCameraToArea()
    {
        Camera cam = GetComponent<Camera>();

        float targetAspect = (float)MinWidth / MinHeight;
        float screenAspect = (float)Screen.width / Screen.height;

        float size;
        if (targetAspect > screenAspect)
        {
            size = MinWidth / (2 * screenAspect);
        }
        else
        {
            size = MinHeight / 2;
        }

        cam.orthographicSize = size;
    }
}
