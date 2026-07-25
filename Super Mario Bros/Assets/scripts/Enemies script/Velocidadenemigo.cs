using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Velocidadenemigo : MonoBehaviour 
    
{

     public	Vector2 velocity,acceleration;

     public Vector2 input;

 
     	public  float gravity;
      public float speed = 4f;

       Controller2D controller;

       public float mass = 1f,contadorapplyforce;
    private Transform playerTransform;
    public bool isActivated = false;

    // Start is called before the first frame update
    void Start()
    {

        controller = GetComponent<Controller2D> ();
      
      gravity = -50;
      
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) playerTransform = p.transform;
    }

    // Update is called once per frame
    void Update()
    {   
      if (!isActivated) {
          if (playerTransform != null && Mathf.Abs(playerTransform.position.x - transform.position.x) < 15f) {
              isActivated = true;
          }
      }

      controller.collisionMask &= ~(1 << 9);
      
      if (isActivated) {
          velocity.x = speed;
      } else {
          velocity.x = 0;
      }

       input = Vector3.zero;

      controller.Move(velocity*Time.deltaTime,input); 

    if(velocity.x > 0.1f){

	transform.localScale = new Vector3(1f,1f,1f);

 

      }

   if(velocity.x < -0.1f){
	transform.localScale = new Vector3(-1f,1f,1f);
   
   
    }

      velocity.y += gravity * Time.deltaTime;

        
    }


       public void ApplyForce(Vector2 force) {

		if(mass != 0f)
			force /= mass;
		
		acceleration += force;
	}


}
