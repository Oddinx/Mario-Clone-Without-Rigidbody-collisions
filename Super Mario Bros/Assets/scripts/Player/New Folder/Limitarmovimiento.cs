using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limitarmovimiento : MonoBehaviour
{
    // Start is called before the first frame update


    // Update is called once per frame

    void Update()
    {

        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);

        pos.x = Mathf.Clamp01(pos.x);

        

        transform.position = Camera.main.ViewportToWorldPoint(pos);

       
        
    }
}
