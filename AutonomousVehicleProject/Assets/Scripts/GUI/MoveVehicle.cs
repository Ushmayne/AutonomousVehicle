using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveVehicle : MonoBehaviour
{
    public GameObject myVehicle;

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        myVehicle.GetComponent<VehicleController>().isMove = true;
    }
}
