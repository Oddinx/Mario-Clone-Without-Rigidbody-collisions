using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalTypes;
public class Lanzarfuego : Personaje
{

    bool lanzar = true;

    public float cantidad = 0;

    public GameObject bolafuego;



    public Transform posicionlanzamiento;

   public float timer = 0;

   bool activarTemporizador;
    // Start is called before the first frame update
    void Start()
    {

        Init();
        
    }

    // Update is called once per frame
    void Update()
    {
        Preparacionlanzamiento();

        Temporizador();
    }


    void Preparacionlanzamiento(){

        if(playerStates._estadosmario == estadosmario.Fuego){
        

    
           if(Input.GetKeyDown (KeyCode.A)){
               
               cantidad++;
              
               
            if(cantidad < 3){
                 Lanzar();
                 anim.SetBool("fuego",true);
            }

            
        }

        if(Input.GetKeyUp (KeyCode.A)){
        
           anim.SetBool("fuego",false);

        }
      
             if(cantidad == 2){

                activarTemporizador = true;
            }else if(cantidad == 0){

                activarTemporizador = false;
            }

        }
    }

 void Temporizador(){
      if(activarTemporizador){

              timer += Time.deltaTime;

               if(timer >0.47f){

                   cantidad = 0;
                   timer = 0;
                   
               }
          }

 }
    void Lanzar(){
    Instantiate (bolafuego, posicionlanzamiento.position, posicionlanzamiento.rotation);

    }
}
