using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PositionDisplay : MonoBehaviour
{
    public Text positionText;
	public VehicleBrain vehicleBrain;


    // Update is called once per frame
    void Update()
    {
        //pos = sensorScript.position;
        positionText.text = "Position X : " + vehicleBrain.GetSimulationPosition().x.ToString("0.00") + 
							"\n\nPosition Y : " + vehicleBrain.GetSimulationPosition().z.ToString("0.00");
    }
}
