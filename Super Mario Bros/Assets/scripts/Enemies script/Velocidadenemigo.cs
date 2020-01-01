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
    // Start is called before the first frame update
    void Start()
    {

        controller = GetComponent<Controller2D> ();
      
      gravity = -50;
      
        
    }

    // Update is called once per frame
    void Update()
    {   
      controller.collisionMask &= ~(1 << 9);
      velocity.x = speed;
       input = Vector3.zero;

      controller.Move(velocity*Time.deltaTime,input); 

    if(speed > 0.1){

	transform.localScale = new Vector3(1f,1f,1f);

 

      }

   if(speed < -0.1f){
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
