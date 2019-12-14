using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goombamovement : MonoBehaviour
{



    float gravity = -20;

    Vector3 velocity,m;

    Controller2D controller;

    Animator animator;

  public float speed = 1f;



  GameObject Player;



BoxCollider2D boxCollider;

PlayerStates PlayerState;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<Controller2D>();

        animator = GetComponent<Animator>();
     
     boxCollider = GetComponent<BoxCollider2D>();
     

               Player = GameObject.FindGameObjectWithTag("Player");

        
        PlayerState = Player.GetComponent<PlayerStates>();

 
    }

    // Update is called once per frame
    void Update()
    {
       controller.collisionMask &= ~(1 << 9);
      velocity.x = speed;
      velocity.y += gravity*Time.deltaTime;

      controller.Move(velocity*Time.deltaTime,m);  
      
  
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


	StartCoroutine (Muerte ());

    }


  void OnVerticalCollisionEnter(Collider2D collider) {

      	if (collider.tag == "Player") {

			
			Destroy();
		}

   
	}

	void OnHorizontalCollisionEnter(Collider2D collider) {

    	if (collider.tag == "Player") {

             if(PlayerState.estado == 0){


            collider.SendMessage("Death");
            }

           if(PlayerState.estado == 1){
             
          PlayerState.Actualizarestado(0);

          collider.SendMessage ("TakeDamage", SendMessageOptions.DontRequireReceiver);

          

         }
          

           
			
		}

 

		if (collider.tag == "Obstaculo" || collider.tag == "Ground" ){
			speed *= -1f;

        }
	}


    	public IEnumerator Muerte()
	{

	velocity = Vector2.zero;
		animator.SetBool("Muerte", true);
	
	
		yield return new WaitForSeconds(0.08f);
		Destroy(gameObject);
	}
}
