using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public Camera m_OrthographicCamera;

    public float m_Zoom;

    public float minSize = 1f;
    public float maxSize = 20f;
    public float scrollSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
        float newSize = m_OrthographicCamera.orthographicSize - scrollDelta * scrollSpeed;

        newSize = Mathf.Clamp(newSize, minSize, maxSize);

        m_OrthographicCamera.orthographicSize = newSize;

    }
}
