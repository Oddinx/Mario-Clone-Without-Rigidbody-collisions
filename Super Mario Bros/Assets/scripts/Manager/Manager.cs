using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{

    public static Manager _manager;

    Pausaenemigo _penemigo;

    GameObject enemigo;
 
    private AudioSource musiclevel;

    // Start is called before the first frame update
    private void Awake() 
    {
        _manager = this;
          
          

    }
  
  void Start(){


         enemigo = GameObject.FindGameObjectWithTag("Enemigo");
      _penemigo = enemigo.GetComponent<Pausaenemigo>();

      musiclevel = GetComponent<AudioSource>();

      musiclevel.Play();
  }
    
   
   
    // Update is called once per frame

    public event Action pausa;

    public event Action reanudar;

    public event Action suscribir;

    public event Action desuscribir;

    public void Suscripcion(){

      if( suscribir!= null){

          suscribir();
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
