using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StopVehicle : MonoBehaviour, IPointerDownHandler
{
    public GameObject myVehicle;

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        myVehicle.GetComponent<VehicleController>().isMove = false;
    }
}
