using System.Collections;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public delegate void Hide();
    public static Hide hideSpike;


    public delegate void Show();
    public static Show showSpike;

    private Collider _collider;

    private Vector3 TargetPosition;

    private void Start()
    {
        _collider = GetComponent<Collider>();

        TargetPosition = transform.position;
    }

    private void OnEnable()
    {
        hideSpike += HideSpike;
        showSpike += ShowSpike;
    }

    private void OnDisable()
    {
        hideSpike -= HideSpike;
        showSpike -= ShowSpike;
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

    private void HideSpike()
    {
        _collider.enabled = false;

        TargetPosition = transform.position - transform.up;
    }

    private void ShowSpike()
    {
        _collider.enabled = true;

        TargetPosition = transform.position + transform.up;
    }
}
