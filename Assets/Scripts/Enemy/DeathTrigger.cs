using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Minion"))
        {
            if (other.TryGetComponent(out Minion minion))
            {
                minion.Death();
            }
        }
    }
}
