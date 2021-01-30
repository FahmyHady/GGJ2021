﻿using System.Collections;
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
    [SerializeField] float shootRate;
    float elapsedTime;
    Rigidbody rigidbody;
    float h, v;
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
            if (Input.GetKey(KeyCode.Mouse0))
            {
                Shoot();
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed *= 1.5f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed /= 1.5f;
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
        //shipModel.localEulerAngles = new Vector3(data.steeringInput.x * data.leanAmount_Y
        //    , shipModel.localEulerAngles.y, data.steeringInput.z * data.leanAmount_X);
    }

}
