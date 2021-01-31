using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class DamageVisual : MonoBehaviour
{
    [SerializeField] float flashSpeed;
    public void ShowDamage(MeshRenderer meshRenderer, Color original)
    {
        StopAllCoroutines();
        if (gameObject.activeSelf)
        {
            StartCoroutine(ApplyDamageVisual(meshRenderer.material, original));
        }
    }

    IEnumerator ApplyDamageVisual(Material mat, Color original)
    {

        Color target = original + Color.white * 0.4f;
        while (mat.color != target)
        {
            mat.color = Vector4.MoveTowards(mat.color, target, Time.deltaTime * flashSpeed);
            yield return null;
        }
        target = original;
        while (mat.color != target)
        {
            mat.color = Vector4.MoveTowards(mat.color, target, Time.deltaTime * flashSpeed);
            yield return null;
        }
    }
}
