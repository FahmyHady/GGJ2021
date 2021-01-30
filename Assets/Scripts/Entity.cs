using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class Entity : MonoBehaviour
{
    [SerializeField] public float maxHP;
    [SerializeField] public float touchDamage;
    [SerializeField] public float bulletDamage;
    [HideInInspector] public float currentHP;
     void Awake()
    {
        currentHP = maxHP;
    }
    virtual public void ApplyDamage(float damage, Vector3 hitLocation=default)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
