using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    private bool _isParkingZoneDetected;
    
    public Action OnObstacleDetected = delegate {  };
    public Action OnParkingZoneDetected = delegate {  };

    private void Start()
    {
        GameController.Instance.AddCar(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent(out ParkingObstacle obstacle))
        {
            OnObstacleDetected?.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ParkingZone parkingZone))
        {
            _isParkingZoneDetected = true;
            parkingZone.carInParkingZone = this;
            
            OnParkingZoneDetected?.Invoke();
        }
    }

    public bool IsParkingZoneDetected()
    {
        return _isParkingZoneDetected;
    }
}
