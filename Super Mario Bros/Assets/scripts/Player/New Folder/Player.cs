
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalTypes;
using UnityEngine.SceneManagement;
[RequireComponent (typeof (Controller2D))]


public class Player : Personaje
	
{
	float accelerationTimeAirborne = .4f;
	float accelerationTimeAirborneAgainst = .7f; // Resistance when pushing opposite direction in air
	float accelerationTimeGrounded = .1f;
	public float moveSpeed = 6;
	float velocityXSmoothing;

    public float timerfinal;

     public bool buttoninactive;
	   public KeyCode run; 
 public bool final,Intangible,grounded = true,lanzarcaparazon,choquefinal,bounce,muerto;


bool checkexit;


	void Start() {

	     
	   //Inicializa la gravedad,boxcollider,animator y demás cosas
     Init();

	      
		final = false;

		choquefinal = false;

		controller.horizontalcolision +=OnHorizontalCollisionEnter;
		controller.verticalcolision +=OnVerticalCollisionEnter;

		controller.horizontalTrigger +=OnHorizontalTriggerEnter;

		controller.verticallTrigger += OnVerticalTriggerEnter;

        Manager._manager.muertemario += Death;
	
	}

	void Update() {

		// Si mario está muerto, no procesar ningún input ni movimiento
		if(muerto) {
			velocity.y += gravity * Time.deltaTime;
			controller.Move(velocity * Time.deltaTime, Vector2.zero);
			return;
		}
 
 
	 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		int wallDirX = (controller.collisions.left) ? -1 : 1;

       moveSpeed = Definirvelocidad();
		float targetVelocityX = input.x * moveSpeed;

		// Aerial momentum: when pushing against current direction, apply more resistance
		float smoothTime;
		if(controller.collisions.below) {
			smoothTime = accelerationTimeGrounded;
		} else {
			// If input opposes current horizontal velocity, use higher smoothing time
			bool pushingAgainstVelocity = (input.x > 0.01f && velocity.x < -0.01f) || (input.x < -0.01f && velocity.x > 0.01f);
			smoothTime = pushingAgainstVelocity ? accelerationTimeAirborneAgainst : accelerationTimeAirborne;
		}

	if( final){

		velocity.x = 0;

		input.x = 0;

		controller.collisionMask &= ~(1 << 13);

		


		if(choquefinal != true){

         velocity.y = -5f;

		 anim.SetBool("BajarBandera",true);

		}else{

			anim.SetBool("BajarBandera",false);

			timerfinal += Time.deltaTime;
            
			if(timerfinal >=3){
			 velocity.x = 0;
			  SceneManager.LoadScene(0,LoadSceneMode.Single);

			}else if(timerfinal >=2){
			velocity.x = 6f;

			}else if(timerfinal < 2){

				velocity.x = 0f;
			}
		}

			
	}else{

		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, smoothTime);

		input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
	}


     
//CHECHG Detecta si estamos tocando el suelo o si estamos saltando
checkg();

if(grounded){

anim.SetFloat ("Speed",Mathf.Abs(velocity.x));

}


anim.SetBool("Grounded",grounded);



if(input.x > 0.1){

	transform.localScale = new Vector3(1f,1f,1f);

 

}

if(input.x < -0.1f){
	transform.localScale = new Vector3(-1f,1f,1f);
   
   
}





	if (Input.GetKeyDown (KeyCode.Space)) {
        
			
		
			if (controller.collisions.below ) {
				velocity.y = maxJumpVelocity;
				
				
			}

			  
		
			
		}
		if (Input.GetKeyUp (KeyCode.Space) ) {
			
			if (velocity.y > minJumpVelocity) {
				velocity.y = minJumpVelocity;
                 
				 
				
			}
		
		}

		velocity.y += gravity * Time.deltaTime;

	
		controller.Move (velocity * Time.deltaTime, input);
           velocity += acceleration;
       	   
   
         
		if (controller.collisions.above || controller.collisions.below) {
			velocity.y = 0;
		}

      //Contador de tiempo de cuando el bototn correr no esta presionado
  


//Funciones para correr


Jump();


     //boxcast




	 //finboxcast

 // Si hemos aplastado algun enemigo, Mario hara un pequeño salto ,cuando el contador sea mayor a 0.1 la fuerza aplicada para ese pequeño salto se desactivara y el contador también se desactivara	 

BounceActivo();


	}
  

 public float Definirvelocidad(){

   	float speed = xSpeed;
		if (Input.GetKey (KeyCode.Z)) {

			speed *= runningMultiplyer;

			if (grounded)
				_runningTimer += Time.fixedDeltaTime;

			_runningTimer = Mathf.Clamp (_runningTimer, 0f, 2f);

			if (_runningTimer >= runTime)
				speed *= runningMultiplyer * 0.625f;
		} 
		else if (Input.GetKeyUp (KeyCode.Z)) {

			_runningTimer = 0f;
		}

		return speed;




  }


  void Jump(){

        if(_runningTimer > 1){

        gravitysetter(5.5f);
		} else if(_runningTimer > 0.5f){
         

	     gravitysetter(5f);
           
       } else if(_runningTimer  >0.1f){

         

	     

		 gravitysetter(4.5f);
		
       }else if(_runningTimer  == 0){

         gravitysetter(4f);

		  
	  }
       

      

  }




public void Death(){

StartCoroutine(Muerte());


		
}




	void Bounce(){

    
    bounce = true;

		//ApplyForce(Vector2.up*4f);



	}



	void BounceActivo(){
       
	  if(bounce){
		 velocity.y = Mathf.Max(velocity.y, 9f);
		 bounce = false;
	  }

	}



public void Corutina(){

	StartCoroutine(invencible());

}
IEnumerator invencible(){
	
anim.SetTrigger("Daño");
boxCollider.isTrigger = true;
controller.collisionMask &= ~(1 << 9);	

  for(var i = 0; i < 10; i++){
  _renderer.enabled = false;
    yield return new WaitForSeconds (0.1f);
     _renderer.enabled = true;
       yield return new WaitForSeconds (0.1f);

  }


  boxCollider.isTrigger = false;
	controller.collisionMask |= (1 << 9);

	Debug.Log("Fin corutina");

}




public IEnumerator Muerte(){


anim.SetBool("Death",true);
muerto = true;

boxCollider.enabled= false;

velocity.x = 0;

moveSpeed = 0f;

gravity = -17f;



controller.collisionMask = 0;

yield return new WaitForSeconds(0.1f);
transform.position = new Vector3(transform.position.x,2f,-2f);

yield return new WaitForSeconds(2f);

  SceneManager.LoadScene(1,LoadSceneMode.Single);

}





void OnHorizontalCollisionEnter(Collider2D collider) {
        
        _runningTimer = 0;

		if(collider.tag == "Ground" || collider.tag == "Obstaculo" ){
           
		   moveSpeed = 6.03f;

		   maxJumpHeight = 4;

		   buttoninactive = false;

		   
		}

		
		
			
	}




	void OnHorizontalTriggerEnter(Collider2D collider) {
        
     Hongo hongo = collider.gameObject.GetComponent<Hongo>();

	 Colisionenemigo colisionenemigo = collider.GetComponent<Colisionenemigo>();


	  if (collider.tag == "Hongo" && playerStates._estadosmario == estadosmario.Normal && hongo._tipopowerups == tipopowerups.Hongo) {

		    Debug.Log("Triggereando");
        
		
	  
		playerStates.Actualizarestado(estadosmario.Grande);
	
     //collider.SendMessage ("Destroy", SendMessageOptions.DontRequireReceiver);
	 hongo.Destroy();
	
		
		
		}else if(collider.tag == "Hongo" && hongo._tipopowerups == tipopowerups.Hongo){

			//collider.SendMessage ("Destroy", SendMessageOptions.DontRequireReceiver);

			hongo.Destroy();
			
		}

		if(collider.tag == "Hongo" && playerStates._estadosmario != estadosmario.Fuego && hongo._tipopowerups == tipopowerups.Flor){

          playerStates.Actualizarestado(estadosmario.Fuego);

		   hongo.Destroy();
	
		
		

		}else if(collider.tag == "Hongo" && hongo._tipopowerups == tipopowerups.Flor){

			//collider.SendMessage ("Destroy", SendMessageOptions.DontRequireReceiver);

			hongo.Destroy();
			
		}

		if(collider.tag == "Final"){


			
			 final = true;    

			collider.SendMessage("Bajarbandera",SendMessageOptions.DontRequireReceiver);      
		  

	

		}


		if(collider.tag =="Enemigo"){

		
        
		if( colisionenemigo._tipoEnemigos == tipoenemigos.Goomba){

		
                if(boxCollider.bounds.min.y > collider.bounds.max.y){
			   
			   
               
				colisionenemigo.Destroy();

			

				
				
				}

		 }
           



		 if(playerStates._estadosmario != estadosmario.Normal  && boxCollider.bounds.min.y < collider.bounds.max.y  ){

           if(lanzarcaparazon != true){
            //TakeDamage ();

			StartCoroutine(invencible());
			 playerStates.Actualizarestado(estadosmario.Normal);
		   }
			
		 }else if(boxCollider.bounds.min.y < collider.bounds.max.y ){
           if(lanzarcaparazon != true){
			 Death();
             
		   }
		 }

		



		}



	}


		void OnVerticalTriggerEnter(Collider2D collider){
			     Hongo hongo = collider.gameObject.GetComponent<Hongo>();

	   Colisionenemigo colisionenemigo = collider.GetComponent<Colisionenemigo>();
	    
	
             
			 Debug.Log("Triggereando");
			 if (collider.tag == "Hongo" && playerStates._estadosmario == estadosmario.Normal) {

           
		    playerStates.Actualizarestado(estadosmario.Grande);
		
	
	  

			//collider.SendMessage ("Destroy", SendMessageOptions.DontRequireReceiver);
			hongo.Destroy();


		}else if(collider.tag =="Hongo"){
            hongo.Destroy();

			

		}

			if(collider.tag == "Hongo" && playerStates._estadosmario != estadosmario.Fuego && hongo._tipopowerups == tipopowerups.Flor){

          playerStates.Actualizarestado(estadosmario.Fuego);

		   hongo.Destroy();
	
		
			

		}else if(collider.tag == "Hongo" && hongo._tipopowerups == tipopowerups.Flor){

			//collider.SendMessage ("Destroy", SendMessageOptions.DontRequireReceiver);

			hongo.Destroy();
			
		}

		if(collider.tag == "Final"){

			final = true;


		}
         
		 if(collider.tag == "Limitey"){

			 Death();
		 }

		if(collider.tag =="Enemigo"){

			
			
         
        	if(colisionenemigo._tipoEnemigos == tipoenemigos.Goomba ){

	       
	 if(boxCollider.bounds.min.y > collider.bounds.max.y){
		//collider.SendMessage ("Destroy", SendMessageOptions.DontRequireReceiver);

		

		colisionenemigo.Destroy();
		  Debug.Log("Goombeando");

		
		   
	 }
		      

			}
                Bounce();
			   
	

		}


     




		}



public void Activarlanzamiento(){


lanzarcaparazon = true;

}

public void DesactivarLanzamiento(){

	lanzarcaparazon = false;
}
// Detecta si estamos tocando el suelo o si estamos saltando
void checkg(){

	  
	
		if(controller.collisions.below){

			//grounded = true;
           
		   checkexit = false;
		
		}else{

			//grounded = false;
			//bounce = false;
			checkexit = true;

		   bounce = false;

		   

		}

	if(checkexit){

			grounded = false;
		}
	


}


	void OnVerticalCollisionEnter(Collider2D collider){				  

  Prizeblock prizeblock = collider.gameObject.GetComponent<Prizeblock>();
	
     if(collider.tag == "Block" && boxCollider.bounds.max.y < collider.bounds.min.y){

					  if(playerStates._estadosmario != estadosmario.Normal && prizeblock._tipobloque == tipobloque.Normal){
                         

                                prizeblock.Destroy();
				 }

				 if(prizeblock._tipobloque == tipobloque.Premio){

					 prizeblock.Rebotar();
				 }

	 }


				 	if(collider.gameObject.name == "BloqueBandera"){

             choquefinal = true;
			}
       

		
		  if(collider.tag == "Ground" || collider.tag == "Obstaculo" || collider.tag == "Block" || collider.tag == "Premio" && collider.bounds.max.y < boxCollider.bounds.min.y){


           

			 grounded = true;

		  }


		}








}