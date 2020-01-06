using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colisionenemigo : Enemigo
{

    
    public enum tipoenemigos{
      
      Goomba,
      Koopa

    }

    public tipoenemigos _tipoEnemigos;

    private Colisionenemigo colision;

    public float contador,contadorgiro, contm;

    public  bool  girando,daño,capes = false,contadormuerte;

   public float vars;
   
   

  

    // Start is called before the first frame update
    void Start()
    {
        Init();

    colision = this;

      controller.horizontalcolision +=OnHorizontalCollisionEnter;
         controller.verticalcolision +=OnVerticalCollisionEnter;
        
    }

    // Update is called once per frame
    void Update()
    {
      controller.collisionMask &= ~(1 << 12);

      Stomp();
       

  
       if (_tipoEnemigos.Equals(tipoenemigos.Koopa)){ 
     // Actgiro();

      Contador(capes); 

      

       }
    }

          

    void Stomp(){


      Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0);

        foreach (Collider2D hit in hits)
        {         
              if(hit == boxCollider)
              continue;

              if(hit.tag == "Enemigo" && !capes){
                   
                   velocidadenemigo.speed *= -1f;

              }
              if(hit.tag == "Player" && _tipoEnemigos.Equals(tipoenemigos.Koopa))  {

                
                  if( hit.transform.position.y >= boxCollider.bounds.max.y && !capes){

                     capes = true;

                      girando = false;

                     anim.SetBool("girar",false);

                    velocidadenemigo.speed = 0;
                    Debug.Log("Playereando");
                    
                    

                  }

                 else if(capes){
                        vars = Random.Range(0,2)*2-1;

                         if(hit.transform.position.y >= boxCollider.bounds.max.y){

                          girando = true;

                           velocidadenemigo.speed = vars*16f;

                          anim.SetBool("girar",true);

                         }

                     
                       
                  else if(boxCollider.bounds.min.x > hit.transform.position.x && boxCollider.bounds.max.y > hit.bounds.min.y ){
                            girando = true;
       
           

         

                         velocidadenemigo.speed = 16;

                          anim.SetBool("girar",true);
                           Debug.Log("CAPES");
                     }else if(boxCollider.bounds.min.x < hit.transform.position.x && boxCollider.bounds.max.y > hit.bounds.min.y){
                             girando = true;

           



                         velocidadenemigo.speed = -16f;

                          anim.SetBool("girar",true);
                          Debug.Log("SEPAC");
                     }



                 }
              }
        }
    }

 


       public void Destroy(){

    Manager._manager.Desuscripcion();
  

 if (_tipoEnemigos.Equals(tipoenemigos.Goomba)){

	StartCoroutine (Muerte ());
 }
      if(colision!=null && colision == this.colision){

	
    

     controller.horizontalcolision -=OnHorizontalCollisionEnter;

     controller.verticalcolision -=OnVerticalCollisionEnter;

   }
    

    }

    	void OnHorizontalCollisionEnter(Collider2D collider) {

       

      if(colision!=null && colision == this.colision)
           
           if (collider.tag == "Player") {
            
            if(capes != true){


             if(playerStates.estado == 0){

              
            //collider.SendMessage ("Death", SendMessageOptions.DontRequireReceiver);

            player.Death();
            }

           if(playerStates.estado == 1){
             
          playerStates.Actualizarestado(0);

          //collider.SendMessage ("TakeDamage", SendMessageOptions.DontRequireReceiver);

          player.TakeDamage();

          StartCoroutine(inmune());

         }
          
           } 
           
			
		}
 

		if (collider.tag == "Obstaculo" || collider.tag == "Ground" ){
			   velocidadenemigo.speed *= -1f;


        }
	}







 void OnVerticalCollisionEnter(Collider2D collider) {

   if(colision!=null && colision == this.colision)

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
       velocidadenemigo.speed = 4f;

       

       anim.SetTrigger("normal");

  }else if( contador <= 7  && contador > 6){

    

     anim.SetTrigger("saliendo");

     

  }

 }

}

 public IEnumerator inmune(){
     controller.collisionMask &= ~(1 << 11);
  yield return new WaitForSeconds(1.2f);

  controller.collisionMask |= (1 << 11);

 }

    	public IEnumerator Muerte()
	{

	velocidadenemigo.velocity = Vector2.zero;
		anim.SetBool("Muerte", true);
	
	
		yield return new WaitForSeconds(0.08f);
		Destroy(gameObject);
	}

}