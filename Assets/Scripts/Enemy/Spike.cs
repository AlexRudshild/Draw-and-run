using System.Collections;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public delegate void Toggle();
    public static Toggle ToggleSpikeState;


    private Collider _collider;

    private Vector3 TargetPosition;

    private void Start()
    {
        _collider = GetComponent<Collider>();

        TargetPosition = transform.position;
    }

    private void OnEnable()
    {
        ToggleSpikeState += ToggleSpike;
    }

    private void OnDisable()
    {
        ToggleSpikeState -= ToggleSpike;
    }

    private void FixedUpdate()
    {
        if (TargetPosition != transform.position)
        {
            transform.position = Vector3.Lerp(transform.position, TargetPosition, Time.deltaTime * 10);

            if ((TargetPosition - transform.position).magnitude < 0.01f)
            {
                transform.position = TargetPosition;
            }
        }
    }

    private void ToggleSpike()
    {
        _collider.enabled = !_collider.enabled;

        if (_collider.enabled)
        {
            TargetPosition = transform.position + transform.up;
        }
        else
        {
            TargetPosition = transform.position - transform.up;
        }
    }
}
