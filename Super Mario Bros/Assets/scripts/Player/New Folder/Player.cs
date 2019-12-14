using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour{



//[SerializeField]   private  LayerMask groundLayerMask;

	public float maxJumpHeight = 4;
	public float minJumpHeight = 1;
	public float timeToJumpApex = .4f;
	float accelerationTimeAirborne = .2f;
	float accelerationTimeGrounded = .1f;
	public float moveSpeed = 6;





	float gravity;
	float maxJumpVelocity;
	float minJumpVelocity;
	Vector3 velocity,autovelocity;
	float velocityXSmoothing;

	Controller2D controller;

	Animator anim;

    BoxCollider2D boxCollider;

        public float timer = 0f,_blinkTimer,blinkTime = 0.1f,_blinkAmount,transtimer;

     public float rundecend = 0;

     public bool buttoninactive;

	   public KeyCode run; 



	

	  PlayerStates playerStates;


 public bool final,Intangible,grounded = true,lanzarcaparazon;
 protected SpriteRenderer  _renderer;


    

	void Start() {
		controller = GetComponent<Controller2D> ();
        anim = GetComponent<Animator>();
		boxCollider = GetComponent<BoxCollider2D>();
		gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (gravity) * minJumpHeight);

        
		

	

	  

		 playerStates = GetComponent<PlayerStates>();

		 	_renderer = GetComponent<SpriteRenderer> ();


	}

	void Update() {

 controller.collisionMask &= ~(1 << 11);
		
   setIntangible(Intangible);
		

       autovelocity.x = moveSpeed;
     

		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		int wallDirX = (controller.collisions.left) ? -1 : 1;

     
		float targetVelocityX = input.x * moveSpeed;

	velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);

	if( final){

			velocity.x = 0;

			input.x = 0;
	}else{

		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);

		input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
	}

//CHECHG Detecta si estamos tocando el suelo o si estamos saltando
checkg();

anim.SetFloat ("Speed",Mathf.Abs(velocity.x));


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

		if (controller.collisions.above || controller.collisions.below) {
			velocity.y = 0;
		}

      //Contador de tiempo de cuando el bototn correr no esta presionado
     if(buttoninactive == true){
 rundecend += Time.deltaTime;

 if(rundecend > 3){


    buttoninactive = false;
    rundecend = 0;
    moveSpeed = 6.03f;

	maxJumpHeight = 4f;
 } else if( rundecend > 2){

         moveSpeed= 8f;
		 maxJumpHeight = 5;
       }else if(rundecend > 1){

         moveSpeed= 12f;

		 maxJumpHeight = 5.5f;
       }

      
}

//Funciones para correr
     if(Input.GetKey(run) && grounded){
       timer += Time.deltaTime;
       rundecend = 0;
       buttoninactive = true;
        if(timer > 2){

         moveSpeed = 15f;

		 maxJumpHeight = 5.5f;

        }else if(timer > 1){
         moveSpeed = 12f;

		 maxJumpHeight = 5f;
           
       } else if(timer >0.3f){

         moveSpeed = 8f;

		 maxJumpHeight = 4.5f;
       }else if(timer == 0){

          moveSpeed = 6.03f;
	  }
       
       

     }else if(Input.GetKeyUp(run) && grounded){
 
      timer = 0;
      buttoninactive = true;

     

     }


     //boxcast




	 //finboxcast





	}
  






public void Death(){


StartCoroutine(Muerte());

}

void TakeDamage(){

anim.SetTrigger("Daño");



	Intangible = true;

    if(Intangible){


boxCollider.isTrigger = true;
controller.collisionMask &= ~(1 << 9);
}else {
			
			boxCollider.isTrigger = false;
			controller.collisionMask |= (1 << 9);
		}


}
void Blink(){
_blinkTimer += Time.fixedDeltaTime;

if (_blinkTimer >= blinkTime) {
			_renderer.enabled = !_renderer.enabled;
			_blinkTimer = 0f;
			_blinkAmount++;
		}

			if (_blinkAmount == 10)
			blinkTime *= 0.8f;

		else if (_blinkAmount == 20)
			blinkTime *= 0.8f;

		else if (_blinkAmount >= 40) {

			_blinkAmount = 0;
			_blinkTimer = 0f;

			Intangible = false;
			_renderer.enabled = true;
		}


}

void setIntangible(bool Intangible ){



if(Intangible){

Blink ();
//boxCollider.isTrigger = true;
//controller.collisionMask &= ~(1 << 9);
}//else {
			
	//		boxCollider.isTrigger = false;
	//		controller.collisionMask |= (1 << 9);
	//	}


}

public IEnumerator Muerte(){


anim.SetBool("Death",true);

boxCollider.enabled= false;

velocity.x = 0;

moveSpeed = 0f;

gravity = -17f;

controller.collisionMask = LayerMask.GetMask("Nothing");

yield return new WaitForSeconds(0.1f);
transform.position = new Vector3(transform.position.x,2f,-2f);

 

}

	   //private bool IsGrounded(){

      //Bounds bounds = boxCollider.bounds;
      // float extraHeightText = 1f;


     //RaycastHit2D raycastHit= Physics2D.BoxCast(bounds.center,new Vector2(0.5f,1.0f),0f,Vector2.down,extraHeightText,groundLayerMask);
      //Color rayColor; 

      //if(raycastHit.collider != null){

      //  rayColor = Color.green;

	
		
      //}else{

      //  rayColor = Color.red;
      //}
      //Debug.DrawRay(bounds.center + new Vector3(bounds.extents.x,0),Vector2.down*(bounds.extents.y+extraHeightText),rayColor);
      //Debug.DrawRay(bounds.center - new Vector3(bounds.extents.x,0),Vector2.down*(bounds.extents.y+extraHeightText),rayColor);
      //Debug.DrawRay(bounds.center - new Vector3(bounds.extents.x,bounds.extents.y),Vector2.right*(bounds.extents.y),rayColor);
    

  
      
     // return raycastHit.collider != null;

   // }

void OnHorizontalCollisionEnter(Collider2D collider) {
        
	
	
        timer = 0;

		if(collider.tag == "Ground" || collider.tag == "Obstaculo" ){
           
		   moveSpeed = 6.03f;
		}
		



		
	}




	void OnHorizontalTriggerEnter(Collider2D collider) {
        
    

	  if (collider.tag == "Hongo") {

		    Debug.Log("Triggereando");

		playerStates.Actualizarestado(1);
     collider.SendMessage ("Destroy", SendMessageOptions.DontRequireReceiver);
			

		}

		if(collider.tag == "Final"){
           
		   StartCoroutine(ganarnivel());

	

		}


		if(collider.tag =="Enemigo"){
        
		 if(collider.gameObject.name == "Goomba"){

		if (boxCollider.bounds.min.y > collider.bounds.max.y ) {

				collider.SendMessage ("Destroy", SendMessageOptions.DontRequireReceiver);
			}

		 }
           
		   

		 if(playerStates.estado == 1 && boxCollider.bounds.min.y < collider.bounds.max.y  ){

           if(lanzarcaparazon != true){
            TakeDamage ();
			 playerStates.Actualizarestado(0);
		   }
			
		 }else if(boxCollider.bounds.min.y < collider.bounds.max.y ){
           if(lanzarcaparazon != true){
			 Death();

		   }
		 }

		



		}



	}


		void OnVerticalTriggerEnter(Collider2D collider){
             
			 Debug.Log("Triggereando");
			 if (collider.tag == "Hongo") {

		    playerStates.Actualizarestado(1);

			
	

			collider.SendMessage ("Destroy", SendMessageOptions.DontRequireReceiver);

			

		}

		if(collider.tag == "Final"){
           
		   StartCoroutine(ganarnivel());

		}


		if(collider.tag =="Enemigo"){

		
        	if(collider.gameObject.name == "Goomba"){
	
		collider.SendMessage ("Destroy", SendMessageOptions.DontRequireReceiver);

			}

	

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


	if(velocity.y == 0){

		grounded = true;
	}else{

		grounded = false;
	}
}

	void OnVerticalCollisionEnter(Collider2D collider){				  

			

			  if(collider.tag == "Block" && boxCollider.bounds.max.y < collider.bounds.min.y){

					   if(playerStates.estado == 0){
                         
					 	collider.SendMessage ("movimiento", SendMessageOptions.DontRequireReceiver);

					   }else{

						   collider.SendMessage ("Destroy", SendMessageOptions.DontRequireReceiver);
					   }
				 }


		}

	


public IEnumerator ganarnivel(){

final = true;


velocity.y = -3f;
anim.SetBool("BajarBandera",true);

yield return new WaitForSeconds (1.5f);
anim.SetBool("BajarBandera",false);

controller.collisionMask = LayerMask.GetMask("Ground");

final = false;


velocity.x = 8f;


yield return new WaitForSeconds (1f);




}



}