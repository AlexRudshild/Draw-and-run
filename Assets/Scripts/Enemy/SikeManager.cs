using UnityEngine;

public class SikeManager : MonoBehaviour
{
    private bool _enableSpike;
    private float _timer;

    private void Awake()
    {
        var spike = FindObjectOfType<Spike>(false);

        if (spike == null)
        {
            Destroy(this);
        }
    }

    private void FixedUpdate()
    {
        _timer += Time.fixedDeltaTime;

        if (_timer >= 5f)
        {
            _timer = 0;

            if (_enableSpike) Spike.showSpike();
            else Spike.hideSpike();

            _enableSpike = !_enableSpike;
        }
    }
}
