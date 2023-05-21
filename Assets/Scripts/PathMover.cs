using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class PathMover : MonoBehaviour
{
    [SerializeField] private PathCreator _pathCreator;
    
    private NavMeshAgent _navMeshAgent;
    private Queue<Vector3> _pathPoints = new Queue<Vector3>();
    
    private bool _isGameStarted;
    private bool _isInDestinationPoint;

    private void OnEnable()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _pathCreator.OnNewPathCreated += SetPoints;
        UIController.Instance.OnStartGameButtonPressed += OnStartGameButtonPressed;
    }

    private void OnDisable()
    {
        _pathCreator.OnNewPathCreated -= SetPoints;
        UIController.Instance.OnStartGameButtonPressed -= OnStartGameButtonPressed;
    }

    private void SetPoints(IEnumerable<Vector3> points)
    {
        _pathPoints = new Queue<Vector3>(points);
    }

    private void Update()
    {
        if (!_isGameStarted || _isInDestinationPoint) return;
        
        UpdatePathing();
    }

    private void UpdatePathing()
    {
        if (ShouldGetDestination())
            _navMeshAgent.SetDestination(_pathPoints.Dequeue());

       // else _isInDestinationPoint = true;
    }

    private bool ShouldGetDestination()
    {
        if (_pathPoints.Count == 0) return false;

        if (!_navMeshAgent.hasPath || _navMeshAgent.remainingDistance < 0.5f) return true;

        return false;
    }

    private void OnMouseDown()
    {
        _pathCreator.isSelected = true;
    }

    private void OnMouseUp()
    {
        _pathCreator.isSelected = false;
    }

    private void OnStartGameButtonPressed()
    {
        _isGameStarted = true;
        print("Game Started");
    }

    public bool IsInDestinationPoint()
    {
         return _isInDestinationPoint;
    }
}
