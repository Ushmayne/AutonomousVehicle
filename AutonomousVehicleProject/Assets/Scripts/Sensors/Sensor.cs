using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Sensor : MonoBehaviour
{
	public float inaccuracy = 0.05f;

    /// <summary>
    /// The sensor's position relative to the vehicle that it's a child of
    /// </summary>
    public Vector3 relPos;

    private void Awake()
    {
        relPos = transform.localPosition;
    }

    public virtual float GetInaccurateDistance(float accurateDist)
        {
            //Determine how much the sensor can be inaccurate by
            float varianceAmt = accurateDist * inaccuracy;
            
            //Determine the bounds of inaccuracy for the sensor
            float lowerBound = accurateDist - varianceAmt;
            float upperBound = accurateDist + varianceAmt;
            
            //Now generate a number between the lower bound and upper bound
            return Random.Range(lowerBound, upperBound);
        }
}
