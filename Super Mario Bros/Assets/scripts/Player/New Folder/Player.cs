
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

	// SMB1 VerticalForce ($0709) / VerticalForceDown ($070a)
	// Falling is 1.6x faster than rising — gives the snappy Mario feel
	const float GRAVITY_FALL_MULTIPLIER = 1.6f;

	// SMB1 Hold-to-float jump physics constants
	const float INITIAL_JUMP_VELOCITY = 19f;
	const float JUMP_HOLD_GRAVITY_WALK = -50f;
	const float JUMP_HOLD_GRAVITY_RUN = -36f;
	const float JUMP_RELEASE_GRAVITY = -100f;
	bool isHoldingJump = false;
	float currentJumpGravity = -50f;

	// SMB1 FrictionAdderHigh ($0701) / FrictionAdderLow ($0702)
	// Deceleration rate when releasing horizontal input on the ground
	const float GROUND_FRICTION = 28f;

	// SMB1 MaximumRightSpeed / MaximumLeftSpeed ($0450/$0456)
	// Three distinct speed caps matching the original game feel
	const float MAX_WALK_SPEED   = 5.5f;  // no run button held
	const float MAX_RUN_SPEED    = 9.0f;  // Z held, before sprint threshold
	const float MAX_SPRINT_SPEED = 11.0f; // Z held + RunningTimer >= runTime
	[HideInInspector] public float currentMaxSpeed = 5.5f; // tracked for clamping

	// SMB1 StarInvincibleTimer ($079f)
	[HideInInspector] public float starTimer = 0f;
	const float STAR_DURATION = 10f;       // ~10 seconds like the original
	bool isStarInvincible => starTimer > 0f;

    public float timerfinal;

     public bool buttoninactive;
	   public KeyCode run; 
 public bool final,Intangible,grounded = true,lanzarcaparazon,choquefinal,bounce,muerto;

 // InjuryTimer: invincibility frames after taking damage (mirrors SMB1 $079e)
 // Original uses ~2 seconds (~128 frames at 60fps). We use the same duration.
 [HideInInspector] public float injuryTimer = 0f;
 const float INJURY_DURATION = 2f; // seconds of post-damage invincibility
 bool isInjured => injuryTimer > 0f;

bool checkexit;

	public Sprite spriteAgachadoGrande;
	public Sprite spriteAgachadoFuego;
	[HideInInspector] public bool isDucking = false;
	private Vector2 originalColliderSize;
	private Vector2 originalColliderOffset;
	private bool colliderSaved = false;


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

		// InjuryTimer countdown (SMB1 $079e) — blink effect while invincible
		if(injuryTimer > 0f) {
			injuryTimer -= Time.deltaTime;
			// Blink every 0.1s while injury timer is active
			_renderer.enabled = (Mathf.FloorToInt(injuryTimer / 0.1f) % 2 == 0);
			if(injuryTimer <= 0f) {
				injuryTimer = 0f;
				_renderer.enabled = true;
			}
		}

		// StarInvincibleTimer countdown (SMB1 $079f) — faster blink effect
		if(starTimer > 0f) {
			starTimer -= Time.deltaTime;
			// Fast color cycle: blink every 0.05s (twice as fast as injury)
			_renderer.enabled = (Mathf.FloorToInt(starTimer / 0.05f) % 2 == 0);
			if(starTimer <= 0f) {
				starTimer = 0f;
				_renderer.enabled = true;
			}
		}
 
 
	 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		// Handle Ducking (Agacharse)
		if (playerStates != null && (playerStates._estadosmario == estadosmario.Grande || playerStates._estadosmario == estadosmario.Fuego)) {
			if (input.y < -0.1f && grounded) {
				if (!isDucking) {
					isDucking = true;
					if (!colliderSaved) {
						originalColliderSize = boxCollider.size;
						originalColliderOffset = boxCollider.offset;
						colliderSaved = true;
					}
					boxCollider.size = new Vector2(originalColliderSize.x, originalColliderSize.y / 2f);
					boxCollider.offset = new Vector2(originalColliderOffset.x, originalColliderOffset.y - (originalColliderSize.y / 4f));
					anim.enabled = false;
					if (playerStates._estadosmario == estadosmario.Grande && spriteAgachadoGrande != null) {
						_renderer.sprite = spriteAgachadoGrande;
					} else if (playerStates._estadosmario == estadosmario.Fuego && spriteAgachadoFuego != null) {
						_renderer.sprite = spriteAgachadoFuego;
					}
				}
			} else {
				if (isDucking) {
					isDucking = false;
					boxCollider.size = originalColliderSize;
					boxCollider.offset = originalColliderOffset;
					anim.enabled = true;
				}
			}
		} else if (isDucking) {
			isDucking = false;
			boxCollider.size = originalColliderSize;
			boxCollider.offset = originalColliderOffset;
			anim.enabled = true;
		}

		if (isDucking) {
			input.x = 0; // Cannot add new speed while ducking, but momentum is preserved and handled by friction
		}

		int wallDirX = (controller.collisions.left) ? -1 : 1;

       moveSpeed = Definirvelocidad();
		float targetVelocityX = input.x * moveSpeed;

		bool pushingAgainstVelocity = (input.x > 0.01f && velocity.x < -0.01f) || (input.x < -0.01f && velocity.x > 0.01f);
		float accelRate = 0f;
		if(controller.collisions.below) {
			accelRate = pushingAgainstVelocity ? 60f : 16f; // Ground acceleration / skidding
		} else {
			// Air acceleration: if pushing opposite direction, we turn around faster.
			// If letting go of the D-pad (input.x == 0), keep momentum (accelRate = 0).
			if (Mathf.Abs(input.x) < 0.01f) {
				accelRate = 0f; 
			} else {
				accelRate = pushingAgainstVelocity ? 60f : 12f;
			}
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

			
		// Clamp horizontal velocity to current max speed (SMB1 MaximumRightSpeed/Left $0450/$0456)
		velocity.x = Mathf.Clamp(velocity.x, -currentMaxSpeed, currentMaxSpeed);

	}else{

		// SMB1 FrictionAdderHigh/Low ($0701/$0702) & Acceleration:
		// Linear acceleration/deceleration feels natural and matches SMB1 unlike SmoothDamp
		if(controller.collisions.below && Mathf.Abs(input.x) < 0.01f) {
			velocity.x = Mathf.MoveTowards(velocity.x, 0f, GROUND_FRICTION * Time.deltaTime);
		} else {
			velocity.x = Mathf.MoveTowards(velocity.x, targetVelocityX, accelRate * Time.deltaTime);
		}

		// Clamp horizontal velocity to current max speed
		velocity.x = Mathf.Clamp(velocity.x, -currentMaxSpeed, currentMaxSpeed);

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
				velocity.y = INITIAL_JUMP_VELOCITY;
				isHoldingJump = true;
				
				// Determine how much gravity resists the jump based on horizontal speed
				if(Mathf.Abs(velocity.x) < 6f) {
					currentJumpGravity = JUMP_HOLD_GRAVITY_WALK; // Heavier (shorter jump)
				} else {
					currentJumpGravity = JUMP_HOLD_GRAVITY_RUN;  // Lighter (higher/longer jump)
				}
			}
		}
		
		if (Input.GetKeyUp (KeyCode.Space) ) {
			isHoldingJump = false;
		}

		// SMB1 Variable Jump Gravity:
		if(isHoldingJump && velocity.y > 0f) {
			// Player is holding jump and moving up — apply floaty gravity
			velocity.y += currentJumpGravity * Time.deltaTime;
		} else if (!isHoldingJump && velocity.y > 0f) {
			// Player released jump early while still moving up — apply heavy gravity to halt
			velocity.y += JUMP_RELEASE_GRAVITY * Time.deltaTime;
		} else {
			// Falling — apply fall multiplier
			velocity.y += gravity * GRAVITY_FALL_MULTIPLIER * Time.deltaTime;
		}

	
		controller.Move (velocity * Time.deltaTime, input);
           velocity += acceleration;
       	   
   
         
		if (controller.collisions.above || controller.collisions.below) {
			velocity.y = 0;
		}

      //Contador de tiempo de cuando el bototn correr no esta presionado
  


//Funciones para correr

     //boxcast




	 //finboxcast

 // Si hemos aplastado algun enemigo, Mario hara un pequeño salto ,cuando el contador sea mayor a 0.1 la fuerza aplicada para ese pequeño salto se desactivara y el contador también se desactivara	 

BounceActivo();


	}
  

 public float Definirvelocidad(){

		// SMB1 MaximumRightSpeed ($0450) / MaximumLeftSpeed ($0456)
		// Three speed tiers instead of a multiplier chain
		if (Input.GetKey(KeyCode.Z)) {

			if (grounded)
				_runningTimer += Time.fixedDeltaTime;

			_runningTimer = Mathf.Clamp(_runningTimer, 0f, 2f);

			if (_runningTimer >= runTime) {
				// Full sprint — held Z long enough
				currentMaxSpeed = MAX_SPRINT_SPEED;
			} else {
				// Running — Z held but sprint not yet reached
				currentMaxSpeed = MAX_RUN_SPEED;
			}
		} else {
			if (Input.GetKeyUp(KeyCode.Z))
				_runningTimer = 0f;

			// Gradually lower max speed back to walk when Z released
			// (mirrors how SMB1 doesn't hard-cut the speed on release)
			currentMaxSpeed = Mathf.MoveTowards(currentMaxSpeed, MAX_WALK_SPEED, 8f * Time.deltaTime);
		}

		return currentMaxSpeed;
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

// Activa el timer de estrella (SMB1 $079f)
public void ActivarEstrella(){
	starTimer = STAR_DURATION;
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

Manager._manager.Disminuirvidas();

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

		// StarInvincibleTimer (SMB1 $079f): kill any enemy on lateral contact
		if(isStarInvincible) {
			colisionenemigo.Destroy();
			return;
		}

		if( colisionenemigo._tipoEnemigos == tipoenemigos.Goomba){

		
                if(boxCollider.bounds.min.y > collider.bounds.max.y){
			   
			   
               
				colisionenemigo.Destroy();

			

				
				
				}

		 }
           



		 if(playerStates._estadosmario != estadosmario.Normal  && boxCollider.bounds.min.y < collider.bounds.max.y  ){

           if(lanzarcaparazon != true && !isInjured){
            // Mario is Grande or Fuego — shrink and start InjuryTimer
			injuryTimer = INJURY_DURATION;
			StartCoroutine(invencible());
			 playerStates.Actualizarestado(estadosmario.Normal);
		   }
			
		 }else if(boxCollider.bounds.min.y < collider.bounds.max.y ){
           if(lanzarcaparazon != true && !isInjured){
			 // Mario is Normal — die (no InjuryTimer, death is instant)
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
		// Block bounce: Force velocity down immediately to prevent clipping through
		velocity.y = -2f;
						
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