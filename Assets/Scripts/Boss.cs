using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Boss : Entity
{


    [SerializeField] bool pulsate;
    float elapsedTime;
    Vector3 originalScale;
    [SerializeField] [Range(0.01f, 3)] float pulsatingDuration;
    [SerializeField] [Range(0, 100)] float pulsatingRange;
    [SerializeField] float puslatingSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float laserDamage;
    [SerializeField] float riseSpeed;
    [SerializeField] Transform bossMesh;
    [SerializeField] ObjectPoolBehaviour stuffToSpew;
    [SerializeField] float throwStrength;
    [SerializeField] ObjectPoolBehaviour rocksToThrow;
    [SerializeField] int numberToSpew;
    [SerializeField] GameObject weapon;
    [SerializeField] GameObject deathParticle;

    public bool canBeHit;
    MeshRenderer phaseOneMeshRenderer;
    DamageVisual damageVisualiser;
    Player player;
    bool rise;
    Vector3 riseTarget;
    private void Start()
    {
        phaseOneMeshRenderer = bossMesh.gameObject.GetComponent<MeshRenderer>();
        damageVisualiser = GetComponent<DamageVisual>();
        player = FindObjectOfType<Player>();
        currentHP = maxHP;
        originalScale = bossMesh.localScale;
    }

    internal void Rise()
    {
        rise = true;
        riseTarget = transform.position + Vector3.up * Random.Range(100, 300);
    }

    private void Update()
    {
        if (rise)
        {
            transform.position = Vector3.MoveTowards(transform.position, riseTarget, Time.deltaTime * riseSpeed);
            if (transform.position == riseTarget)
            {
                rise = false;
                EnableWeapons();
                canBeHit = true;
            }
        }
        Pulsate();

        if (!rise)
        {
            Quaternion lookRotation = Quaternion.LookRotation(-(transform.position - player.transform.position).normalized);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void EnableWeapons()
    {
        weapon.SetActive(true);
    }

    private void Pulsate()
    {
        if (pulsate)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime < pulsatingDuration)
            {
                Vector3 newScale = bossMesh.localScale;
                newScale.x = Mathf.PingPong(Time.time * puslatingSpeed, pulsatingRange) + originalScale.x;
                newScale.y = Mathf.PingPong(Time.time * puslatingSpeed, pulsatingRange) + originalScale.y;
                newScale.z = Mathf.PingPong(Time.time * puslatingSpeed, pulsatingRange) + originalScale.z;
                bossMesh.localScale = newScale;
            }
            else
            {
                bossMesh.localScale = originalScale;
                pulsate = false;
                elapsedTime = 0;

            }
        }

    }
    public override void ApplyDamage(float damage, Vector3 hitLocation = default)
    {
        if (canBeHit)
        {
            currentHP -= damage;
            if (currentHP <= 0)
            {
                Instantiate(deathParticle, transform.position, Quaternion.identity);

                gameObject.SetActive(false);
            }
            pulsate = true;
            SpewStuff(hitLocation);
            damageVisualiser.ShowDamage(phaseOneMeshRenderer, Color.black);
        }

    }
    private void SpewStuff(Vector3 whereToSpew)
    {
        for (int i = 0; i < numberToSpew; i++)
        {
            GameObject spew = stuffToSpew.GetPooledObject();
            Vector3 pos = transform.position + Random.onUnitSphere * (bossMesh.localScale.x);
            pos.y = 0.5f;
            spew.transform.position = whereToSpew;
            spew.SetActive(true);
        }
        Vector3 direction = (player.transform.position - whereToSpew).normalized;
        Vector3 posOne = whereToSpew;
        Vector3 posTwo = whereToSpew;
        posOne.x -= 5;
        posOne.z -= 5;
        posTwo.x += 5;
        posTwo.z += 5;
        ThrowRock(direction, posOne);
        ThrowRock(direction, posTwo);
    }

    private void ThrowRock(Vector3 direction, Vector3 rockPos)
    {
        GameObject rock = rocksToThrow.GetPooledObject();
        rock.transform.position = rockPos;
        rock.SetActive(true);
        Rock rockComponent = rock.GetComponent<Rock>();
        rockComponent.rb.isKinematic = false;
        rockComponent.rb.AddForce(direction * throwStrength);
        rockComponent.rb.AddTorque(throwStrength * Vector3.one);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.ApplyDamage(laserDamage);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.ApplyDamage(touchDamage);
        }
        ApplyDamage(player.touchDamage);
        elapsedTime = 0;
        pulsate = true;
    }
}
