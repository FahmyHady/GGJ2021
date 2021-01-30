using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Boss : Entity
{

    bool phaseOne;
    bool phaseTwo;
    bool phaseThree;

    [SerializeField] bool pulsate;
    float elapsedTime;
    Vector3 originalScale;
    [SerializeField] [Range(0.01f, 3)] float pulsatingDuration;
    [SerializeField] [Range(0, 100)] float pulsatingRange;
    [SerializeField] float puslatingSpeed;
    [SerializeField] Transform bossPhaseOneMesh;
    [SerializeField] ObjectPoolBehaviour stuffToSpew;
    [SerializeField] int numberToSpew;
    private void Start()
    {
        currentHP = maxHP;
        phaseOne = true;
        originalScale = bossPhaseOneMesh.localScale;
    }

    private void Update()
    {
        if (phaseOne)
        {
            Pulsate();
        }
        else if (phaseTwo)
        {

        }
        else
        {

        }
    }

    private void Pulsate()
    {
        if (pulsate)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime < pulsatingDuration)
            {
                Vector3 newScale = bossPhaseOneMesh.localScale;
                newScale.x = Mathf.PingPong(Time.time * puslatingSpeed, pulsatingRange) + originalScale.x;
                newScale.y = Mathf.PingPong(Time.time * puslatingSpeed, pulsatingRange) + originalScale.y;
                newScale.z = Mathf.PingPong(Time.time * puslatingSpeed, pulsatingRange) + originalScale.z;
                bossPhaseOneMesh.localScale = newScale;
            }
            else
            {
                bossPhaseOneMesh.localScale = originalScale;
                pulsate = false;
                elapsedTime = 0;

            }
        }
    }
    public override void ApplyDamage(float damage,Vector3 hitLocation=default)
    {
        base.ApplyDamage(damage);
        pulsate = true;
        SpewStuff(hitLocation);

    }
    private void SpewStuff(Vector3 whereToSpew)
    {
        for (int i = 0; i < numberToSpew; i++)
        {
            GameObject spew = stuffToSpew.GetPooledObject();
            Vector3 pos= transform.position + Random.onUnitSphere * (bossPhaseOneMesh.localScale.x );
            pos.y = 0.5f;
            spew.transform.position =whereToSpew ;
            spew.SetActive(true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        elapsedTime = 0;
        pulsate = true;
    }
}
