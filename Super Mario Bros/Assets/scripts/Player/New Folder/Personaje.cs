using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personaje : MonoBehaviour
{
    protected Controller2D controller;

     protected  PlayerStates playerStates;

    protected   	Animator anim;

 protected   BoxCollider2D boxCollider;

     protected SpriteRenderer  _renderer;
	protected   float gravity;
      public float mass = 1f,contadorapplyforce;

  public float maxJumpHeight =4f;
	public float minJumpHeight = 1;
	public float timeToJumpApex = .4f;

  protected	float maxJumpVelocity;
protected	float minJumpVelocity;

  public	Vector2 velocity,acceleration;

  protected Vector2 input;

   protected GameObject Player;


   protected   Player player;
   

  protected float xSpeed = 5f;

  protected float ySpeed = 500f;
  
protected float runTime = 1f;

protected float runningMultiplyer = 2f;

public float   _runningTimer;

protected float  jumpBoost;

protected float   _jumpTimer;

protected float longJumpWeight = 0.1f;

public float longJumpTime = 1f;
     public void Init(){



        		controller = GetComponent<Controller2D> ();
        anim = GetComponent<Animator>();
		boxCollider = GetComponent<BoxCollider2D>();
		
		gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);


   controller.collisionMask &= ~(1 << 11);
    controller.collisionMask &= ~(1 << 15);
		
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;

	
		minJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (gravity) * minJumpHeight);



     Player = GameObject.FindGameObjectWithTag("Player");



       player = Player.GetComponent<Player>();
        	 playerStates = Player.GetComponent<PlayerStates>();

		 	_renderer = GetComponent<SpriteRenderer> ();

		
      }
  

  public void gravitysetter(float mjump){
    maxJumpHeight = mjump;

    	gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);
	    maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (gravity) * minJumpHeight);


  }

  public void OnPauseGame() {

	    player.enabled = false;

		controller.enabled = false;
	
		//anim.enabled = false;
	}

public void OnResumeGame() {
         player.enabled = true;
	
		controller.enabled = true;

		//anim.enabled = true;
	}





      public void ApplyForce(Vector2 force) {

		if(mass != 0f)
			force /= mass;
		
		acceleration += force;
	}


}
