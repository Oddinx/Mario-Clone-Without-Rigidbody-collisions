using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pbandera : MonoBehaviour
{
    // Start is called before the first frame update
   GameObject Player;

   GameObject Band;


BoxCollider2D boxCollider;
   void Start(){
   
Band = GameObject.Find("Band");



   }


 void Update() {
     //boxcast


//Finboxcastizquierda

//fin boxcast      



   }

   void Bajarbandera(){

         Band.SendMessage("Bajarbandera");

   }


   
 

}
