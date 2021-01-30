using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateChain : MonoBehaviour
{
    [SerializeField] int chainPartsCount;
    [SerializeField] [Range(0.05f, 0.2f)] float minScale;
    [SerializeField] [Range(1, 5)] float maxScale;

    [ContextMenu("CreateChain")]
    public void CreateAChain()
    {
        GameObject parent = new GameObject("ChainParent");
        Transform[] tentacleParts = new Transform[chainPartsCount];
        for (int i = 0; i < chainPartsCount; i++)
        {
            tentacleParts[i] = GameObject.CreatePrimitive(PrimitiveType.Capsule).transform;

            DestroyImmediate(tentacleParts[i].gameObject.GetComponent<Collider>());
        }
        for (int i = 0; i < chainPartsCount; i++)
        {
            tentacleParts[i].localScale = (((i + ((minScale * tentacleParts.Length) / maxScale))) / (tentacleParts.Length / maxScale)) * Vector3.one;
        }
        for (int i = 1; i < tentacleParts.Length; i++)
        {
            Vector3 pos = tentacleParts[i - 1].transform.position;
            Vector3 offset = (tentacleParts[i - 1].localScale + tentacleParts[i].localScale * 0.6f);
            pos.y = pos.y - offset.y;
            tentacleParts[i].transform.position = pos;
        }     
        for (int i = tentacleParts.Length-1; i >=0; i--)
        {
            if (i == tentacleParts.Length-1)
                tentacleParts[i].transform.parent = parent.transform;
            else
                tentacleParts[i].transform.parent = tentacleParts[i+1];
        }

    }
}
