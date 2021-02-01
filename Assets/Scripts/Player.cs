using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class Player : Entity
{

    [SerializeField] float speed, turnSpeed, tiltSpeed;
    [SerializeField] ObjectPoolBehaviour bulletPool;
    [SerializeField] Transform shootPointOne;
    [SerializeField] Transform shootPointTwo;
    [SerializeField] Transform playerVisualMesh;
    [SerializeField] float visualLeanAmout;
    [SerializeField] float shootRate;
    [SerializeField] GameObject deathParticle;
    float elapsedTime;
    Rigidbody rigidbody;
    float h, v;
    public soundrand srand;
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        elapsedTime += Time.deltaTime;
        if (elapsedTime - shootRate >= 0)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Shoot();
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed *=2;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed /= 2;
        }
    }
    public override void ApplyDamage(float damage, Vector3 hitLocation = default)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            Instantiate(deathParticle, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
        }
    }
    private void Shoot()
    {
        elapsedTime = 0;
        var bulletOne = bulletPool.GetPooledObject();
        bulletOne.transform.position = shootPointOne.position;
        bulletOne.transform.rotation = shootPointOne.rotation;
        bulletOne.SetActive(true);
        var bulletTwo = bulletPool.GetPooledObject();
        bulletTwo.transform.position = shootPointTwo.position;
        bulletTwo.transform.rotation = shootPointTwo.rotation;
        bulletTwo.SetActive(true);
        srand.SendMessage("randlsrsound");
        
    }

    private void FixedUpdate()
    {
        rigidbody.velocity = transform.forward * speed;
        TurnSpaceship();
    }
    void TurnSpaceship()
    {
        Vector3 newTorque = new Vector3(v * tiltSpeed, h * turnSpeed, 0);
        rigidbody.AddRelativeTorque(newTorque);

        rigidbody.rotation =
            Quaternion.Slerp(rigidbody.rotation, Quaternion.Euler(new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0)), .5f);

        VisualSpaceshipTurn();
    }

    void VisualSpaceshipTurn()
    {
        playerVisualMesh.localEulerAngles = new Vector3(playerVisualMesh.localEulerAngles.x, playerVisualMesh.localEulerAngles.y, -h * visualLeanAmout);
    }

}
