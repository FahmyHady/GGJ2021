using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MiniBoss : Entity
{


    [SerializeField] bool pulsate;
    float elapsedTime;
    Vector3 originalScale;
    [SerializeField] [Range(0.01f, 3)] float pulsatingDuration;
    [SerializeField] [Range(0, 100)] float pulsatingRange;
    [SerializeField] float puslatingSpeed;
    [SerializeField] Transform miniBossMesh;
    [SerializeField] ObjectPoolBehaviour stuffToSpew;
    [SerializeField] float throwStrength;
    [SerializeField] ObjectPoolBehaviour rocksToThrow;
    [SerializeField] int numberToSpew;
    [SerializeField] GameObject deathParticle;
    MeshRenderer phaseOneMeshRenderer;
    DamageVisual damageVisualiser;
    Player player;
    public AudioSource aud;
    public AudioClip[] auds;
    public AudioSource camaud;
    public AudioClip camauds;
    private void Start()
    {
        phaseOneMeshRenderer = miniBossMesh.gameObject.GetComponent<MeshRenderer>();
        damageVisualiser = GetComponent<DamageVisual>();
        player = FindObjectOfType<Player>();
        currentHP = maxHP;
        originalScale = miniBossMesh.localScale;
    }

    private void Update()
    {
            Pulsate();
  
    }



    private void Pulsate()
    {
        if (pulsate)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime < pulsatingDuration)
            {
                Vector3 newScale = miniBossMesh.localScale;
                newScale.x = Mathf.PingPong(Time.time * puslatingSpeed, pulsatingRange) + originalScale.x;
                newScale.y = Mathf.PingPong(Time.time * puslatingSpeed, pulsatingRange) + originalScale.y;
                newScale.z = Mathf.PingPong(Time.time * puslatingSpeed, pulsatingRange) + originalScale.z;
                miniBossMesh.localScale = newScale;
            }
            else
            {
                miniBossMesh.localScale = originalScale;
                pulsate = false;
                elapsedTime = 0;

            }
        }
        else
        {
            Vector3 newScale = miniBossMesh.localScale;
            newScale.x = Mathf.PingPong(Time.time * puslatingSpeed *0.1f, pulsatingRange * 10) + originalScale.x;
            newScale.y = Mathf.PingPong(Time.time * puslatingSpeed*0.1f , pulsatingRange * 10) + originalScale.y;
            newScale.z = Mathf.PingPong(Time.time * puslatingSpeed*0.1f , pulsatingRange * 10) + originalScale.z;
            miniBossMesh.localScale = newScale;
        }
    }
    public override void ApplyDamage(float damage, Vector3 hitLocation = default)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            camaud.clip = camauds;
            camaud.Play();
            Instantiate(deathParticle, transform.position, Quaternion.identity);

            gameObject.SetActive(false);
            aud.PlayOneShot(auds[1]);
        }
        pulsate = true;
        SpewStuff(hitLocation);
        damageVisualiser.ShowDamage(phaseOneMeshRenderer,Color.black);
        aud.PlayOneShot(auds[0]);


    }
    private void SpewStuff(Vector3 whereToSpew)
    {
        for (int i = 0; i < numberToSpew; i++)
        {
            GameObject spew = stuffToSpew.GetPooledObject();
            Vector3 pos = transform.position + Random.onUnitSphere * (miniBossMesh.localScale.x);
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
