using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour {
	//
	// VARIABLES
	//
	
	public float panSpeed = 0.1f;		// Speed of the camera when being panned
	public float zoomSpeed = 4.0f;		// Speed of the camera going back and forth
	
	private Vector3 mouseOrigin;	// Position of cursor when mouse dragging starts
	private bool isPanning;		// Is the camera being panned?
	private bool isRotating;	// Is the camera being rotated?
	private bool isZooming;		// Is the camera zooming?
	
	//
	// UPDATE
	//
	
	void Update () 
	{
		
		// Disable movements on button release
		if (!Input.GetMouseButton(2)) isZooming=false;
		
		
		// Move the camera on it's XY plane
		if (!isZooming)
		{
	        	Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);
	        	Vector3 move = new Vector3(0, 0, 0); 
	        	if (pos.x <= 0.01f || pos.x >= 0.99f || pos.y <= 0.01f || pos.y >= 0.99f){
		        	move += new Vector3((pos.x-0.5f)*panSpeed, 0, (pos.y-0.5f)*panSpeed);
		        }

		        transform.Translate(move, Space.World);
		}
		
		// Move the camera linearly along Z axis
		if (isZooming)
		{
	        	Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);

	        	Vector3 move = pos.y * zoomSpeed * transform.forward; 
	        	transform.Translate(move, Space.World);
		}
	}
}
