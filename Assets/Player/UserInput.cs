using UnityEngine;
using System.Collections;
using RTS;


public class UserInput : MonoBehaviour {
	private Player player;

	// Use this for initialization
	void Start () {
		player = transform.root.GetComponent< Player >();
	}
	
	// Update is called once per frame
	void Update () {
		if(player.human) {
		    MoveCamera();
		    MouseActivity();
		}
	}

	private void MouseActivity() {
	    if(Input.GetMouseButtonDown(0)) 
	    	LeftMouseClick();
	    else if(Input.GetMouseButtonDown(1)) 
	    	RightMouseClick();
	    MouseHover();
	}

	private void MoveCamera() {
 		float xpos = Input.mousePosition.x;
		float ypos = Input.mousePosition.y;
		Vector3 movement = new Vector3(0,0,0);

		//horizontal camera movement
		if(xpos >= 0 && xpos < ResourceManager.ScrollWidth) {
		    movement.x -= ResourceManager.ScrollSpeed;
		} else if(xpos <= Screen.width && xpos > Screen.width - ResourceManager.ScrollWidth) {
		    movement.x += ResourceManager.ScrollSpeed;
		}
		 
		//vertical camera movement
		if(ypos >= 0 && ypos < ResourceManager.ScrollWidth) {
		    movement.z -= ResourceManager.ScrollSpeed;
		} else if(ypos <= Screen.height && ypos > Screen.height - ResourceManager.ScrollWidth) {
		    movement.z += ResourceManager.ScrollSpeed;
		}

		//make sure movement is in the direction the camera is pointing
		//but ignore the vertical tilt of the camera to get sensible scrolling
		movement = Camera.main.transform.TransformDirection(movement);
		movement.y = 0;

		//calculate desired camera position based on received input
		Vector3 origin = Camera.main.transform.position;
		Vector3 destination = origin;
		destination.x += movement.x;
		destination.y += movement.y;
		destination.z += movement.z;

		//limit away from ground movement to be between a minimum and maximum distance
		if(destination.y > ResourceManager.MaxCameraHeight) {
		    destination.y = ResourceManager.MaxCameraHeight;
		} else if(destination.y < ResourceManager.MinCameraHeight) {
		    destination.y = ResourceManager.MinCameraHeight;
		}

		//if a change in position is detected perform the necessary update
		if(destination != origin) {
		    Camera.main.transform.position = Vector3.MoveTowards(origin, destination, Time.deltaTime * ResourceManager.ScrollSpeed);
		}
	}

	private void LeftMouseClick() {
		//Deselect();
	    if(player.hud.MouseInBounds()) {
	    	if(player.IsFindingBuildingLocation()) {
				if(player.CanPlaceBuilding()) player.StartConstruction();
			}
			else{
		        GameObject hitObject = WorkManager.FindHitObject(Input.mousePosition);
		        Vector3 hitPoint = WorkManager.FindHitPoint(Input.mousePosition);
		        if(hitObject && hitPoint != ResourceManager.InvalidPosition) {
		            if(player.SelectedObject){ 
		            	player.SelectedObject.MouseClick(hitObject, hitPoint, player);
		            }
		            WorldObject selected = player.SelectedObject;
		            Deselect();
		            if(hitObject.name!="Ground") {
		                WorldObject worldObject = hitObject.transform.parent.GetComponent< WorldObject >();
		                if(worldObject) {
		                    //we already know the player has no selected object
		                    player.SelectedObject = worldObject;
		                    worldObject.SetSelection(true, player.hud.GetPlayingArea());
		                }
		            }
		            else if (selected){
		            	player.SelectedObject=selected;
		            	player.SelectedObject.SetSelection(true, player.hud.GetPlayingArea());
		            }
		        }
	    	}
	    }
	}

	private void Deselect() {
	    if(player.hud.MouseInBounds() && player.SelectedObject) {
	        player.SelectedObject.SetSelection(false, player.hud.GetPlayingArea());
	        player.SelectedObject = null;
	    }
	}


	private void MouseHover() {
	    if(player.hud.MouseInBounds()) {
	        if(player.IsFindingBuildingLocation()) {
	            player.FindBuildingLocation();
	        }
    	}
	}

	private void RightMouseClick() {
	    if(player.hud.MouseInBounds() && !Input.GetKey(KeyCode.LeftAlt) && player.SelectedObject) {
	        if(player.IsFindingBuildingLocation()) {
	            player.CancelBuildingPlacement();
	        } else {
	            Deselect();
	        }
	    }
	}


}
