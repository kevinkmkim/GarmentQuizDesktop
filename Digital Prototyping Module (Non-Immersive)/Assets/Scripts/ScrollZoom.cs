using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollZoom : MonoBehaviour
{
    [SerializeField]
    private float ScrollSpeed = 10f;
    private Camera ZoomCamera;
    public Vector3 startPosition;
    public Vector3 posDelta;

    private void Start() {
        ZoomCamera = Camera.main;
        startPosition = new Vector3(0,0,0);
    }
    void Update()
    {
        if (ZoomCamera.orthographic)
        {
            Vector3 camPos = Camera.main.transform.position;
            if (camPos[1] >= 0 && camPos[1] <= 2)
            {
                float scrollDelta = Input.GetAxis("Mouse ScrollWheel") * ScrollSpeed;
                posDelta = new Vector3(-scrollDelta * 1.2f, scrollDelta / 2, 0);
                if (camPos[1] + posDelta[1] >= 0 && camPos[1] + posDelta[1] <= 2)
                {
                    ZoomCamera.orthographicSize -= scrollDelta;
                    transform.position = camPos + posDelta;
                }
            }
        }
    }
}
