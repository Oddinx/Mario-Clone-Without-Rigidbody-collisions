using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{




 public int estado; // 0 pequeño // 1 grande // fuego

    private SpriteRenderer spriteRenderer;
    private Animator animator;

bool cambioEstado;

float transtimer;


BoxCollider2D boxCollider;

Player player;





    // Start is called before the first frame update

  void Start() {
        

        boxCollider = GetComponent<BoxCollider2D>();

        player = GetComponent <Player>();


        


    }

       void Awake () 
    {
        spriteRenderer = GetComponent<SpriteRenderer> ();    
        animator = GetComponent<Animator> ();




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

         animator.SetLayerWeight (1, 0);

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

 animator.SetBool ("Crecer",true);
 transtimer += Time.fixedDeltaTime;



    

     if(transtimer > 0.4){

         animator.SetBool("Crecer",false);
     }

     animator.SetLayerWeight (1, 1);


 }



 




}
