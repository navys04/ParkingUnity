using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkingZone : MonoBehaviour
{
    public Car carInParkingZone;

    private void Start()
    {
        GameController.Instance.AddParkingZone(this);
    }
}
