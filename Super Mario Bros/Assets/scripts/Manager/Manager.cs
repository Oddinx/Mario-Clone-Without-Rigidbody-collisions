using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Manager : MonoBehaviour
{
    // Start is called before the first frame update

      public static Manager _manager;

    Pausaenemigo _penemigo;

    GameObject enemigo;
 
    private AudioSource musiclevel;

      public event Action pausa;

    public event Action reanudar;

    public event Action suscribir;

    public event Action desuscribir;

    public event Action Destruirpremio;
    void Awake(){

        _manager = this;
    }
       public void Suscripcion(){

      if( suscribir!= null){

          suscribir();
      }

    }
     

     public void Destruccionpremio(){

       if(Destruirpremio != null){

         Destruirpremio();
       }


     }
 
       public void Desuscripcion(){

      if( desuscribir!= null){

          desuscribir();
      }

    }
    
      public void Pausarenemigo(){


    

          if(pausa !=null){

               pausa();
      }

      }

   

      public void Reanudarenemigo(){



         // Pausaenemigo.detener = false;
         if(reanudar!= null){

             reanudar();
         } 
      }
}
