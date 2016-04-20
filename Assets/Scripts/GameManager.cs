using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    // Update is called once per frame
    public Plane groundPlane;
    public Transform markerObject;
    public static float planex, planey;

    void Start()
    {
        groundPlane = new Plane(new Vector3(0, 1, 0), new Vector3(0,0,0)); 
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float rayDistance;
            if (groundPlane.Raycast(ray, out rayDistance))
            {
                planex = ray.GetPoint(rayDistance).x;
                planey = ray.GetPoint(rayDistance).z;
            }
        }
    }
}
