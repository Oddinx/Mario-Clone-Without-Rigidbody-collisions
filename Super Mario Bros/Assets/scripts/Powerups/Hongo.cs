using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;

[RequireComponent(typeof(BoxCollider2D))]
public class Hongo : MonoBehaviour
{    

  

 
     public float speed = 1f;


     private BoxCollider2D boxCollider;
    

        private Vector2 velocity,m;

        private bool grounded;



 protected LayerMask  prevlayer;







     
   private bool destruido;

float gravity = -20;


Controller2D controller;
    // Start is called before the first frame update
    void Start()
    {
       



            boxCollider = GetComponent<BoxCollider2D>();
   

    controller = GetComponent<Controller2D>();

 prevlayer = LayerMask.GetMask("Ground","Obstaculo","Player","Premiobloque") ;
  
 
    }



    // Update is called once per frame
    void Update()
 {
    controller.collisionMask &= ~(1 << 12);
        velocity.x = speed;
      velocity.y += gravity*Time.deltaTime;


      controller.Move(velocity*Time.deltaTime,m);  




      //Raycast





      //finraycast

      //boxcast



//Finboxcastizquierda

//fin boxcast



   


 



      
      if(grounded){

       velocity.y = 0;
   




     }

          
     



  grounded = false;

 //    Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0);

 //       foreach (Collider2D hit in hits)
 //       {
            // Ignore our own collider.
 //           if (hit == boxCollider)
 //               continue;

 //           ColliderDistance2D colliderDistance = hit.Distance(boxCollider);
    
            // Ensure that we are still overlapping this collider.
            // The overlap may no longer exist due to another intersected collider
            // pushing us out of this one.
   //         if (colliderDistance.isOverlapped)
         //   {
     //           transform.Translate(colliderDistance.pointA - colliderDistance.pointB);

                // If we intersect an object beneath us, set grounded to true. 
       //         if (Vector2.Angle(colliderDistance.normal, Vector2.up) < 90 && velocity.y < 0)
        //        {
          //          grounded = true;
           //     }
           // }
       // }
    }


public void Accion(){




	StartCoroutine (AC ());

   
}



void Cambiodedireccion(){


speed *=-1;

}
void Destroy(){


 Destroy(this.gameObject);


}

IEnumerator AC(){




velocity.x = 0;

speed= 0f;

velocity.y = 1f;

gravity = 0;

controller.collisionMask = LayerMask.GetMask("Nothing");
yield return new WaitForSeconds (0.9f);

 velocity.y += gravity*Time.deltaTime;
gravity = -20;
controller.collisionMask = prevlayer;

speed = 4f;
velocity.x = speed;


}








	void OnVerticalCollisionEnter(Collider2D collider) {

      	if (collider.tag == "Player") {

			collider.gameObject.SendMessage ("Actualizarestado",1);
			Destroy();
		}

    if(collider.tag =="Ground" || collider.tag =="Obstaculo" ){

         grounded = true;
    }
	}

	void OnHorizontalCollisionEnter(Collider2D collider) {

    	if (collider.tag == "Player") {

			collider.gameObject.SendMessage ("Actualizarestado",1);
			Destroy();
		}

    if(collider.tag == "Premio"){

       Accion();

    }

		if (collider.tag != "Player")
			speed *= -1f;
	}






}
