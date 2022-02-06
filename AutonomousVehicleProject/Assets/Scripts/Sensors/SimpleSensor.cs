using System.Collections;
using UnityEngine;

/// <summary>
/// This sensor shoots out a ray at (0,1,0) relative to its own position
/// </summary>
public class SimpleSensor : Sensor
{
    public Vector3 angleVector;    //This is the current angle that a raycast is being sent from 
    public float yAngleDegs;
    private bool isWaiting = false; //True if the program is currently waiting to cast a ray
    private float rayDelay = 1f;    //Amount of time between raycasts 
    
	private HitObject hitObject;
	
    void Start()
    {
        //TODO: Replace the Vector3.zero argument with the position of the vehicle? Not sure if this is needed as this may just be a placeholder value until the sensor first hits something
        hitObject = new HitObject(0.0f, 0.0f, this, Vector3.zero);
        //Start the current angle as the direction that the vehicle is facing
        //currentAngleVector = transform.rotation.eulerAngles;
        angleVector = new Vector3(0, 1, 0);
        yAngleDegs = transform.rotation.eulerAngles.y;
    }

    void Update()
    {
        if (!isWaiting)
        {
            StartCoroutine(WaitThenCastRayCoroutine(rayDelay));
        }
    }

    /// <summary>
    /// Wait for the provided delay and then cast a ray at the angle vector
    /// </summary>
    /// <param name="delay">The amount of seconds to wait</param>
    /// <returns></returns>
    IEnumerator WaitThenCastRayCoroutine(float delay)
    {
        //Set the bool that represents waiting to true so we don't call this multiple times
        isWaiting = true;
        //Wait for the 'delay' number of seconds
        yield return new WaitForSeconds(delay);
        
        //Cast a ray from the vehicle's position out in the direction of the current angle variable 
        Physics.Raycast(transform.position, transform.TransformDirection(angleVector), out RaycastHit hit);
        //Please note, the ray has much longer range than is shown in the debug window
        Debug.DrawRay(transform.position, transform.TransformDirection(angleVector), Color.red, 0.5f);

        //Print info on the hit object if there was a hit object
        if (hit.collider != null)
        {
            //TODO: Replace the Vector3.zero argument with the position of the vehicle.
            hitObject.UpdateValues(GetInaccurateDistance(hit.distance), yAngleDegs, this, Vector3.zero);
            print("Simple sensor hit object " + hitObject);
        }
        
        //Done waiting, can call again
        isWaiting = false;
    }
	
	public HitObject GetHitObject(){ return hitObject; }
}
