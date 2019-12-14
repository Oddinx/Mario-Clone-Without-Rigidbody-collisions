using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Koopamovement : MonoBehaviour
{
      public float speed = 1f;
    float gravity = -20;

    Vector2 velocity,m,acceleration;

    Controller2D controller;

    Animator animator;



     PlayerStates PlayerState;

     BoxCollider2D boxCollider;

     Player player;

     GameObject Player;

    public float mass = 1f,contador,contadorgiro;

    public  bool  girando,daño,capes = false;

    

     
    // Start is called before the first frame update
    void Start()
    {
          controller = GetComponent<Controller2D>();

        animator = GetComponent<Animator>();

        boxCollider = GetComponent <BoxCollider2D>();

       Player = GameObject.FindGameObjectWithTag("Player");

       player = Player.GetComponent<Player>();

        
        PlayerState = Player.GetComponent<PlayerStates>();
        
    }

    // Update is called once per frame
    void Update()
    {   controller.collisionMask &= ~(1 << 9);
        velocity.x = speed;
      velocity.y += gravity*Time.deltaTime;

      controller.Move(velocity*Time.deltaTime,m); 

      Actgiro();

      Contador(capes);   

       


      
            if(speed > 0.1){

	transform.localScale = new Vector3(1f,1f,1f);

 

       }

   if(speed < -0.1f){
	transform.localScale = new Vector3(-1f,1f,1f);
   
   
     }

        
    }


  void Actgiro(){

  BoxCollider2D pcollider = Player.GetComponent<BoxCollider2D>();

   Bounds koopabounds = boxCollider.bounds;


   Bounds Playerbounds = pcollider.bounds;

      if(koopabounds.Intersects(Playerbounds)){

   

        if(Playerbounds.min.y > boxCollider.transform.position.y){

            capes = true;

            girando = false;

             animator.SetBool("girar",false);

             speed = 0;
           

        }

   
        if(capes){
        	
         if(koopabounds.min.x > pcollider.transform.position.x){

           girando = true;
       
           

           ApplyForce (Vector2.right * 6f);

           speed = 16f;

           animator.SetBool("girar",true);

          

         }else if(koopabounds.max.x < pcollider.transform.position.x){

           girando = true;

           

             //ApplyForce (Vector2.left * 6f);

           speed = -16f;

           animator.SetBool("girar",true);
           
         


         }

         }


       }
          
           

     



      }


  



  public void ApplyForce(Vector2 force) {

		if(mass != 0f)
			force /= mass;
		
		acceleration += force;
	}

    	void OnHorizontalCollisionEnter(Collider2D collider) {

    
           
           if (collider.tag == "Player") {
            
            if(capes != true){


             if(PlayerState.estado == 0){


            collider.SendMessage ("Death", SendMessageOptions.DontRequireReceiver);
            }

           if(PlayerState.estado == 1){
             
          PlayerState.Actualizarestado(0);

         }
          
           } 
           
			
		}
 

		if (collider.tag == "Obstaculo" || collider.tag == "Ground" ){
			speed *= -1f;


        }
	}







 void OnVerticalCollisionEnter(Collider2D collider) {

      	if (collider.tag == "Player") {

         
			
			

		}

   
	}





void Contador(bool cont){

  if(girando){

    contadorgiro += Time.deltaTime;

    if(contadorgiro > 0.1){

      capes = false;
    }
  }else{

    contadorgiro = 0;
  }

  if(cont){
   
  contador += Time.deltaTime;

  	player.Activarlanzamiento();

 if( contador < 6  && girando!= true){
     speed = 0;


   animator.SetTrigger("escondido");
  animator.SetBool("girar",false);

  }



 
 
  }else{

    animator.ResetTrigger("escondido");

    contador = 0;

    player.DesactivarLanzamiento();
  }


  if(girando !=true){

  if(contador == 9  || contador >= 9){
      capes = false;
       speed = 3f;

       

       animator.SetTrigger("normal");

  }else if( contador <= 7  && contador > 6){

    

     animator.SetTrigger("saliendo");

     

  }

 }

}


  
     	public IEnumerator Escondido()
	{
    animator.SetBool("girar",false);
    girando = false;
    capes = true;
	speed =0;
	
    animator.SetTrigger("escondido");

   if( girando != true){

    yield return new WaitForSeconds(6f);
   animator.SetTrigger("saliendo");
   yield return new WaitForSeconds(0.5f);

    capes = false;

       speed =3f;

        animator.SetTrigger("normal");
   		
	}


  }
}
