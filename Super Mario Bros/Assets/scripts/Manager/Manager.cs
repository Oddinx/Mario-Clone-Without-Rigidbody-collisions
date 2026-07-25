using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    
    
    public static Manager _manager;

    public Text textovidas;
    public Text textomonedas;
    public Text textotiempo;

    public Text textopuntaje;

    private static int cantidadvidas = 5;
    
     private static int cantidadmonedas = 0;

      private static float cantidadtiempo = 300;

      private static int puntaje = 0;


 
    private AudioSource musiclevel;

    Player player;
    
   public bool timeractivo = true;

     public event Action pausa;

    public event Action reanudar;

    public event Action suscribir;

    public event Action desuscribir;

    public event Action muertemario;
    // Start is called before the first frame update
    private void Awake() 
    {
        _manager = this;
          
          

    }
  
  void Start(){
      musiclevel = GetComponent<AudioSource>();
      musiclevel.Play();
      player = GetComponent<Player>();

      cantidadtiempo = 300;
      textopuntaje.text = puntaje.ToString("D6");
      textovidas.text = cantidadvidas.ToString();
      textomonedas.text = cantidadmonedas.ToString("D2");
      textotiempo.text = cantidadtiempo.ToString("f0");
  }

  void Update(){
      if(timeractivo){
         cantidadtiempo -= Time.deltaTime;
      }

      textotiempo.text = cantidadtiempo.ToString("f0");
      textovidas.text = cantidadvidas.ToString();
      textomonedas.text = cantidadmonedas.ToString("D2");
      textopuntaje.text = puntaje.ToString("D6");

      if(cantidadtiempo  < 0){
         timeractivo = false;
        Muerteportiempo();
        cantidadtiempo = 0;
        
      }
    }
   
   
    // Update is called once per frame

   public void Muerteportiempo(){

     if(muertemario !=null){

        muertemario();
     }
   }

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

   
   public void Actualizarvidas(){

     cantidadvidas +=1;

     textovidas.text = cantidadvidas.ToString();


   }

   public void Disminuirvidas(){
        cantidadvidas -=1;

     textovidas.text = cantidadvidas.ToString();

   }

      public void ActualizarMonedas(){

     if(cantidadmonedas == 99){

       cantidadmonedas = 0;
       Actualizarvidas();
     }else{

       cantidadmonedas +=1;
     }

       textomonedas.text = cantidadmonedas.ToString();
   }

   public void Actualizarpuntos(int puntos){
       puntaje += puntos;
       textopuntaje.text = puntaje.ToString("D6");
   }


      public void Reanudarenemigo(){



         // Pausaenemigo.detener = false;
         if(reanudar!= null){

             reanudar();
         } 
      }
     

}
