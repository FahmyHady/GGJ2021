using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundrand : MonoBehaviour
{
    public AudioSource Asource;
    public AudioClip[] lasers;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void randlsrsound()
    {
        Asource.PlayOneShot(lasers[Random.Range(0, lasers.Length)]);
    }
}
