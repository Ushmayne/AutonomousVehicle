using System.Collections.Generic;
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

	private List<HitObject> allHitObjects;
	[SerializeField] private float sensorDataRetrievalInterval = 1.5f;
	private float dataRetrievalTimer = 0.0f;
	
    void Start(){
		allHitObjects = new List<HitObject>();
        vehicleController = GetComponent<VehicleController>();
    }

	//gets all hit objects from all sensors and stores them in allHitObjects
	private void ExtractSensorHitObjects(){
		if (sensorParent == null) return;
		
		allHitObjects.Clear();
		
		foreach (Transform t in sensorParent.transform){
			Sensor s = t.GetComponent<Sensor>();
			if (s is SimpleSensor simpleSensor){
				allHitObjects.Add(simpleSensor.GetHitObject());
			}
			else if (s is RadarSensor radarSensor){
				allHitObjects.AddRange(radarSensor.GetHitObjects());
			}
		}
	}

	private void ProcessSensorData(){}
	
	void Update(){
		
		//update sensor data every time interval
		dataRetrievalTimer += Time.deltaTime;
		
		if (dataRetrievalTimer >= sensorDataRetrievalInterval){
			ExtractSensorHitObjects();
			dataRetrievalTimer = 0.0f;
		}
	}
	

}
