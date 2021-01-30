using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class TentacleConstraint : MonoBehaviour
{
    [SerializeField] Transform Body;
    float maxDistance;
    [SerializeField] [Range(0.1f, 2)] float distanceFactor = 0.7f;
    [SerializeField] [Range(0.1f, 10)] float pullBackSpeed;

    private void Start()
    {
        var tentecaleParts = GetComponentsInChildren<Transform>();
        maxDistance = distanceFactor * tentecaleParts.Sum(t => t.localScale.z);
    }
    private void Update()
    {
        if (Vector3.Distance(transform.position, Body.position) > maxDistance)
        {
            PullBack();
        }
    }

    private void PullBack()
    {
        transform.position = Vector3.Lerp(transform.position, Body.position, Time.deltaTime * pullBackSpeed);
    }
}
