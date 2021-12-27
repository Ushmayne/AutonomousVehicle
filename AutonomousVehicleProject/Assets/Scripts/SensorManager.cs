using UnityEngine;

/*
	Script to manage all sensors on vehicle that this script is attached through
	
	This essentially acts as a pipeline for the sensor inputs to pass through to get to the car and the car's controller
*/
[RequireComponent(typeof(VehicleController))]
public class SensorManager : MonoBehaviour
{
	[SerializeField] private GameObject sensorParent; //parent object that contains all needed sensors as its children
	private VehicleController vehicleController; //VehicleController script gotten from this object

    void Start(){
        vehicleController = GetComponent<VehicleController>();
    }

	//TODO : function to retrieve all angles and positions from sensors and apply any noise necessary
	//then send that updated positioning/rotation to vehicle using VehicleController script
	//might need to split into two functions for simplicity's sake
	private void ProcessSensorData(){}
	
	
    void Update()
    {
        //move car
    }
}
