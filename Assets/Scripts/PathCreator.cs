using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PathCreator : MonoBehaviour
{
    [SerializeField] private float _distanceBetweenPoints;
    [SerializeField] private Color _lineColor;
    [SerializeField] private float _lineWidthMultiplier;
    
    public bool isSelected;
    public bool needToUpdatePath;

    private LineRenderer _lineRenderer; // used to draw lines for player
    private List<Vector3> _points = new List<Vector3>(); // list, storing points of player input
    private Camera _mainCamera;

    public Action<IEnumerable<Vector3>> OnNewPathCreated = delegate { };

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.material.color = _lineColor;
        _lineRenderer.widthMultiplier = _lineWidthMultiplier;
        
        GameController.Instance.pathCreators.Add(this);
    }

    private void Start()
    {
        _mainCamera = Camera.main;
        
        
    }

    private void Update()
    {
       // if (!isSelected) return;
        
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButton(0)) DrawPath();
        
        else if (Input.GetMouseButtonUp(0)) OnNewPathCreated(_points); print("ssss");
    }

    private void DrawPath()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            Vector3 point = new Vector3(hitInfo.point.x, hitInfo.point.y + 1, hitInfo.point.z);
            
            if (DistanceToLastPoint(point) > _distanceBetweenPoints)
            {
                _points.Add(point);

                _lineRenderer.positionCount = _points.Count;
                _lineRenderer.SetPositions(_points.ToArray());
            }
        }
    }

    private float DistanceToLastPoint(Vector3 point)
    {
        if (!_points.Any())
            return Mathf.Infinity;

        return Vector3.Distance(_points.Last(), point);
    }
}
