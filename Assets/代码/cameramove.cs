using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class CameraZoom : MonoBehaviour
{
    public List<CameraZone> cameraPositions; 
    
    [System.Serializable]
    public class CameraZone
    {
       
        public Vector3 enterPosition;
        public float enterZoom;
        public float minX, maxX;
        public float minY, maxY;
        public float minZoom, maxZoom;
    }
    public Camera cam;
    public CameraZone nowcameraindex;
    private Vector3 dragorigin;

    private void Start()
    {
        nowcameraindex = cameraPositions[0];
        cam.transform.position = nowcameraindex.enterPosition;
    }
    void Update()
    {
        camove();
        cazoom();
    }

    public void buttoncamera(int indexcam)
    {
        nowcameraindex = cameraPositions[indexcam];
        cam.transform.position = nowcameraindex.enterPosition;
    }
    public void camove()
    {
        if (Input.GetMouseButtonDown(1))
        {
            dragorigin = Input.mousePosition;
            return;
        }
        if (Input.GetMouseButton(1))
        {
            Vector3 diff = cam.ScreenToWorldPoint(dragorigin) - cam.ScreenToWorldPoint(Input.mousePosition);
            cam.transform.position += diff;
            dragorigin = Input.mousePosition;

            zoomnotgooutofrange();
        }
    }

    public void cazoom()
    {
        float scroll = Input.mouseScrollDelta.y;
        if (Mathf.Abs(scroll) > 0.01f)
        {
            cam.orthographicSize -= scroll ;
            zoomnotgooutofrange();
        }
    }

    public void zoomnotgooutofrange()
    {
        
        cam.orthographicSize = Mathf.Clamp
        (
            cam.orthographicSize,
            nowcameraindex.minZoom,
            nowcameraindex.maxZoom
        );

        
        float halfHeight = cam.orthographicSize;
        float halfWidth = cam.orthographicSize * cam.aspect;
        
        float minX = nowcameraindex.minX + halfWidth;
        float maxX = nowcameraindex.maxX - halfWidth;
        float minY = nowcameraindex.minY + halfHeight;
        float maxY = nowcameraindex.maxY - halfHeight;

        
        Vector3 pos = cam.transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        cam.transform.position = pos;
    }



}
