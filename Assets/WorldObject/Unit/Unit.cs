using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RTS;
using System;

public class Unit : WorldObject {
	protected bool moving, rotating;
	 
	private Vector3 destination;
    private Vector3 pathpoint;
	private Quaternion targetRotation;
	private GameObject destinationTarget;
    private Stack<Vector3> path = new Stack<Vector3>();
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
        idastar();
        pathpoint = path.Pop();
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
        path.Clear();
        float bound = Vector3.Distance(transform.position, destination);
        float t = 0;
        for (int i=0; i<30;i++)
        {
            t = search(transform.position, 0, bound);
            if (t == -1.0f)
                return true;
            else if ( float.IsPositiveInfinity(t))
                return false;
            bound = t;
        }
        return false;
    }

    private float search(Vector3 node, float g, float bound)
    {
        float f = g + Vector3.Distance(node, destination);
        if (f > bound)
            return f;
        if (isGoal(node)){
            path.Push(node);
            return -1;
        }
        float min = float.PositiveInfinity;
        float t;
        Vector3 successor;
        for (float i = 0; i<360; i+= 20)
        {
            successor = getNextbyAngle(node, i);
            if (isValidPosition(successor)){
                t = search(successor, g + .1f, bound);
                if (t == -1){
                    path.Push(node);
                    return -1;
                }
                if (t < min)
                    min = t;
            }
        }
        return min;
    }
    private Vector3 getNextbyAngle(Vector3 pos, float angle)
    {
        return pos + Quaternion.AngleAxis(angle, Vector3.up) * Vector3.right * .2f;
    }
    public bool isGoal(Vector3 pos)
    {
        return Vector3.Distance(pos, destination)<1.0;
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
        transform.position = Vector3.MoveTowards(transform.position, pathpoint, Time.deltaTime * moveSpeed);
        if (transform.position == pathpoint)
            pathpoint = path.Pop();
        if (isGoal(transform.position))
        {
            moving = false;
            movingIntoPosition = false;
        }
        CalculateBounds();
    }
}
