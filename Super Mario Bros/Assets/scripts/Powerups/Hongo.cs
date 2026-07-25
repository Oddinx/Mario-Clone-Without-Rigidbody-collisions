using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GlobalTypes;
[RequireComponent(typeof(BoxCollider2D))]
public class Hongo : Powerup
{    

  
    // Start is called before the first frame update

    private Hongo hongo;

    public tipopowerups _tipopowerups;
    void Start()
    {
      hongo = this;
    
          Init();
          controller.horizontalcolision +=OnHorizontalCollisionEnter;
		controller.verticalcolision +=OnVerticalCollisionEnter;
  
 
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

public void Accionflor(){

  StartCoroutine (flor ());


}

void Cambiodedireccion(){


speed *=-1;

}
public void Destroy(){


 Destroy(this.gameObject);

 if(hongo !=null && hongo == this.hongo){

 controller.horizontalcolision -=OnHorizontalCollisionEnter;
		controller.verticalcolision -=OnVerticalCollisionEnter;

 }


}

IEnumerator AC(){




velocity.x = 0;

speed= 0f;

velocity.y = 1f;

gravity = 0;

controller.collisionMask = LayerMask.GetMask("Nothing");
yield return new WaitForSeconds (0.3f);

 velocity.y += gravity*Time.deltaTime;
gravity = -20;
controller.collisionMask = prevlayer;

speed = 4f;
velocity.x = speed;


}



IEnumerator flor(){

velocity.x = 0;

speed= 0f;

velocity.y = 1f;

gravity = 0;

controller.collisionMask = LayerMask.GetMask("Nothing");
yield return new WaitForSeconds (0.3f);

 velocity.y += gravity*Time.deltaTime;
gravity = -20;
controller.collisionMask = prevlayer;
}




	void OnVerticalCollisionEnter(Collider2D collider) {

    PlayerStates playerStates = collider.gameObject.GetComponent<PlayerStates>();

      	if (collider.tag == "Player") {
          
          
		   if(playerStates._estadosmario == estadosmario.Normal && _tipopowerups == tipopowerups.Hongo){
			//collider.gameObject.SendMessage ("Actualizarestado",1);

      playerStates.Actualizarestado(estadosmario.Grande);
        }

        	   if(playerStates._estadosmario != estadosmario.Fuego && _tipopowerups == tipopowerups.Flor){
			//collider.gameObject.SendMessage ("Actualizarestado",1);

      playerStates.Actualizarestado(estadosmario.Fuego);
             }


          
			Destroy();
		}

    if(collider.tag =="Ground" || collider.tag =="Obstaculo" ){

         grounded = true;
    }
	}

	void OnHorizontalCollisionEnter(Collider2D collider) {
        PlayerStates playerStates = collider.gameObject.GetComponent<PlayerStates>();

    	if (collider.tag == "Player") {
         
        if(playerStates._estadosmario == estadosmario.Normal){
			//collider.gameObject.SendMessage ("Actualizarestado",1);

      playerStates.Actualizarestado(estadosmario.Grande);
        }

          if(playerStates._estadosmario != estadosmario.Fuego && _tipopowerups == tipopowerups.Flor){
			//collider.gameObject.SendMessage ("Actualizarestado",1);

      playerStates.Actualizarestado(estadosmario.Fuego);
             }
			Destroy();
		}

    if(collider.tag == "Block"){
       
     if( _tipopowerups == tipopowerups.Hongo)
       Accion();

      if(_tipopowerups == tipopowerups.Flor)
        Accionflor();

    }

		if (collider.tag != "Player")
			speed *= -1f;
	}






}
