using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalTypes;
public class Colisionenemigo : Enemigo
{

    
   

    //public tipoenemigos _tipoEnemigos;

    public tipoenemigos _tipoEnemigos;

    private Colisionenemigo colision;

    public float contador,contadorgiro, contm;

    public  bool  girando,daño,capes = false,contadormuerte;

    // Cooldown to prevent shell from being kicked immediately after a stomp
    private float stompCooldown = 0f;
    private const float STOMP_COOLDOWN_TIME = 0.25f;

    // SMB1 ShellChainCounter ($0125)
    [HideInInspector] public int shellChain = 0;
    private static readonly int[] shellPoints = { 100, 200, 400, 800, 1000, 2000, 4000, 5000, 8000 };


   

   public   LayerMask layer;

  

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

      // Tick down stomp cooldown
      if(stompCooldown > 0f) stompCooldown -= Time.deltaTime;

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

              // Koopa-specific player interaction
              if(hit.tag == "Player" && _tipoEnemigos.Equals(tipoenemigos.Koopa))  {
                  
                  if (player != null && player.isStarInvincible) {
                      Muerte2(100);
                      return;
                  }

                  // Player is stomping from above (use player's feet vs Koopa's top)
                  bool stomping = hit.bounds.min.y >= boxCollider.bounds.max.y - 0.15f;

                  if(stomping && !capes) {
                     // First stomp: enter shell (capes) state
                     capes = true;
                     girando = false;
                     anim.SetBool("girar", false);
                     velocidadenemigo.speed = 0;
                     // Start cooldown so the shell is NOT kicked on the same landing
                     stompCooldown = STOMP_COOLDOWN_TIME;
                     Manager._manager.Actualizarpuntos(100);

                  } else if(stomping && capes && velocidadenemigo.speed != 0) {
                     // Stomp on a MOVING shell: stop it
                     girando = false;
                     anim.SetBool("girar", false);
                     velocidadenemigo.speed = 0;
                     stompCooldown = STOMP_COOLDOWN_TIME;

                  } else if(stomping && capes && velocidadenemigo.speed == 0 && stompCooldown <= 0f) {
                     // Stomp on idle shell (after cooldown): kick it away from Mario
                     float marioX = hit.transform.position.x;
                     girando = true;
                     shellChain = 0; // Reset combo when kicked
                     if(marioX < transform.position.x) {
                         velocidadenemigo.speed = 16f;
                     } else {
                         velocidadenemigo.speed = -16f;
                     }
                     anim.SetBool("girar", true);
                     Manager._manager.Actualizarpuntos(400);

                  } else if(!stomping && capes && velocidadenemigo.speed == 0 && stompCooldown <= 0f) {
                     // Player walked into idle shell (after cooldown): kick it away from Mario
                     float marioX = hit.transform.position.x;
                     girando = true;
                     shellChain = 0; // Reset combo when kicked
                     if(marioX < transform.position.x) {
                         velocidadenemigo.speed = 16f;
                     } else {
                         velocidadenemigo.speed = -16f;
                     }
                     anim.SetBool("girar", true);
                     Manager._manager.Actualizarpuntos(400);
                  }
              }
              // Normal enemy bump / Shell kill check via overlap instead of solid collision
              else if (hit.tag == "Enemigo") {
                  Colisionenemigo otroEnemigo = hit.GetComponent<Colisionenemigo>();
                  if (otroEnemigo != null) {
                      if (girando) {
                          // Shell killing other enemies
                          if (!otroEnemigo.contadormuerte) {
                              int pts = shellChain < shellPoints.Length ? shellPoints[shellChain] : 10000;
                              shellChain++;
                              otroEnemigo.Muerte2(pts);
                          }
                      } else {
                          // Bump into each other and walk away
                          if (hit.bounds.center.x > boxCollider.bounds.center.x && velocidadenemigo.speed > 0) {
                              velocidadenemigo.speed = -Mathf.Abs(velocidadenemigo.speed);
                          } else if (hit.bounds.center.x <= boxCollider.bounds.center.x && velocidadenemigo.speed < 0) {
                              velocidadenemigo.speed = Mathf.Abs(velocidadenemigo.speed);
                          }
                      }
                  }
              }
        }
    }

 


       public void Destroy(int pts = 200){

    Manager._manager.Actualizarpuntos(pts);
   pausar.Desuscribir();

 if (_tipoEnemigos.Equals(tipoenemigos.Goomba)){

	StartCoroutine (Muerte ());
 }
      if(colision!=null && colision == this.colision){
         
        
	
    

     controller.horizontalcolision -=OnHorizontalCollisionEnter;

     controller.verticalcolision -=OnVerticalCollisionEnter;

   }
    

    }


    public override void Muerte2(int pts = 200){
     Manager._manager.Actualizarpuntos(pts);
   
    pausar.Desuscribir();

    StartCoroutine(Morir());
      if(colision!=null && colision == this.colision){

	
    

     controller.horizontalcolision -=OnHorizontalCollisionEnter;

     controller.verticalcolision -=OnVerticalCollisionEnter;

   }
    
    }

IEnumerator Morir(){

 transform.localScale = new Vector3(1,-1,1);

 velocidadenemigo.speed = 0;
  velocidadenemigo.velocity.y = 2f;

 controller.collisionMask = 0;

 boxCollider.enabled = false;

   yield return new WaitForSeconds(0.4f);
    velocidadenemigo.velocity = Vector2.zero;

    Destroy(gameObject);


}
    	void OnHorizontalCollisionEnter(Collider2D collider) {

       

      if(colision!=null && colision == this.colision)
           
           if (collider.tag == "Player") {
            
            if (player.isStarInvincible) {
                Muerte2(100);
                return; // Si Mario tiene estrella, muere el enemigo y no le hace daño a Mario
            }

            if(capes != true){


             if(playerStates._estadosmario == estadosmario.Normal){

              
            //collider.SendMessage ("Death", SendMessageOptions.DontRequireReceiver);

            player.Death();
            }

           if(playerStates._estadosmario != estadosmario.Normal){
             
          playerStates.Actualizarestado(estadosmario.Normal);

          //collider.SendMessage ("TakeDamage", SendMessageOptions.DontRequireReceiver);

          //player.TakeDamage();

          player.Corutina();

          StartCoroutine(inmune());

         }
          
           } 
           
			
		}
 

		if (collider.tag == "Enemigo" && girando) {
			Colisionenemigo otroEnemigo = collider.GetComponent<Colisionenemigo>();
			if(otroEnemigo != null) {
				// SMB1 Shell combo points
				int pts = shellChain < shellPoints.Length ? shellPoints[shellChain] : 10000;
				shellChain++;
				otroEnemigo.Muerte2(pts);
			}
			return; // Continue moving, don't bounce
		}

		if (collider.tag == "Obstaculo" || collider.tag == "Ground" || collider.tag == "Enemigo") {
			if (collider.bounds.center.x > boxCollider.bounds.center.x) {
				velocidadenemigo.speed = -Mathf.Abs(velocidadenemigo.speed);
			} else {
				velocidadenemigo.speed = Mathf.Abs(velocidadenemigo.speed);
			}
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
	 boxCollider.enabled = false;
	
		yield return new WaitForSeconds(0.08f);
		Destroy(gameObject);
	}

}