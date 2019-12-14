using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBlock : MonoBehaviour
{
   public int rebote;

   Vector2 position;
 
    // Start is called before the first frame update
    void Start()
    {
      
        position = new Vector2(transform.position.x,transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {

        if(rebote == 1){

         transform.Translate (Vector3.up*8*Time.deltaTime);

       


        }

         if(rebote == -1 ){

         transform.Translate (Vector3.down*3*Time.deltaTime);


         
        
     

        }
        if(rebote == 0){
         
         transform.position = new Vector2(position.x,position.y);

        }
        
    }


void movimiento(){

   StartCoroutine(Rebote());

     


}

void Destroy(){


    Destroy(gameObject);
}

   public IEnumerator Rebote(){
   
   rebote =1;

   yield return new WaitForSeconds (0.1f);
  
  rebote =-1;

   yield return new WaitForSeconds (0.1f);

   rebote = 0;
   }





}
