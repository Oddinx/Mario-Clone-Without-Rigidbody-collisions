using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destruccionportiempo : MonoBehaviour
{
    // Start is called before the first frame update

    float tiempo = 5f;
    void Start()
    {
        Destroy (this.gameObject, tiempo);
    }

    // Update is called once per frame
  
}
