using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogRemover : MonoBehaviour
{
    public GameObject Fog;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if(other.gameObject.tag == "Player")
        {
            Fog.SetActive(false);
        }
        //Fog.SetActive(false);
        if(other.tag == "Ghost")
        {
            Fog.SetActive(true);
        }
    }
}
