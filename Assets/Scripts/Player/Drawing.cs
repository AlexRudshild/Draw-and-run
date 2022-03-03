using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class Drawing : MonoBehaviour
{
    public delegate void Delegate();

    public event Delegate OnDrawComplite;

    public event Delegate OnMInionsRunOut;

    public SplineComputer splineComputer;

    public Transform MinionsHandler;

    public int MinionsStartCount;

    public GameObject MinionPrefab;

    private List<Minion> Minions;
    private Camera _camera;
    private bool _drawing;
    private List<SplinePoint> _splinePoints = new List<SplinePoint>();

    void Start()
    {
        _camera = Camera.main;

        if (splineComputer == null)
            splineComputer = FindObjectOfType<SplineComputer>();

        if (MinionsHandler == null)
        {
            Debug.LogError("MinionsHandler does not exist");

            MinionsHandler = transform;
        }

        for (int i = 0; i < MinionsStartCount; i++)
        {
            Minion minion = Instantiate(MinionPrefab, transform.position, transform.rotation).GetComponent<Minion>();

            MinionAdd(minion);
        }

        MinionPlace();
    }

    void Update()
    {
        if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
        {
            if (!_drawing)
            {
                _splinePoints.Clear();

                StartCoroutine(Draw());
            }
            _drawing = true;
        }
    }

    private void OnEnable()
    {
        if (Minions == null) Minions = new List<Minion>();

        foreach (var minion in Minions)
        {
            minion.OnMinionDeath += MinionRemove;
            minion.OnMinionCollect += MinionCollect;
        }
    }

    private void OnDisable()
    {
        foreach (var minion in Minions)
        {
            minion.OnMinionDeath -= MinionRemove;
            minion.OnMinionCollect -= MinionCollect;
        }
    }

    private void MinionRemove(Minion minion)
    {
        minion.OnMinionDeath -= MinionRemove;
        minion.OnMinionCollect -= MinionCollect;

        Minions.Remove(minion);

        if (Minions.Count == 0)
        {
            OnMInionsRunOut?.Invoke();
        }
    }

    private void MinionAdd(Minion minion)
    {
        minion.OnMinionDeath += MinionRemove;
        minion.OnMinionCollect += MinionCollect;

        minion.transform.parent = MinionsHandler;

        Minions.Add(minion);
    }

    private void MinionCollect(Transform minionTrans)
    {
        Minion minion = Instantiate(MinionPrefab, minionTrans.position, transform.rotation).GetComponent<Minion>();

        Destroy(minionTrans.gameObject);

        minion.StartRun();

        MinionAdd(minion);
    }

    public void MinionPlace()
    {
        float posX, posZ;
        for (int i = 0; i < Minions.Count; i++)
        {

            posX = i % 5 - 2;
            posZ = Mathf.FloorToInt(i / 5) - 2;

            Minions[i].TargetPosition = new Vector3(posX, 0, posZ);
        }
    }

    IEnumerator Draw()
    {
        while (Input.touchCount > 0 || Input.GetMouseButton(0))
        {
            AddSplinePoint(touchPoint);

            yield return new WaitForSeconds(0.05f);
        }

        PlaceMinion();

        OnDrawComplite?.Invoke();

        _drawing = false;
    }

    private void PlaceMinion()
    {
        Vector3 minionPos;

        for (int i = 0; i < Minions.Count; i++)
        {
            if (_splinePoints.Count > 1)
            {
                minionPos = splineComputer.EvaluatePosition(i * (1f / Minions.Count));
            }
            else
                minionPos = touchPoint.position;



            minionPos.y = 0;

            minionPos.x *= 10;

            minionPos.z -= 0.5f;
            minionPos.z *= 20;

            Minions[i].TargetPosition = minionPos;
        }

        _splinePoints.Clear();

        AddSplinePoint(new SplinePoint(Vector3.zero));
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
