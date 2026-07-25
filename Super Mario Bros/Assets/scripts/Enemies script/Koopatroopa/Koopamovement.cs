using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Koopamovement :Enemigo
{


    public float contador,contadorgiro;

    public  bool  girando,daño,capes = false;

     
    // Start is called before the first frame update
    void Start()
    {
          Init();

       
       
    }

    // Update is called once per frame
    void Update()
    {   
      // controller.collisionMask &= ~(1 << 9);
      //  velocity.x = speed;
      //velocity.y += gravity*Time.deltaTime;

      //controller.Move(velocity*Time.deltaTime,input); 

      Actgiro();

      Contador(capes);   
      
      Stomp();

      
           // if(speed > 0.1){

	//transform.localScale = new Vector3(1f,1f,1f);

 

    //   }

   //if(speed < -0.1f){
	//transform.localScale = new Vector3(-1f,1f,1f);
   
   
    // }

        
    }



  void Stomp(){

      
       

  }



  void Actgiro(){

  BoxCollider2D pcollider = Player.GetComponent<BoxCollider2D>();

   Bounds koopabounds = boxCollider.bounds;


   Bounds Playerbounds = pcollider.bounds;

      if(koopabounds.Intersects(Playerbounds)){

   

        if(Playerbounds.min.y > boxCollider.transform.position.y){

            capes = true;

            girando = false;

             anim.SetBool("girar",false);

            velocidadenemigo.speed = 0;
           

        }

   
        if(capes){
        	
         if(koopabounds.min.x > pcollider.transform.position.x && koopabounds.max.y > Playerbounds.min.y){

           girando = true;
       
           

         

           velocidadenemigo.speed = 16f;

           anim.SetBool("girar",true);

          

         }else if(koopabounds.max.x < pcollider.transform.position.x && koopabounds.max.y > Playerbounds.min.y){

           girando = true;

           



           velocidadenemigo.speed = -16f;

           anim.SetBool("girar",true);
           
         


         }

         }


       }
          
           

     



      }


  



    	void OnHorizontalCollisionEnter(Collider2D collider) {

    
           
           if (collider.tag == "Player") {
            
            if(capes != true){


             if(playerStates.estado == 0){


            collider.SendMessage ("Death", SendMessageOptions.DontRequireReceiver);
            }

           if(playerStates.estado == 1){
             
          playerStates.Actualizarestado(0);

         }
          
           } 
           
			
		}
 

		if (collider.tag == "Obstaculo" || collider.tag == "Ground" ){
			   velocidadenemigo.speed *= -1f;


        }
	}







 void OnVerticalCollisionEnter(Collider2D collider) {

      

      	if (collider.tag == "Player") {

         
			Debug.Log("Tortuga");
			

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
     velocidadenemigo.speed = 0;
  
    anim.ResetTrigger("normal");

   anim.SetTrigger("escondido");
  anim.SetBool("girar",false);

  }



 
 
  }else{

    anim.ResetTrigger("escondido");

    contador = 0;

    player.DesactivarLanzamiento();
  }


  if(girando !=true){

  if(contador == 9  || contador >= 9){
      capes = false;
       velocidadenemigo.speed = 3f;

       

       anim.SetTrigger("normal");

  }else if( contador <= 7  && contador > 6){

    

     anim.SetTrigger("saliendo");

     

  }

 }

}


  

}
