using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pbandera : MonoBehaviour
{
    // Start is called before the first frame update
   GameObject Player;

   GameObject Band;

[SerializeField] private LayerMask  groundLayerMask;
BoxCollider2D boxCollider;
   void Start(){
   
Band = GameObject.Find("Band");

boxCollider = GetComponent<BoxCollider2D>();

   }


 void Update() {
     //boxcast

            Bounds bounds = boxCollider.bounds;
       float extraHeightText = 0.3f;

     RaycastHit2D raycastHit= Physics2D.BoxCast(bounds.center,new Vector2(0.6f,bounds.size.y),0f,Vector2.up,extraHeightText,groundLayerMask);
      Color rayColor; 

      if(raycastHit.collider != null){

        rayColor = Color.green;

        if(raycastHit.collider.gameObject.tag == "Player"){

           Band.SendMessage("Bajarbandera");

        }
		
      }else{

        rayColor = Color.red;
      }
      Debug.DrawRay(bounds.center + new Vector3(bounds.extents.x,0),Vector2.up*(bounds.extents.y+extraHeightText),rayColor);
      Debug.DrawRay(bounds.center - new Vector3(bounds.extents.x,0),Vector2.up*(bounds.extents.y+extraHeightText),rayColor);
      Debug.DrawRay(bounds.center - new Vector3(bounds.extents.x,bounds.extents.y),Vector2.right*(bounds.extents.y),rayColor);
    

  

//boxcast abajo


    

  

    
//finboxcastabajo


//boxcastderecha

     RaycastHit2D raycastHit3= Physics2D.BoxCast(bounds.center,new Vector2(0.6f,bounds.size.y),0f,Vector2.right,extraHeightText,groundLayerMask);
     

      if(raycastHit3.collider != null){

        rayColor = Color.green;

          if(raycastHit3.collider.gameObject.tag == "Player"  ){

     
         Band.SendMessage("Bajarbandera");
        

   

        }

	
		
      }else{

        rayColor = Color.red;
      }
      Debug.DrawRay(bounds.center + new Vector3(bounds.extents.x,0),Vector2.down*(bounds.extents.y+extraHeightText),rayColor);
      Debug.DrawRay(bounds.center - new Vector3(bounds.extents.x,0),Vector2.down*(bounds.extents.y+extraHeightText),rayColor);
      Debug.DrawRay(bounds.center - new Vector3(bounds.extents.x,bounds.extents.y),Vector2.right*(bounds.extents.y),rayColor);


//finboxcast derecha

//Boxcast izquierda

   RaycastHit2D raycastHit4= Physics2D.BoxCast(bounds.center,new Vector2(0.6f,bounds.size.y),0f,Vector2.left,extraHeightText,groundLayerMask);
     

      if(raycastHit4.collider != null){

        rayColor = Color.green;

          if(raycastHit4.collider.gameObject.tag == "Player"){

                 
                 Band.SendMessage("Bajarbandera");

               
             StartCoroutine(desactivarlayer());

             


        }

	
		
      }else{

        rayColor = Color.red;
      }
      Debug.DrawRay(bounds.center + new Vector3(bounds.extents.x,0),Vector2.down*(bounds.extents.y+extraHeightText),rayColor);
      Debug.DrawRay(bounds.center - new Vector3(bounds.extents.x,0),Vector2.down*(bounds.extents.y+extraHeightText),rayColor);
      Debug.DrawRay(bounds.center - new Vector3(bounds.extents.x,bounds.extents.y),Vector2.right*(bounds.extents.y),rayColor);

//Finboxcastizquierda

//fin boxcast      



   }

   public IEnumerator desactivarlayer(){

     yield return new WaitForSeconds (3f);

     groundLayerMask = LayerMask.GetMask("Nothing");
  	
   }
   
 

}
