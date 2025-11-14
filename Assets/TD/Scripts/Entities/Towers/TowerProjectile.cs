using System.Collections;
using UnityEngine;

public class TowerProjectile : MonoBehaviour
{
    public float speed;
    public AnimationCurve trajectory;
    private float timeToArrive;
    private Vector3 startPosition;
    private float counter;

    Enemy target;
    CurrentTowerStats currentStats;
    public GameObject projectileTransformationPrefab;
    Tower Attacker;
    public float transformationDuration;

    public void SetInfoProjectile(Enemy towerTarget, CurrentTowerStats towerStats, Tower attacker)
    {
        target = towerTarget;
        Attacker = attacker;
        currentStats = towerStats;
        transformationDuration = 0f;
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

        target.TakeDamage(currentStats.Damage, Attacker);
        target.ApplySlow(currentStats.SlowFactor, currentStats.SlowDuration);
        target.ApplyStun(currentStats.StunDuration);

        
        if (projectileTransformationPrefab != null)
        {
            GameObject transformation = Instantiate(projectileTransformationPrefab, transform.position, Quaternion.identity);

            if (currentStats.StunDuration > currentStats.SlowDuration)
                transformationDuration = currentStats.StunDuration;
            else
                transformationDuration = currentStats.SlowDuration;
            Destroy(transformation, transformationDuration);
        }



        Destroy(gameObject);
    }
    
    
}

