using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectorRotate : MonoBehaviour
{
    private float rotation = 90;

    private Quaternion targetRot;

    private void FixedUpdate()
    {
        if (transform.localRotation.eulerAngles == targetRot.eulerAngles)
        {
            rotation += Random.Range(-40f, 40f);

            rotation = Mathf.Clamp(rotation, -120f, -60f);

            targetRot = Quaternion.Euler(rotation, 0, 0);
        }

        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRot, Time.fixedDeltaTime * 0.5f);

        if ((targetRot.eulerAngles.x - transform.localRotation.eulerAngles.x) < 2f)
        {
            transform.localRotation = targetRot;
        }
    }
}
