using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class MinionsManager : MonoBehaviour
{
    public delegate void Delegate();
    public static event Delegate OnMinionsRunOut;

    [Header("Minion Settings")]

    [SerializeField] private GameObject MinionPrefab;
    [SerializeField] private Transform MinionsHandler;
    [SerializeField] private int MinionsStartCount;

    private List<Minion> _minions;

    private static MinionsManager Instance;

    void Start()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogError("MinionsManager exist destroying");
            Destroy(this);
        }

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

        DistributeMinions();
    }

    private void OnEnable()
    {
        if (_minions == null) _minions = new List<Minion>();

        Minion.OnMinionDeath += MinionRemove;

        PlayerMover.OnFinishReach += DistributeMinions;
    }

    private void OnDisable()
    {
        Minion.OnMinionDeath -= MinionRemove;

        PlayerMover.OnFinishReach -= DistributeMinions;
    }

    private void MinionRemove(Minion minion)
    {
        _minions.Remove(minion);

        if (_minions.Count == 0)
            OnMinionsRunOut?.Invoke();
    }

    public void MinionAdd(Minion minion)
    {
        minion.transform.parent = MinionsHandler;

        _minions.Add(minion);
    }

    public static void OnMinionCollect(Transform minionTransform)
    {
        Instance.MinionCollect(minionTransform);
    }

    private void MinionCollect(Transform minionTransform)
    {
        Minion minion = Instantiate(MinionPrefab, minionTransform.position, transform.rotation, MinionsHandler).GetComponent<Minion>();

        Destroy(minionTransform.gameObject);

        minion.StartRun();

        minion.TargetPosition = minion.transform.localPosition;

        minion.TargetPosition.y = 0;

        MinionAdd(minion);
    }

    public void DistributeMinions()
    {
        for (int i = 0; i < _minions.Count; i++)
        {
            float posX = i % 5 - 2;
            float posZ = Mathf.FloorToInt(i / 5) - 2;

            _minions[i].TargetPosition = new Vector3(posX, 0, posZ);
        }
    }

    public void MinionsMove(Vector3 minionPos)
    {
        foreach (var minion in _minions)
        {
            minion.TargetPosition = PositionFromCameraToWorld(minionPos);
        }
    }

    public void MinionsMove(SplineComputer spline)
    {
        for (int i = 0; i < _minions.Count; i++)
        {
            Vector3 minionPos = spline.EvaluatePosition(i * (1f / _minions.Count));

            _minions[i].TargetPosition = PositionFromCameraToWorld(minionPos);
        }
    }

    // тут попытка перевести координаты линии перед камерой в координаты для персонажа на земле
    private Vector3 PositionFromCameraToWorld(Vector3 position)
    {
        position.y = 0;

        position.x *= 10;

        position.z -= 0.5f;
        position.z *= 20;

        return position;
    }
}
