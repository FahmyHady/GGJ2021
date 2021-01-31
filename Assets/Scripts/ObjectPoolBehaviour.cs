using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ObjectPoolBehaviour : MonoBehaviour
{

    public ObjectPoolData data;

    private List<GameObject> pooledObjects;

    void Start()
    {
        CreatePool();
    }

    public void CreatePool()
    {
        pooledObjects = new List<GameObject>();

        for(int i = 0; i < data.amountToPool; i++)
        {
            GameObject obj = (GameObject)Instantiate(data.objectToPool);
            obj.transform.parent = transform;
            StartCoroutine(DisableObjects(obj));
            pooledObjects.Add(obj);
        }
        
    }
    IEnumerator DisableObjects(GameObject obj)
    {
        yield return new WaitForEndOfFrame();
        obj.SetActive(false);
    }
    public GameObject GetPooledObject()
    {
        for(int i = 0; i < pooledObjects.Count; i++)
        {
            if(!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        if(data.shouldExpand)
        {
            GameObject obj = (GameObject)Instantiate(data.objectToPool);
            obj.SetActive(false);
            pooledObjects.Add(obj);
            return obj;
        }
       
        return null;
    } 
   

}