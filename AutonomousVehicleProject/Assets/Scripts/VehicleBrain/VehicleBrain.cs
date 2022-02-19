using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//brain that allows car to be autonomous
public class VehicleBrain : MonoBehaviour
{
	[HideInInspector] public const int SPEED_UP = 0;
	[HideInInspector] public const int SPEED_DOWN = 1;
	
	private VehicleController vehicleController; //VehicleController script gotten from this object
	
	[HideInInspector] public SensorManager sensorManager; //sensor manager
	
	//vehicle movement values
	[SerializeField] private float maxVehicleSpeed = 0.6f;
	[SerializeField] private float vehicleAcceleration = 0.001f;
	private float currentVehicleSpeed = 0.0f;
	
	//car positioning
	[SerializeField] private float positionReadingNoise = 0.05f;
	[SerializeField] private float maxErrorThreshold = 0.2f;
	private Vector3 currentCarPosition;
	private Vector3 distanceToRealPosition;
	
	private int speedState = SPEED_DOWN; //car's speed state
	
	//how often to process information
	[SerializeField] private float updateInterval = 0.75f;
	private float updateTimer = 0.0f;
	
	public float maxDirectionError = 0.05f; //directional error clamping
	
    // Start is called before the first frame update
    void Start()
    {
        vehicleController = GetComponent<VehicleController>();
		currentCarPosition = new Vector3(0.0f, 0.0f, 0.0f);
		distanceToRealPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        updateTimer += Time.deltaTime;
		if (updateTimer >= updateInterval){
			
			ProcessInformation();
			
			updateTimer = 0.0f;
		}
		
		switch(speedState){
			case SPEED_UP:
				SpeedUp();
				break;
			case SPEED_DOWN:
				SlowDown();
				break;
		}
		
		
		Vector3 prevPos = transform.position;
		vehicleController.ApplySteer();//rotate vehicle
		vehicleController.CheckWayPointDistance();//check path process
		vehicleController.MoveForward(currentVehicleSpeed); //move vehicle
		
		//calculate distance travelled and record current position with added noise
		Vector3 positionalDifference = (transform.position - prevPos);
		currentCarPosition += VehicleBrain.AddNoiseToPoint3D(positionalDifference, positionReadingNoise);
		
		//making sure currentCarPosition is not strayed too far from actual position
		if (Vector3.Distance(transform.position - distanceToRealPosition, currentCarPosition) > maxErrorThreshold){
			currentCarPosition += (transform.position - distanceToRealPosition - currentCarPosition)/2.0f; //fixing error by moving noisy position half way to actual one
		}
		
		//Debug.Log("Current position with noise: " + currentCarPosition + " | Actual position (Relative): " + (transform.position - distanceToRealPosition));
    }
	
	void ProcessInformation(){
		List<HitObject> hitInformation = sensorManager.GetSensorData();
		//TODO: detect if any distances in hitInformation are too close, then act accordingly (will need to edit HitObject class to retain more information to work with)
		//if there is going to be a collision, make correction and return from this function (we don't want to continue to fix directional error if we're fixing collisions)
		
		speedState = SPEED_UP;
		
		FixDirectionError();
	}
	
	void FixDirectionError(){
		//fixing directional errors based on intended direction
		float directionError = vehicleController.GetDirectionError();
		if (Math.Abs(directionError) > maxDirectionError){
			vehicleController.Turn(-directionError * 0.5f); //fix error by 50% each time
		}
	}
	
	void SpeedUp(){ currentVehicleSpeed = Mathf.Clamp(currentVehicleSpeed + vehicleAcceleration, 0.0f, maxVehicleSpeed); }
	void SlowDown(){ currentVehicleSpeed = Mathf.Clamp(currentVehicleSpeed - vehicleAcceleration, 0.0f, maxVehicleSpeed); }
	
	//given a point and a noise radius, returns back the point within a random radius (clamped by noiseRadius) of original point
	//important: y value will not be effected within this function call!
	public static Vector3 AddNoiseToPoint3D(Vector3 point, float noiseRadius){
		Vector2 twoDimRandomPoint = UnityEngine.Random.insideUnitCircle * noiseRadius;
		//Debug.Log(twoDimRandomPoint.ToString("F3"));
		return point + new Vector3(twoDimRandomPoint.x, 0.0f, twoDimRandomPoint.y);
	}
	
}
