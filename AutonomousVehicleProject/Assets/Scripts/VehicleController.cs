using UnityEngine;

public class VehicleController : MonoBehaviour
{
	
	//moves vehicle forward by distance specified ("forward" refers to current facing direction)
	public void MoveForward(float distance){
		transform.position += transform.forward.normalized * distance; 
	}
	
	//turns vehicle (on y axis) depending on inputted degrees (left and right turns depends on if inputted degrees are negative or positive)
    public void Turn(float deg){
		transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y + deg, transform.rotation.z);
	}
}
