using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VehicleController : MonoBehaviour
{
	public Vector3 intendedDirection;
	private Rigidbody rb;
	public bool isMove;
	//private Quaternion targetLookAt;


	void Start()
	{
		intendedDirection = transform.forward;
		rb = GetComponent<Rigidbody>();
		isMove = true;
	}
	/*
		Transform[] pathTransform = path.GetComponentsInChildren<Transform>();
		nodes = new List<Transform>();
		for (int i = 0; i < pathTransform.Length; i++)
		{//gets all nodes
			if (pathTransform[i] != transform)
			{//new nodes, add to list
				nodes.Add(pathTransform[i]);
			}
		}
		isMove = true;
	}

	// turn the vehicle to face targeted point
	public void ApplySteer()
	{
		if (isMove)
		{
			//transform.LookAt(nodes[currentNode].position);
			Vector3 direction = nodes[currentNode].position - transform.position;
			targetLookAt = Quaternion.LookRotation(direction);
			//getted new target point, slerp to it
			transform.rotation = Quaternion.Slerp(transform.rotation, targetLookAt, 0.2f);
		}

	}*/
	//moves vehicle forward by distance specified ("forward" refers to current facing direction)
	public void MoveForward(float distance)
	{
		//transform.position += transform.forward.normalized * distance; 
		if (isMove)
		{
			rb.MovePosition(transform.position + (transform.forward.normalized * distance));
		}

	}/*
	//check the cloest node in the path
	public void CheckWayPointDistance()
	{
		if (Vector3.Distance(transform.position, nodes[currentNode].position) < 0.5f)
		{//reached node, switch target to next node
		 //makeRota = true;//able to do slowLookAt
			if (currentNode == nodes.Count - 1)
			{
				currentNode = 0;
			}
			else
			{
				currentNode++;
			}
		}
	}*/
	//turns vehicle (on y axis) depending on inputted degrees (left and right turns depends on if inputted degrees are negative or positive)
	public void Turn(float deg)
	{
		transform.Rotate(0f,deg,0f);// = Quaternion.Euler(transform.rotation.x, transform.rotation.y + deg, transform.rotation.z);
	}
	//turns vehicle (on y axis) depending on inputted degrees (left and right turns depends on if inputted degrees are negative or positive)
	public void Turn(float deg)
	{
		transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y + deg, transform.rotation.z);
	}

	public void canMove()
	{
		isMove = true;
	}

	public void canNotMove()
	{
		isMove = false;
	}
	//turns car left or right
	public void MakeLeftTurn() { Turn(-90.0f); }
	public void MakeRightTurn() { Turn(90.0f); }

	//slowly rotate to look at given target
	private void SlowLookAt(Quaternion targetLookAt)
	{
		transform.rotation = Quaternion.Slerp(transform.rotation, targetLookAt, 0.08f);
		//makeRota = false;//reset it, wont slerp until it reached this target
	}

	//sets intended direction for vehicle as a reference point for error
	public void SetIntendedDirection(Vector3 dir) { intendedDirection = dir; }

	//gets how many degrees of error there is in the direction the vehicle is headed vs the intended direction
	public float GetDirectionError() { return Vector3.SignedAngle(intendedDirection, transform.forward, Vector3.up); }

	private bool checkDeg(Vector3 targetDir)
	{
		float angle = Vector3.Angle(targetDir, transform.forward);
		if (angle < 180.0f && angle > 5.0f)
			return true;
		else
			return false;//it is doing a rotation
	}
}
