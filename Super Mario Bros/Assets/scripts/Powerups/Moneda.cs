using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moneda : MonoBehaviour
{


      Collider2D colliderPremio;


 Animator anim;

 
    Vector2  velocity;

    float speed = 3f;

 
   
  
    void Start()
    {
         
           anim = GetComponent <Animator>();
           
         colliderPremio = GetComponent <Collider2D>();
        

     

       
    }


    // Update is called once per frame
 void Update() {
           transform.Translate(velocity * Time.deltaTime);
  }

 

    public  void Monedasalto (){


     StartCoroutine (Muerte ());

    

   

    }
  void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player"){

          Destroy(this.gameObject);
        }
    }
 
       	public IEnumerator Muerte()
	{

    velocity.y = speed;
  anim.SetTrigger ("Msalto");
Manager._manager.ActualizarMonedas();
  Manager._manager.Actualizarpuntos(100);
	
	
		yield return new WaitForSeconds(0.9f);
		Destroy(gameObject);
	}

     
}
