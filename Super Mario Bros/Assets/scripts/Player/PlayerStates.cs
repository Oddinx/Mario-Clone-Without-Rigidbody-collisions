using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates : Personaje
{




 public int estado; // 0 pequeño // 1 grande // fuego

    private SpriteRenderer spriteRenderer;
    private Animator animator;

bool cambioEstado;

float transtimer;







    // Start is called before the first frame update

  void Start() {
        

      
 Init();

        


}



 


    // Update is called once per frame
    void Update()
    {


            //boxcast

          



//boxcast abajo


    

  

    
//finboxcastabajo


//boxcastderecha

    

//finboxcast derecha

//Boxcast izquierda

  

//Finboxcastizquierda

//fin boxcast





//fin rayo der

//RayoArriba


        
       switch (estado){
            

         case 0:
    
   
         //animator.runtimeAnimatorController = Marios[0];

         anim.SetLayerWeight (1, 0);

         break;

         case 1:
   
             //animator.runtimeAnimatorController = Marios[1];

         Crecer();

        

         break;
       

         case 2:



         break;

       } 


    }


     public void Actualizarestado(int nuevoEstado){

  cambioEstado = true;

  estado = nuevoEstado;




 }

 void Crecer(){

 anim.SetBool ("Crecer",true);
 transtimer += Time.fixedDeltaTime;


Manager._manager.Pausarenemigo();
    

     if(transtimer > 0.4){

         anim.SetBool("Crecer",false);

          Manager._manager.Reanudarenemigo();
     }

     anim.SetLayerWeight (1, 1);


 }


public void Activarsuscripcion(){

  Manager._manager.Suscripcion();


  transtimer = 0;
}
 




}
