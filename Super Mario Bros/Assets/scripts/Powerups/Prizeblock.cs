using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalTypes;
public class Prizeblock : MonoBehaviour
{

//[SerializeField] private LayerMask  groundLayerMask;

    public GameObject Mario;


 GameObject  Premio;



    public  GameObject[] Powerups = new GameObject[5];
    

     Estrella estrella;
     Hongo hongo;

     Moneda moneda;

    public int numeroAleatorio;

    public int tipopremio;

    public bool gastada;


     


      public int rebote;

      Animator animator;



     public BoxCollider2D boxCollider,mcollider;



       public bool xcollision = false;

       public tipopowerups _tipopowerups;



public tipobloque _tipobloque;

   Vector2 position;
      private ParticleSystem particula;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent <Animator>();


        position = new Vector2(transform.position.x,transform.position.y);

        boxCollider = GetComponent<BoxCollider2D>();

        Mario = GameObject.FindGameObjectWithTag("Player");

        mcollider = Mario.GetComponent<BoxCollider2D>();

        gastada =false;
 particula = Resources.Load<ParticleSystem>("Particulas/Explosionbloque");
        
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

     
//Action();
  

//fin boxcast
//cast





//cast

     
Colision();


  if(rebote == -1 && _tipobloque == tipobloque.Premio){
          switch(_tipopowerups){
          
          case tipopowerups.Moneda:
          
           moneda.Monedasalto();

          break;
          
          case tipopowerups.Hongo:
          
          if(hongo !=null){
          hongo.Accion();
          }
          
          break;

          case tipopowerups.Flor:

          hongo.Accionflor();

          break;

          case tipopowerups.Estrella:
          
          if(estrella != null){
              estrella.Accion();
          }
          break;

         }


  }
       


    


    }


public void Rebotar(){

 if(!gastada){
  StartCoroutine (Rebote());

   GenerarPremio();
 }


}
void Colision(){

  Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0);

    foreach (Collider2D hit in hits)
        {
               if(hit == boxCollider)
              continue;


              if(hit.tag == "Player" && !gastada){
                   
                    if(boxCollider.bounds.min.x > hit.transform.position.x || boxCollider.bounds.max.x < hit.transform.position.x){


                         xcollision = true;
                            }else{

                       xcollision = false;
                             }

                if(hit.transform.position.y < boxCollider.bounds.min.y && !xcollision){

              
                  StartCoroutine (Rebote());

                  if(_tipobloque == tipobloque.Premio)
                     GenerarPremio();
                }
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

       if(bounds1.min.y > mcollider.transform.position.y && !gastada){
            
             
               Debug.Log(bounds1.size);
     StartCoroutine (Rebote());
      GenerarPremio();

            
         

       }

          if(bounds1.max.y <= mcollider.transform.position.y){
            
        

       }
       }

  } else{

      xcollision = false;
  }
}





public void Destroy(){

  if(_tipobloque == tipobloque.Normal){

   Instantiate(particula,transform.position,transform.rotation);

    Destroy(gameObject);

  }
}
  void GenerarPremio(){
    
     //numeroAleatorio = Random.Range(0,4);

   
  
   



     
    gastada = true;

    animator.SetBool("gastada",gastada); 

    //tipopremio == 0
   if (_tipopowerups == tipopowerups.Moneda){

Premio = (GameObject)Instantiate (Powerups[0],new Vector3(transform.position.x,transform.position.y + 0.7f,0),Quaternion.identity);
moneda= Premio.GetComponent<Moneda>();


      }


      if (_tipopowerups == tipopowerups.Hongo){

Premio = (GameObject)Instantiate (Powerups[1],new Vector3(transform.position.x,transform.position.y + 0.7f,0),Quaternion.identity);
hongo = Premio.GetComponent<Hongo>();




      }

        if (_tipopowerups == tipopowerups.Flor){

Premio = (GameObject)Instantiate (Powerups[2],new Vector3(transform.position.x,transform.position.y+ 0.7f ,0),Quaternion.identity);
hongo = Premio.GetComponent<Hongo>();




      }

      if (_tipopowerups == tipopowerups.Estrella) {
          // Asumiendo que Powerups[3] será la estrella
          Premio = (GameObject)Instantiate(Powerups[3], new Vector3(transform.position.x, transform.position.y + 0.7f, 0), Quaternion.identity);
          estrella = Premio.GetComponent<Estrella>();
      }

    //premioscodigo = Premio.GetComponent<Premios>();

    //premioscodigo.premio = numeroAleatorio;

   
    }


   public IEnumerator Rebote(){

     
   
   rebote =1;

 
   transform.Translate (Vector2.up*15f*Time.deltaTime);


   yield return new WaitForSeconds (0.09f);
   
  rebote =-1;
 transform.Translate (Vector2.down*15f*Time.deltaTime);

   yield return new WaitForSeconds (0.01f);

   rebote = 0;

   transform.position = position;
   }
}
