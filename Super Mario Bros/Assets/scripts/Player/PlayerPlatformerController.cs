using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject
{


     public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;
    
 
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    

    private Rigidbody2D rb;
 
	public float   _runningTimer = 0;
 
  public float jumprun = 8f;
public bool jumpboost = false;

   public float rundecend = 0;

     public bool buttoninactive;

  


 //Estados

 // 0 pequeño // 1 grande // fuego


 

   Vector2 move;


    // Use this for initialization
    void Awake () 
    {
        spriteRenderer = GetComponent<SpriteRenderer> ();    
        animator = GetComponent<Animator> ();


    }



    protected override void ComputeVelocity()
    {
       
         //Boxcast

       


        
     // Fin Boxcast


       move = Vector2.zero;

        move.x = Input.GetAxis ("Horizontal");
        

        //Salto
        if (Input.GetButtonDown ("Jump") && grounded) {
            velocity.y = jumpTakeOffSpeed*1.36f;
        } else if (Input.GetButtonUp ("Jump")) 
        {
            if (velocity.y > 0) {
                velocity.y = velocity.y * 0.5f;
            }
        }

//Fin salto





        
        if (move.x > 0.1f) {
			transform.localScale = new Vector3(1f, 1f, 1f);
		} 

		if (move.x < -0.1f){
			transform.localScale = new Vector3(-1f, 1f, 1f);
		}
        animator.SetBool ("Grounded", grounded);
        animator.SetFloat ("Speed", Mathf.Abs (velocity.x) / maxSpeed);

        targetVelocity = move * maxSpeed;

//Raycaspisada



//fin raycast pisada

     if(buttoninactive == true){
 rundecend += Time.deltaTime;

 if(rundecend > 3){


    buttoninactive = false;
    rundecend = 0;
    maxSpeed = 7f;
 } else if( rundecend > 2){

         maxSpeed = 10f;
       }else if(rundecend > 1){

         maxSpeed = 13f;
       }

      
}
                //Correr
   if( Input.GetKey(KeyCode.Z ) && grounded){
    
 
       
         
        _runningTimer += Time.deltaTime;
         
       rundecend = 0;
       
        
             if( _runningTimer > 2){

          maxSpeed = 15f;
        }else if( _runningTimer > 1){
           maxSpeed = 12f;
           
       } else if( _runningTimer >0.3f){ 

         maxSpeed = 8f;
       }
        

        if(Input.GetButtonDown("Jump")){
          if(_runningTimer > 0.2f){


         jumpboost = true;

          }else if(_runningTimer < 0.2f){

              jumpboost = false;
          }


          if(jumpboost == true){

                jumpTakeOffSpeed = 9;
      


          } else if(jumpboost == false){

            jumpTakeOffSpeed = 7;

          }
        }
               
              

            // Debug.Log(velocity.magnitude); 
    
        
   


      


   }else if(Input.GetKeyUp(KeyCode.Z) && grounded || grounded){
        
       _runningTimer = 0f; 
       
       buttoninactive = true;

     

        //Debug.Log(velocity.magnitude); 
   } else{
      
        jumpboost = false;

   }



        //fin correr

     




}













    }

