public class HitObject
{
    public float distance;
    public float angle;

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
        return "Distance: " + distance + ". Angle: " + angle;
    }
}