using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PositionDisplay : MonoBehaviour
{
    public Text positionText;
	public Transform vehicleTransform;


    // Update is called once per frame
    void Update()
    {
        //pos = sensorScript.position;
        positionText.text = "Position X : " + vehicleTransform.position.x.ToString("0.00") + "\n\nPosition Y : " + vehicleTransform.position.z.ToString("0.00");
    }
}
