using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class SpeedFluctuation : MonoBehaviour
{
    CinemachineDollyCart dollyCart;
    float targetSpeed;
    [SerializeField] float targetRotation;
    [SerializeField] float changeIntensity;
    [SerializeField] Transform rotateTarget;
    private void Start()
    {
        dollyCart = GetComponent<CinemachineDollyCart>();
        InvokeRepeating("ChangeSpeed",0, 2);
    }
    void ChangeSpeed()
    {
        targetSpeed = Random.Range(50, 150);
    }
    private void Update()
    {
        dollyCart.m_Speed = Mathf.MoveTowards(dollyCart.m_Speed, targetSpeed, Time.deltaTime * changeIntensity);
        rotateTarget.Rotate(Vector3.one * Time.deltaTime* targetRotation);
    }
}
