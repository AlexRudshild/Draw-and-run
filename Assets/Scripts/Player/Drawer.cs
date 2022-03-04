using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

[RequireComponent(typeof(MinionsManager))]
public class Drawer : MonoBehaviour
{
    public delegate void Delegate();
    public static event Delegate OnDrawComplite;

    [SerializeField] private SplineComputer splineComputer;

    private MinionsManager _minionsManager;
    private Camera _camera;
    private List<SplinePoint> _splinePoints = new List<SplinePoint>();

    void Start()
    {
        _camera = Camera.main;

        _minionsManager = GetComponent<MinionsManager>();

        if (splineComputer == null)
            splineComputer = FindObjectOfType<SplineComputer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            StartCoroutine(Draw());
        }
    }

    private IEnumerator Draw()
    {
        _splinePoints.Clear();

        while (Input.touchCount > 0 || Input.GetMouseButton(0))
        {
            AddSplinePoint(touchPoint);

            yield return new WaitForFixedUpdate();
        }

        
        if (_splinePoints.Count > 1)
            _minionsManager.MinionsMove(splineComputer);

        else
            _minionsManager.MinionsMove(touchPoint.position);


        _splinePoints.Clear();

        AddSplinePoint(new SplinePoint(Vector3.zero));

        OnDrawComplite?.Invoke();
    }


    private void AddSplinePoint(SplinePoint point)
    {
        if (_splinePoints.Count == 0 || _splinePoints[_splinePoints.Count - 1].position != touchPoint.position)
        {
            _splinePoints.Add(point);

            splineComputer.SetPoints(_splinePoints.ToArray(), SplineComputer.Space.Local);
        }
    }

    private SplinePoint touchPoint => GetTouchPoint();

    private SplinePoint GetTouchPoint()
    {
        Ray ray;

        if (Input.mousePosition.y > Screen.height / 3)
        {
            ray = _camera.ScreenPointToRay(new Vector3(Input.mousePosition.x, Screen.height / 3));
        }
        else
        {
            ray = _camera.ScreenPointToRay(Input.mousePosition);
        }

        return new SplinePoint(ray.direction);
    }
}
