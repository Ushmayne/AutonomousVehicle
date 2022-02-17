using UnityEngine;

public class VehicleController : MonoBehaviour
{
	public Vector3 intendedDirection;
	private Rigidbody rb;
	public bool isMove;
	
	void Start(){ 
		intendedDirection = transform.forward; 
		rb = GetComponent<Rigidbody>();
		isMove = true;
	}
	
	//moves vehicle forward by distance specified ("forward" refers to current facing direction)
	public void MoveForward(float distance){
		//transform.position += transform.forward.normalized * distance; 
		if (isMove)
		{
			rb.MovePosition(transform.position + (transform.forward.normalized * distance));
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
	
	//sets intended direction for vehicle as a reference point for error
	public void SetIntendedDirection(Vector3 dir){ intendedDirection = dir;	}
	
	//gets how many degrees of error there is in the direction the vehicle is headed vs the intended direction
	public float GetDirectionError(){ return Vector3.SignedAngle(intendedDirection, transform.forward, Vector3.up); }
}
