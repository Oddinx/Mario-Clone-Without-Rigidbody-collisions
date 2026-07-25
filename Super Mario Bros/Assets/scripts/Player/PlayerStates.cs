using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalTypes;
public class PlayerStates : Personaje
{




 public int estado; // 0 pequeño // 1 grande // fuego



 bool cambioEstado;

bool fuego;

bool grande;


public estadosmario _estadosmario;

bool animacion;



    // Start is called before the first frame update

  void Start() {
        

       Init();

    }


 


    // Update is called once per frame
    void Update()
    {
      
           
      if(cambioEstado) 
       switch (_estadosmario){
            

         case estadosmario.Normal:
    
   
         //animator.runtimeAnimatorController = Marios[0];

         Marionormal();

     

         //Activarsuscripcion();

         break;

         case estadosmario.Grande:
   
         
       
         //Crecer();

         StartCoroutine(Anima());

         break;
       

         case estadosmario.Fuego:


          Mariofuego();

         break;

       } 
      
      
   

    }


public void Actualizarestado(estadosmario nuevoestadomario){

_estadosmario = nuevoestadomario;

cambioEstado = true;

}

void Marionormal(){


StartCoroutine(Normal());
if(fuego){
  anim.SetLayerWeight (2, 0);
}

if(grande){
  anim.SetLayerWeight (1, 0);
  
}


}

IEnumerator Normal(){

//anim.SetTrigger("Daño");

Manager._manager.Pausarenemigo();
OnPauseGame();

yield return new WaitForSeconds(0.4f);

//anim.ResetTrigger("Daño");

   Manager._manager.Reanudarenemigo();

  OnResumeGame();

 

  yield return new WaitForSeconds(0.1f);

  fuego = false;

  grande = false;

  cambioEstado = false;

  Activarsuscripcion();

}

void Layers(){

  if(_estadosmario == estadosmario.Grande){
       anim.SetLayerWeight (1, 0);

  }
}

 void Mariofuego(){

   grande = false;

   fuego = true;

     StartCoroutine(Flor());
 }




IEnumerator Flor(){


 anim.SetBool ("Crecer",true);




Manager._manager.Pausarenemigo();

OnPauseGame();

yield return new WaitForSeconds(0.4f);
 anim.SetBool("Crecer",false);
  OnResumeGame();
         Manager._manager.Reanudarenemigo();

     anim.SetLayerWeight (2, 1);
 
  yield return new WaitForSeconds(0.1f);

  cambioEstado = false;

  Activarsuscripcion();

}


public void Activarsuscripcion(){

  Manager._manager.Suscripcion();



}


IEnumerator Anima(){
 anim.SetBool ("Crecer",true);

fuego = false;

grande = true;
Manager._manager.Pausarenemigo();

OnPauseGame();

yield return new WaitForSeconds(0.4f);
 anim.SetBool("Crecer",false);
 OnResumeGame();
         Manager._manager.Reanudarenemigo();

    
     anim.SetLayerWeight (1, 1);
  yield return new WaitForSeconds(0.1f);

  cambioEstado = false;

  Activarsuscripcion();

}



}
