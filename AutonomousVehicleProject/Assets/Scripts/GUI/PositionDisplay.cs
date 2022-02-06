using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PositionDisplay : MonoBehaviour
{
    public Text positionText;
    public Vector2 pos;

    // Start is called before the first frame update
    void Start()
    {
        pos = new Vector2(1, 1);
        //GameObject theSensor = GameObject.Find("TheSensor");
        //SensorScript sensorScript = theSensor.GetComponent<SensorScript>();
        //pos = sensorScript.position;
    }

    // Update is called once per frame
    void Update()
    {
        //pos = sensorScript.position;
        positionText.text = "Position X : " + pos.x + "\n\nPosition Y : " + pos.y;
    }
}
