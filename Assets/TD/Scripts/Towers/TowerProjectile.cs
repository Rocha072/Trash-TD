using System.Collections;
using UnityEngine;

public class TowerProjectile : MonoBehaviour
{
    public float speed;
    public AnimationCurve trajectory;
    public Enemy target;
    private float timeToArrive;
    private Vector3 startPosition;
    private float counter;
    public float damage;
    public float slowFactor;
    public float slowDuration;
    public GameObject projectileTransformationPrefab;
    void Start()
    {
        startPosition = transform.position;
        timeToArrive = Vector3.Distance(target.transform.position, transform.position) / speed;
        StartCoroutine(nameof(GoToTarget));
    }

    IEnumerator GoToTarget()
    {
        do
        {
            counter += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, target.transform.position, counter / timeToArrive);
            transform.position += Vector3.up * trajectory.Evaluate(counter / timeToArrive);
            yield return null;
        } while (counter < timeToArrive);

        target.TakeDamage(damage);
        target.ApplySlow(slowFactor, slowDuration);
        if (projectileTransformationPrefab != null)
        {
            GameObject transformation = Instantiate(projectileTransformationPrefab, transform.position, Quaternion.identity);

            Destroy(transformation, slowDuration);
        }



        Destroy(gameObject);
    }
    
    
}

