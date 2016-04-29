using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RTS;
using System;

public class Unit : WorldObject {
	protected bool moving, rotating;
	 
	private Vector3 destination;
	private Quaternion targetRotation;
	private GameObject destinationTarget;
    private Queue<Vector3> path = new Queue<Vector3>();
	public float moveSpeed, rotateSpeed;
 
 
    /*** Game Engine methods, all can be overridden by subclass ***/
 
    protected override void Awake() {
        base.Awake();
    }
 
    protected override void Start () {
        base.Start();
    }
 
    protected override void Update () {
	    base.Update();
	    if(rotating) TurnToTarget();
	    else if(moving) MakeMove();
	}
 
    protected override void OnGUI() {
        base.OnGUI();
    }

    public virtual void SetBuilding(Building building) {
		//specific initialization for a unit can be specified here
	}

    public override void MouseClick(GameObject hitObject, Vector3 hitPoint, Player controller) {
	    base.MouseClick(hitObject, hitPoint, controller);
	    //only handle input if owned by a human player and currently selected
	    if(player && player.human && currentlySelected) {
	        if(hitObject.name == "Ground" && hitPoint != ResourceManager.InvalidPosition) {
                attacking = false;
	            float x = hitPoint.x;
	            //makes sure that the unit stays on top of the surface it is on
	            float y = hitPoint.y + player.SelectedObject.transform.position.y;
	            float z = hitPoint.z;
	            Vector3 destination = new Vector3(x, y, z);
	            StartMove(destination);
	        }
	    }
    }

	public virtual void StartMove(Vector3 destination) {
	    this.destination = destination;
	    targetRotation = Quaternion.LookRotation (destination - transform.position);
	    rotating = true;
	    moving = false;
	}

	public void StartMove(Vector3 destination, GameObject destinationTarget) {
		StartMove(destination);
		this.destinationTarget = destinationTarget;
	}

	private void TurnToTarget() {
	    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed);

	    CalculateBounds();
	    //sometimes it gets stuck exactly 180 degrees out in the calculation and does nothing, this check fixes that
	    Quaternion inverseTargetRotation = new Quaternion(-targetRotation.x, -targetRotation.y, -targetRotation.z, -targetRotation.w);
	    if(transform.rotation == targetRotation || transform.rotation == inverseTargetRotation) {
	        rotating = false;
	        moving = true;
	    }
	}
    public bool idastar()
    {
        float bound = Vector3.Distance(transform.position, destination);
        float t = 0;
        while (true)
        {
            path.Clear();
            path.Enqueue(transform.position);
            t = search( 0, bound);
            if (t == -1)
                return true;
            else if ( t == float.PositiveInfinity)
                return false;
            bound = t;
        }
    }

    private float search(int v, float bound)
    {
        throw new NotImplementedException();
    }

    public bool isValidPosition(Vector3 pos)
    {
        Building[] buildings = FindObjectsOfType(typeof(Building)) as Building[];
        foreach (Building element in buildings)
        {
            if (element.GetSelectionBounds().Contains(pos))
                return false;
        }
        return true;
    }

    private void MakeMove()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * moveSpeed);
        if (transform.position == destination)
        {
            moving = false;
            movingIntoPosition = false;
        }
        CalculateBounds();
    }
}
