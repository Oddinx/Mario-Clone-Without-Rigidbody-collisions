using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goombamovement : Enemigo
{
    // Start is called before the first frame update



   
    void Start()
    {
    Init();
         controller.horizontalcolision +=OnHorizontalCollisionEnter;
         controller.verticalcolision +=OnVerticalCollisionEnter;
   
    }

    // Update is called once per frame
    void Update()
    {

     
      // controller.collisionMask &= ~(1 << 9);
      //velocity.x = speed;
      //velocity.y += gravity*Time.deltaTime;

  //input = Vector3.zero;
   
      //controller.Move(velocity*Time.deltaTime,input);  
      
  
//  Colisionmario();
 
//boxcast abajo

  


//fin boxcast


//Boxcast

    //boxcast

  





//Finboxcast




    }


//void Colisionmario(){



//BoxCollider2D Pcollider = Player.GetComponent<BoxCollider2D>();

//Bounds gbounds = boxCollider.bounds;

//Bounds pbounds = Pcollider.bounds;

//bool ycolision;

//gbounds.Expand( new Vector2 (0.01f,0.3f));
  //if(gbounds.Intersects(pbounds)){
    
 
        //if( pbounds.min.y > boxCollider.transform.position.y){

          //  ycolision = true;

          //  Destroy();
        //} else{

        //    ycolision = false;
       // }

      //if(ycolision == false){ 

      //if( gbounds.min.x > Pcollider.transform.position.x  ||
      // gbounds.max.x < Pcollider.transform.position.x )
      // {
            
           //  if(PlayerState.estado == 0){


            //     M.SendMessage("Death");
             //  }

           //   if(PlayerState.estado == 1){
             
         //   PlayerState.Actualizarestado(0);

       //    }


     // }


   //   }

  //} else{

    //  ycolision = false;
  //}



//}
    void Destroy(){


   Manager._manager.Desuscripcion();

	StartCoroutine (Muerte ());

    

    }

  void OnVerticalCollisionEnter(Collider2D collider) {

      	if (collider.tag == "Player") {

            

         
			   Destroy();
         Debug.Log("Destruido");
		}

   
	}



	public void OnHorizontalCollisionEnter(Collider2D collider) {

          

    	if (collider.tag == "Player") {

             if(playerStates.estado == 0){


            collider.SendMessage("Death");

           controller.collisionMask &= ~(1 << 11);
            }

           if(playerStates.estado == 1){
             
          playerStates.Actualizarestado(0);

          collider.SendMessage ("TakeDamage", SendMessageOptions.DontRequireReceiver);

          StartCoroutine(inmune());

          

         }
          

           
			
		}

 

		if (collider.tag == "Obstaculo" || collider.tag == "Ground" ){
			velocidadenemigo.speed *= -1f;

    

        }
	}



 public IEnumerator inmune(){
     controller.collisionMask &= ~(1 << 11);
  yield return new WaitForSeconds(1.2f);

  controller.collisionMask |= (1 << 11);

 }

    	public IEnumerator Muerte()
	{

	velocidadenemigo.velocity = Vector2.zero;
		anim.SetBool("Muerte", true);
	
	
		yield return new WaitForSeconds(0.08f);
		Destroy(gameObject);
	}
}
