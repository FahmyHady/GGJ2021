using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Rock : Entity
{

    [SerializeField] float speed;
    [SerializeField] Entity player;
    public Rigidbody rb;
    [SerializeField] GameObject deathParticle;

    private void Start()
    {
        touchDamage = FindObjectOfType<MiniBoss>().bulletDamage;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();
        rb = GetComponent<Rigidbody>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.ApplyDamage(touchDamage);
        }
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        Instantiate(deathParticle, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
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

}
