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
	[SerializeField] private float maxVehicleSpeed;
	[SerializeField] private float vehicleAcceleration;
	private float currentVehicleSpeed = 0.0f;
	
	private int speedState = SPEED_DOWN; //car's speed state
	
	//how often to process information
	[SerializeField] private float updateInterval;
	private float updateTimer = 0.0f;
	
	public float maxDirectionError = 0.05f; //directional error clamping
	
    // Start is called before the first frame update
    void Start()
    {
        vehicleController = GetComponent<VehicleController>();
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
		vehicleController.MoveForward(currentVehicleSpeed);
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
	
	
	
	
	
}
