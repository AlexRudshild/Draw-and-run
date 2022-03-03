using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Drawing))]
public class Move : MonoBehaviour
{
    public delegate void Delegate();

    public event Delegate OnFinishReach;

    public float Speed = 2f;
    public bool canMove = false;

    public Transform FinishPoint;

    Drawing _drawing;

    private void Awake()
    {
        _drawing = GetComponent<Drawing>();

        _drawing.OnDrawComplite += StartMove;

        if (FinishPoint == null)
            FinishPoint = GameObject.FindWithTag("Finish").transform;
    }

    private void OnEnable()
    {
        _drawing.OnMInionsRunOut += StopMove;
    }

    private void OnDisable()
    {
        _drawing.OnMInionsRunOut -= StopMove;
    }

    void Update()
    {
        if (canMove)
            transform.position += transform.forward * Speed * Time.deltaTime;

        if ((FinishPoint.position - transform.position).magnitude < 1)
        {
            OnFinishReach?.Invoke();
            this.enabled = false;
            _drawing.MinionPlace();
            Minion.startDance();
        }
    }

    public void StartMove()
    {
        GetComponent<Drawing>().OnDrawComplite -= StartMove;

        canMove = true;

        Minion.startRuning();
    }

    public void StopMove()
    {
        canMove = false;
    }
}
