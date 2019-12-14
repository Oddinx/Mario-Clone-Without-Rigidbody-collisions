using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bandera : MonoBehaviour
{
    // Start is called before the first frame update

    Vector2  velocity;

    float speed = -3f;









   void Bajarbandera(){

      transform.Translate(velocity * Time.deltaTime);

StartCoroutine(movb());


   }



IEnumerator movb(){
velocity.y = speed;

yield return new WaitForSeconds (1f);

speed =0;

velocity.y = 0;

}



}
