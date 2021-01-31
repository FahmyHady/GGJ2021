using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Spew : Entity
{

    [SerializeField] float speed;
    [SerializeField] Entity player;
    Rigidbody rigidbody;
    [SerializeField] GameObject deathParticle;

    private void Start()
    {
        MiniBoss mini = FindObjectOfType<MiniBoss>();
        if (mini!=null)
        {
        touchDamage = mini.bulletDamage;
        }
        else
        {
            touchDamage = FindObjectOfType<Boss>().bulletDamage;

        }
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
