using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bandera : MonoBehaviour
{
    // Start is called before the first frame update

    Vector2  velocity;

    float speed = -4f;

  


 void Update() {

          transform.Translate(velocity * Time.deltaTime);

   


   
}




   void Bajarbandera(){



StartCoroutine(movb());




   }



IEnumerator movb(){


velocity.y = speed;

yield return new WaitForSeconds (2f);

speed =0;

velocity.y = 0;

}



}
