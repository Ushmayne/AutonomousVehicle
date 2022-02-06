using UnityEngine;

public class HitObject
{
    /// <summary>
    /// The distance from the sensor to the hit object
    /// </summary>
    public float distance;
    /// <summary>
    /// The angle that the hit object is from the sensor in degrees. This is from a top-down view where the direction that the car is facing is 0 degrees, directly right is 90 degrees, etc.
    /// </summary>
    public float angle;

    /// <summary>
    /// The sensor that hit this object
    /// </summary>
    public Sensor sensor;
    
    /// <summary>
    /// The origin of the raycast that hit the object in worldspace
    /// </summary>
    public Vector3 pointOrigin;
    
    /// <summary>
    /// The impact point where the raycast hit the object in worldspace
    /// </summary>
    public Vector3 pointImpact;
    
    public HitObject(float distance, float angle, Sensor sensor, Vector3 vehiclePos)
    {
        UpdateValues(distance, angle, sensor, vehiclePos);
    }
	
	public void UpdateValues(float distance, float angle, Sensor sensor, Vector3 vehiclePos)
    {
        this.distance = distance;
        this.angle = angle;
        this.sensor = sensor;
        //The pointOrigin is the position of the sensor that sensed this object (world position). sensor.relPos is the sensor's position relative to the vehicle and vehiclePos is the vehicles position in the world when the sensor hit this object
        pointOrigin = sensor.relPos + vehiclePos;
        pointImpact = CalculatePointImpact(pointOrigin, distance, angle);
    }

    /// <summary>
    /// Take an already inaccurate relative distance and angle and calculate the world position of this impact
    /// </summary>
    /// <param name="pointOrigin">The world position of the sensor that hit this object</param>
    /// <param name="distance">The distance from the pointOrigin to the hit object</param>
    /// <param name="angle">The angle at which the ray hit the object, in DEGREES</param>
    /// <returns>The world position of the impact</returns>
    public Vector3 CalculatePointImpact(Vector3 pointOrigin, float distance, float angle)
    {
        //The angle variable considers 0 to be "up" (if you are looking from a bird's eye view). When working with a unit circle, 0 would be "right". This means that we need to modify the angle value for calculations
        angle = ConvertAngleToUnitCircle(angle);
        
        //Need to convert the angle from degrees to radians
        float radians = Mathf.Deg2Rad * angle;
        float xPos = Mathf.Cos(radians);
        float zPos = Mathf.Sin(radians);

        //TODO: calculate y position as well?
        Vector3 pointImpact = new Vector3(xPos, 0, zPos);
        
        //This will be a normalized vector, need to multiply by the distance to get the correct value
        pointImpact *= distance;
        
        //Now that we have the vector of the impact point, need to add it onto the origin point
        pointImpact += pointOrigin;

        return pointImpact;
    }

    public float ConvertAngleToUnitCircle(float angle)
    {
        angle = 90 - angle;
        //If this value is negative, we will need to convert it to a positive. For example, -90 degrees is equivalent to 270 degrees. This can be done by incrementing by 360 and then doing modulus 
        angle += 360;
        angle %= 360;
        return angle;
    }

    public override string ToString()
    {
        return "Distance: " + distance + " | Angle: " + angle + " | Origin Point (world): " + pointOrigin +
               " | Impact Point (world): " + pointImpact;
    }
}