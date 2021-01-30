using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Spew : Entity
{

    [SerializeField] float speed;
    [SerializeField] Entity player;
    Rigidbody rigidbody;

    private void Start()
    {
        touchDamage = FindObjectOfType<Boss>().bulletDamage;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 playerPos = player.transform.position - player.transform.forward *Random.Range(-25f, 25f);
        Vector3 direction = -(transform.position - playerPos).normalized;
        rigidbody.MovePosition(transform.position+(direction * speed));
        rigidbody.MoveRotation(Quaternion.LookRotation(player.transform.forward));
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.ApplyDamage(touchDamage);
        }
        gameObject.SetActive(false);
    }
    public override void ApplyDamage(float damage, Vector3 hitLocation = default)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            gameObject.SetActive(false);
        }

    }

}
