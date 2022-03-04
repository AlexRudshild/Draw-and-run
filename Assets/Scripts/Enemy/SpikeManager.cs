using UnityEngine;

public class SpikeManager : MonoBehaviour
{
    public float SpikeToggleDealay = 5f;

    private float _timer;

    private void FixedUpdate()
    {
        _timer += Time.fixedDeltaTime;

        if (_timer >= SpikeToggleDealay)
        {
            _timer = 0;

            try
            {
                Spike.ToggleSpikeState();
            }
            catch (System.Exception)
            {
                Debug.LogError("Class Spike not found disable SpikeManager");

                this.enabled = false;
            }
        }
    }
}
