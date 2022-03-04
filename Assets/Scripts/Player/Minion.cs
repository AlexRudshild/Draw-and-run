using System.Collections;
using UnityEngine;

public class Minion : MonoBehaviour
{
    public delegate void death(Minion minion);
    public static event death OnMinionDeath;

    public delegate void Animation();
    public static Animation startRuning;
    public static Animation startDance;

    private Animator animator;

    public Vector3 TargetPosition;

    [Header("Particles")]

    public ParticleSystem DeathPartical;

    private void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();

    }

    private void OnEnable()
    {
        startRuning += StartRun;
        startDance += StartDance;
    }

    private void OnDisable()
    {
        startRuning -= StartRun;
        startDance -= StartDance;
    }

    private void Update()
    {
        if (TargetPosition != transform.localPosition)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, TargetPosition, Time.deltaTime * 5);

            if ((TargetPosition - transform.localPosition).magnitude < 0.01f)
            {
                transform.localPosition = TargetPosition;
            }
        }
    }

    public IEnumerator DeathDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Death();
    }

    public void Death()
    {

        OnMinionDeath?.Invoke(this);

        var effect = Instantiate(DeathPartical, transform.position, Quaternion.identity).gameObject;

        Destroy(effect, 3);

        animator.SetBool("Running", false);
        transform.parent = null;

        var rb = gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.AddTorque(new Vector3(1, 1, 1) * 2, ForceMode.VelocityChange);

        Destroy(gameObject, 5);

        this.enabled = false;
    }

    public void StartRun()
    {
        if (animator == null) animator = GetComponent<Animator>();

        animator.SetBool("Running", true);
    }

    public void StartDance()
    {
        if (animator == null) animator = GetComponent<Animator>();

        animator.SetTrigger("Victory");

        transform.rotation = Quaternion.Euler(0, 180, 0);

        StartCoroutine(DeathDelay(2));
    }
}
