using System.Collections.Generic;
using UnityEngine;

public class HazardSpawner : MonoBehaviour
{
    #region //Pooling variables
    [Header("Pooling")]
    [SerializeField] private GameObject hazardPrefab = null;
    [SerializeField, Min(1)] private float poolSize = 20;
    private List<GameObject> hazards = new List<GameObject>();
    #endregion

    #region //Spawn timing variables
    [Header("Spawn Timing")]
    [SerializeField] private float startTime = 30;
    [SerializeField] private float stopTime = 0;
    [SerializeField, Min(0.1f)] private float spawnDelay = 1;
    [SerializeField, Min(0)] private float spawnVariance = 0.5f;
    private Timer timer = null;
    private float currentTime = 0;
    #endregion

    #region //Spawn state
    [Header("Velocity")]
    [SerializeField] private Vector2 spawnVelocity = Vector2.zero;
    [SerializeField, Min(0)] private Vector2 velocityVariance = Vector2.zero;

    [Header("Scale and Rotation")]
    [SerializeField] private float baseScale = 1;
    [SerializeField] private float scaleVariance = 0.2f;
    [SerializeField] private float baseRotation = 0;
    [SerializeField] private float rotationVariance = 45f;
    #endregion


    #region //Monnobehaviour
    private void Awake()
    {
        timer = FindObjectOfType<Timer>();

        for(int ii = 0; ii < poolSize; ii++)
        {
            var hazard = Instantiate(hazardPrefab, transform);
            hazards.Add(hazard);
            hazard.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if(timer.GetTimeLeft() > startTime) return;
        if(timer.GetTimeLeft() < stopTime) return;

        if(currentTime <= 0)
        {
            var hazard = SpawnHazard();
            SetUpHazard(hazard);
            currentTime = VaryValue(spawnDelay, spawnVariance);
        }
        else 
        {
            currentTime -= Time.deltaTime;
        }
    }
    #endregion

    #region //Spawning
    private GameObject SpawnHazard()
    {
        foreach(var hazard in hazards)
        {
            if(hazard.activeInHierarchy) continue;
            hazard.SetActive(true);
            foreach(Transform child in hazard.transform)
                child.gameObject.SetActive(true);

            return hazard;
        }

        var newHazard = Instantiate(hazardPrefab, transform);
        hazards.Add(newHazard);
        return newHazard;
    }

    private void SetUpHazard(GameObject hazard)
    {
        var rb = hazard.GetComponent<Rigidbody2D>();
        rb.position = transform.position;
        rb.velocity = new Vector2(VaryValue(spawnVelocity.x, velocityVariance.x), VaryValue(spawnVelocity.y, velocityVariance.y));
        rb.SetRotation(VaryValue(baseRotation, rotationVariance));

        var scale = VaryValue(baseScale, scaleVariance);
        hazard.transform.localScale = new Vector3(scale, scale, 1);
    }

    private float VaryValue(float baseValue, float variance)
    {
        return baseValue + Random.Range(-variance, variance);
    }
    #endregion
}