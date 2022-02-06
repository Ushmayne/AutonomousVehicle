public class HitObject
{
    public float distance;
    public float angle;

	//TODO: Polish class so that more information is stored (point of origin, point of impact, object that was hit, anything else you can think of)
	
    public HitObject(float distance, float angle)
    {
        UpdateValues(distance, angle);
    }
	
	public void UpdateValues(float distance, float angle)
    {
        this.distance = distance;
        this.angle = angle;
    }

    public override string ToString()
    {
        return "Distance: " + distance + " | Angle: " + angle;
    }
}