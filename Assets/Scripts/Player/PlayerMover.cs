using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    public delegate void Delegate();

    public static event Delegate OnFinishReach;

    public float Speed = 2f;
    public bool canMove = false;

    public Transform FinishPoint;

    private void Awake()
    {
        Drawer.OnDrawComplite += StartMove;

        if (FinishPoint == null)
            FinishPoint = GameObject.FindWithTag("Finish")?.transform;
    }

    private void OnEnable()
    {
        MinionsManager.OnMinionsRunOut += StopMove;
    }

    private void OnDisable()
    {
        MinionsManager.OnMinionsRunOut -= StopMove;
    }

    void Update()
    {
        if (canMove)
            transform.position += transform.forward * Speed * Time.deltaTime;

        if (FinishPoint != null && (FinishPoint.position - transform.position).magnitude < 1)
        {
            OnFinishReach?.Invoke();
            this.enabled = false;
            Minion.startDance();
        }
    }

    public void StartMove()
    {
        Drawer.OnDrawComplite -= StartMove;

        canMove = true;

        Minion.startRuning();
    }

    public void StopMove()
    {
        canMove = false;
    }
}
