using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VehicleController : MonoBehaviour
{
	public Vector3 intendedDirection;
	public Transform path;
	private List<Transform> nodes;
	private Rigidbody rb;
	private int currentNode = 0;
	public bool isMove;
	
	void Start(){ 
		intendedDirection = transform.forward; 
		rb = GetComponent<Rigidbody>();

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
		//transform.LookAt(nodes[currentNode].position);
		SlowLookAt(nodes[currentNode]);
    }
    //moves vehicle forward by distance specified ("forward" refers to current facing direction)
    public void MoveForward(float distance){
		//transform.position += transform.forward.normalized * distance; 
		if (isMove)
		{
			rb.MovePosition(transform.position + (transform.forward.normalized * distance));
		}

	}
	//check the cloest node in the path
	public void CheckWayPointDistance()
    {
		if(Vector3.Distance(transform.position,nodes[currentNode].position) < 0.5f)
        {
			if (currentNode == nodes.Count - 1)
			{
				currentNode = 0;
			}
			else
            {
				currentNode++;
			}
        }
    }
	//turns vehicle (on y axis) depending on inputted degrees (left and right turns depends on if inputted degrees are negative or positive)
    public void Turn(float deg){
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
	public void MakeLeftTurn(){ Turn(-90.0f); }
	public void MakeRightTurn(){ Turn(90.0f); }

	//slowly rotate to look at given target
	private void SlowLookAt(Transform target)
    {
		Vector3 direction = (target.position - transform.position).normalized;
		Quaternion targetLookAt = Quaternion.LookRotation(direction);
		transform.rotation = Quaternion.Slerp(transform.rotation,targetLookAt,0.08f);
    }
	
	//sets intended direction for vehicle as a reference point for error
	public void SetIntendedDirection(Vector3 dir){ intendedDirection = dir;	}
	
	//gets how many degrees of error there is in the direction the vehicle is headed vs the intended direction
	public float GetDirectionError(){ return Vector3.SignedAngle(intendedDirection, transform.forward, Vector3.up); }
}
