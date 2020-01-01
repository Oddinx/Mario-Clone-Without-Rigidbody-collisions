using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pausaenemigo : Enemigo
{
   public int id;
    
    public Pausaenemigo ps;
    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<Pausaenemigo>();

        Init();
 
         Manager._manager.pausa+= OnPauseGame;

       Manager._manager.reanudar+=OnResumeGame;

       Manager._manager.suscribir += Suscribir;

       Manager._manager.desuscribir+= Desuscribir;

    }





    public void Suscribir(){

      
          Manager._manager.pausa+= OnPauseGame;

       Manager._manager.reanudar+=OnResumeGame;
   

     
        

      }
    // Update is called once per frame
 
    public void OnPauseGame(){

        

        if (ps != null) {   

       controller.enabled = false;
	
		anim.enabled = false;

        velocidadenemigo.enabled = false;
        
       Manager._manager.pausa-= OnPauseGame;

       }

         
        

    }

     public void Desuscribir(){

         if(ps == this.ps){
        Manager._manager.pausa-=OnPauseGame;

          Manager._manager.reanudar-=OnResumeGame;

          Manager._manager.suscribir -= Suscribir;

           Manager._manager.desuscribir-= Desuscribir;

         }

      }

    	public void OnResumeGame() {

         if (ps != null) {   
        
        velocidadenemigo.enabled = true;
	
		controller.enabled = true;

		anim.enabled = true;

         Manager._manager.reanudar-=OnResumeGame;
       
         }
        
       
	 }

}
