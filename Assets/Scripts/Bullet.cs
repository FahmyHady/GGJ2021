using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float damage;
    public float timeBeforeAutoDestroy;
    public LayerMask damagable;
    private void Start()
    {
        damage = FindObjectOfType<Player>().bulletDamage;
    }
    private void OnEnable()
    {
        Invoke("DestroyBullet", 5);
    }
    void FixedUpdate()
    {
        Vector3 tempPos = transform.position;
        MoveProjectile();
        if (Physics.Linecast(tempPos, transform.position, out RaycastHit hitInfo, damagable))
        {
            DestroyBullet();
            if (hitInfo.transform.TryGetComponent(out Entity damageable))
            {
                Debug.Log("hi");
                if (hitInfo.transform.CompareTag("Boss"))
                {
                    ((Boss)damageable).ApplyDamage(damage,hitInfo.point);

                }
                else
                {
                    damageable.ApplyDamage(damage);
                }
            }
        }

    }

    void MoveProjectile()
    {
        Vector3 tempVect = transform.forward * speed;
        transform.position += tempVect;
    }

    void DestroyBullet()
    {
        gameObject.SetActive(false);
    }

}
