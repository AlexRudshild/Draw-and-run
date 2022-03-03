using System.Collections;
using UnityEngine;

public class Minion : MonoBehaviour
{
    public delegate void death(Minion minion);
    public delegate void collect(Transform minion);

    public event death OnMinionDeath;
    public collect OnMinionCollect;

    public delegate void Animation();
    public static Animation startRuning;
    public static Animation startDance;

    private Animator animator;

    public Vector3 TargetPosition;

    [Header("Particles")]

    public ParticleSystem DeathPartical;
    public ParticleSystem PickUpMinion;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gem"))
        {
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Minion"))
        {
            PickUpMinion.Play();
            other.gameObject.SetActive(false);
            OnMinionCollect?.Invoke(other.gameObject.transform);
        }
        else
        {
            Death();
        }
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

        DeathPartical.gameObject.SetActive(true);

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
