using UnityEngine;

public class PickUpMinionTrigger : MonoBehaviour
{
    public ParticleSystem PickUpEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Minion"))
        {
            gameObject.SetActive(false);

            var effect = Instantiate(PickUpEffect, transform.position, Quaternion.identity).gameObject;

            Destroy(effect, 3);

            MinionsManager.OnMinionCollect(transform);
        }
    }
}
