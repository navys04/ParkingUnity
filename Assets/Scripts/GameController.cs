using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController _instance;

    public static GameController Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<GameController>();
            }

            return _instance;
        }

        private set => _instance = value;
    }

    public List<PathCreator> pathCreators;

    private int _currentPathController;

    private List<Car> _cars = new List<Car>();
    private List<ParkingZone> _parkingZones = new List<ParkingZone>();

    public Action OnGameFailed = delegate {  };
    public Action OnGameCompleted = delegate {  };

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        if (pathCreators.Count == 0) return;
        DeactivateUnavailablePathCreators();
        
        pathCreators[_currentPathController].OnNewPathCreated += OnPathCreated;
    }

    private void DeactivateUnavailablePathCreators()
    {
        for (int i = 0; i < pathCreators.Count; i++)
        {
            if (i == _currentPathController) continue;

            pathCreators[i].enabled = false;
        }
        
        print("deactivated");
    }

    private void OnPathCreated(IEnumerable<Vector3> points)
    {
        pathCreators[_currentPathController].OnNewPathCreated -= OnPathCreated;
        pathCreators[_currentPathController].enabled = false;
        _currentPathController++;
        if (pathCreators.Count > _currentPathController)
        {
            pathCreators[_currentPathController].enabled = true;
            pathCreators[_currentPathController].OnNewPathCreated += OnPathCreated;
        }
    }

    public void AddCar(Car car)
    {
        if (_cars.Contains(car)) return;
        
        _cars.Add(car);
        car.OnObstacleDetected += FailGame;
        car.OnParkingZoneDetected += CheckGameState;
    }

    public void AddParkingZone(ParkingZone parkingZone)
    {
        _parkingZones.Add(parkingZone);
    }

    private void CheckGameState()
    {
        foreach (var car in _cars)
        {
            if (!car.IsParkingZoneDetected()) return;
        }
        
        CompleteGame();
    }
    
    private void CompleteGame()
    {
        OnGameCompleted?.Invoke();
    }
    
    private void FailGame()
    {
        OnGameFailed?.Invoke();
    }
}
