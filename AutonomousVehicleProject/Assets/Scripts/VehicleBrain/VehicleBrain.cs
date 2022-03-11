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
	public float sensorLengthCenter = 15f;
	public float sensorLengthSide = 22f;// sqrt(2) * 15
	private bool previousTurnLeft;
	
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
		previousTurnLeft = false;
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
		Sensors();
		//vehicleController.CheckWayPointDistance();//check path process
		//vehicleController.ApplySteer();//rotate vehicle
		//vehicleController.CheckWayPointDistance();//check path process
		//vehicleController.ApplySteer();//rotate vehicle
		//vehicleController.MoveForward(currentVehicleSpeed); //move vehicle
		
		//calculate distance travelled and record current position with added noise
		Vector3 positionalDifference = (transform.position - prevPos);
		currentCarPosition += VehicleBrain.AddNoiseToPoint3D(positionalDifference, positionReadingNoise);
		
		//making sure currentCarPosition is not strayed too far from actual position
		if (Vector3.Distance(transform.position - distanceToRealPosition, currentCarPosition) > maxErrorThreshold){
			currentCarPosition += (transform.position - distanceToRealPosition - currentCarPosition)/2.0f; //fixing error by moving noisy position half way to actual one
		}
		
		//Debug.Log("Current position with noise: " + currentCarPosition + " | Actual position (Relative): " + (transform.position - distanceToRealPosition));
    }
	
	void Sensors(){
		RaycastHit hit;
		Vector3 sensorStartPos = transform.position;
		bool checkLeft, checkRight;

		
		//three sensors, \|/,45 degrees
		if(Physics.Raycast(sensorStartPos,transform.forward, out hit, sensorLengthCenter))
        {// collider in front, need to make rotation
			Debug.DrawLine(sensorStartPos, hit.point);
			// check left and right side, to decide which rotate direction
			checkLeft = Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(-45f, transform.up) * transform.forward, out hit, sensorLengthSide);
			if(checkLeft)
				Debug.DrawLine(sensorStartPos, hit.point);
			checkRight = Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(45f, transform.up) * transform.forward, out hit, sensorLengthSide);
			if(checkRight)
				Debug.DrawLine(sensorStartPos, hit.point);
			
			// decide rotation direction
			if(checkLeft && checkRight)
            {//is facing a straight wall, do the opposite rotation than previous to avoid running into a endless circle
				if(previousTurnLeft)
                {//previous direction is left, then turn right instead
					vehicleController.MakeRightTurn();
					previousTurnLeft = false; //made right turn, then previous turn is not left
                }else
                {//if previous turn is not left, then it is a right turn. this time doing the opposite, left
					vehicleController.MakeLeftTurn();
					previousTurnLeft = true; //made left turn, set it to true
                }
            }else if(checkLeft)
            {//collider on the left, need to turn right
				vehicleController.MakeRightTurn();
				previousTurnLeft = false;//made right turn, then previous turn is not left
			}else
            {// this case checkRight is true
				vehicleController.MakeLeftTurn();
				previousTurnLeft = true; //made left turn, set it to true
			}
		}
    }

	void ProcessInformation(){
		List<HitObject> hitInformation = sensorManager.GetSensorData();
		//TODO: detect if any distances in hitInformation are too close, then act accordingly (will need to edit HitObject class to retain more information to work with)
		//if there is going to be a collision, make correction and return from this function (we don't want to continue to fix directional error if we're fixing collisions)
		
		speedState = SPEED_UP;
		
		//FixDirectionError();
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
