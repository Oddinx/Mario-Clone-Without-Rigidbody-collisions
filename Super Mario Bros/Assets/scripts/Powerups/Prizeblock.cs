using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prizeblock : MonoBehaviour
{

//[SerializeField] private LayerMask  groundLayerMask;

    public GameObject Mario;


 GameObject  Premio;

   
    public GameObject  PremioInstancia;
    public GameObject  PremioInstancia2;
    public GameObject  PremioInstancia3;
    public GameObject  PremioInstancia4;
    



    public Premios premioscodigo;

     Hongo hongo;

     Moneda moneda;

    public int numeroAleatorio;

    public int tipopremio;

    public bool gastada;

    public bool sueltaPremio;

     


      public int rebote;

      Animator animator;

      Rigidbody2D rb;

     public BoxCollider2D boxCollider,mcollider;

      
       public GameObject RayoArriba;

    private Vector2 IRayoArriba;

     private Vector2 FRayoArriba;

       public bool xcollision = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent <Animator>();

        rb = GetComponent <Rigidbody2D>();

        boxCollider = GetComponent<BoxCollider2D>();

        Mario = GameObject.FindGameObjectWithTag("Player");

        mcollider = Mario.GetComponent<BoxCollider2D>();

        gastada =false;

        
        
    }

    // Update is called once per frame
    void Update()
    {


// Bounds bounds = boxCollider.bounds;
  //     float extraHeightText = 0.5f;

//     RaycastHit2D raycastHit= Physics2D.BoxCast(bounds.center,new Vector2(0.75f,0.5f),0f,Vector2.down,extraHeightText,groundLayerMask);
//      Color rayColor; 

//      if(raycastHit.collider != null){

//        rayColor = Color.green;

        
//    if(raycastHit.collider.gameObject == Mario){

//      if(!gastada){
//    StartCoroutine (Rebote());
//     GenerarPremio();

     //       }  
//       }

//      Debug.DrawRay(bounds.center + new Vector3(bounds.extents.x,0),Vector2.down*(bounds.extents.y+extraHeightText),rayColor);
//      Debug.DrawRay(bounds.center - new Vector3(bounds.extents.x,0),Vector2.down*(bounds.extents.y+extraHeightText),rayColor);
//      Debug.DrawRay(bounds.center - new Vector3(bounds.extents.x,bounds.extents.y),Vector2.right*(bounds.extents.y),rayColor);

	
		
//      }else{

//        rayColor = Color.red;
//      }

//boxcasf

     
Action();
  

//fin boxcast
//cast





//cast
  

     




        if(rebote == 1){

         transform.Translate (Vector3.up*2*Time.deltaTime);

         sueltaPremio =true;


        }

         if(rebote == -1 ){

         transform.Translate (Vector3.down*2.92f*Time.deltaTime);


         
         switch(tipopremio){
          
          case 0:
          
           moneda.Monedasalto();

          break;
          
          case 1:
          
          if(hongo !=null){
          hongo.Accion();
          }
          
          break;

         }
     

        }

    }


  


void Action(){


           Bounds bounds1 = boxCollider.bounds;
  
          Vector2 boundse = new Vector2(0.69f,0.3f);
     bounds1.Expand(boundse);
  if(bounds1.Intersects(mcollider.bounds)){
     

       
 //Esta parte sirve para detectar la colision de los bounds de la parte horizontal del jugador, 
 //es necesaria esta parte para que no se genere el premio a menos que unicamente choque con la parte vertical
 //del bloque    
    if(bounds1.min.x > mcollider.transform.position.x || bounds1.max.x < mcollider.transform.position.x){


        xcollision = true;
    }else{

        xcollision = false;
    }
       
       if(xcollision == false){

       if(bounds1.min.y > mcollider.transform.position.y){
            
             if(!gastada){
     StartCoroutine (Rebote());
      GenerarPremio();

            }
         

       }

          if(bounds1.max.y <= mcollider.transform.position.y){
            
        

       }
       }

  } else{

      xcollision = false;
  }
}





  void GenerarPremio(){
    
     //numeroAleatorio = Random.Range(0,4);

   
   if(sueltaPremio){
   



     
    gastada = true;

    animator.SetBool("gastada",gastada); 
   if (tipopremio == 0){

Premio = (GameObject)Instantiate (PremioInstancia,new Vector3(transform.position.x,transform.position.y + 0.7f,0),Quaternion.identity);
moneda= Premio.GetComponent<Moneda>();


      }


      if (tipopremio == 1){

Premio = (GameObject)Instantiate (PremioInstancia2,new Vector3(transform.position.x,transform.position.y + 0.1f,0),Quaternion.identity);
hongo = Premio.GetComponent<Hongo>();




      }

    //premioscodigo = Premio.GetComponent<Premios>();

    //premioscodigo.premio = numeroAleatorio;

   }
    }

   public IEnumerator Rebote(){
   
   rebote =1;

   yield return new WaitForSeconds (0.01f);
  
  rebote =-1;

   yield return new WaitForSeconds (0.01f);

   rebote = 0;
   }
}
