using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Premios : MonoBehaviour

{

    public int premio;

    public float velPremio;

    public Sprite[] sprites;

public Sprite[] monedaAnimación;

public Sprite[] destruirMoneda;

 public int vidaMoneda = 0;

 public int indicemoneda;

 SpriteRenderer sprite;

 Rigidbody2D rb;

 Animator animator;

 Collider2D colliderPremio;

 public int indicedestruir = 0;



    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent <Animator>();

        rb = GetComponent <Rigidbody2D>();

        sprite = GetComponent <SpriteRenderer>();

        colliderPremio = GetComponent <Collider2D>();

      switch(premio){
       
       case 0:

       gameObject.name = "Moneda";
       sprite.sprite = sprites [0];

      InvokeRepeating("AnimacionMoneda",0.1f,0.1f);

      Moneda();

       break;


       case 1:

       gameObject.name = "Seta Roja";

       sprite.sprite = sprites[1];

       PowerUps();

       break;


         case 2:

       gameObject.name = "Seta Verde";

       sprite.sprite = sprites[1];

       PowerUps();

       break;


         case 3:

       gameObject.name = "Flor";

       sprite.sprite = sprites[1];
        
        PowerUps();
       break;


      

      }


    }

    // Update is called once per frame
    void Update()
    {

        
    }

void PowerUps(){
 colliderPremio.isTrigger = true;

 rb.gravityScale = 0;

 rb.velocity = new Vector2(0,0.7f);



}

void OnTriggerExit2D(Collider2D other){


if(premio ==1 || premio == 2){

   rb.velocity = new Vector2 (2f,0);

}
rb.gravityScale =0.5f;

colliderPremio.isTrigger = false;

}

    void Moneda (){
     colliderPremio.isTrigger = true;

     rb.gravityScale = 0;

     rb.velocity = new Vector2(0,2.5f);

    }


    void AnimacionMoneda(){

   if( indicemoneda < monedaAnimación.Length-1 && vidaMoneda < 5){
    

    sprite.sprite = monedaAnimación [indicemoneda];

     indicemoneda++;

   }else if(indicemoneda == monedaAnimación.Length-1){

     indicemoneda = 0;

   }else{
     rb.isKinematic = true;

      if(vidaMoneda > 0 && indicedestruir < destruirMoneda.Length-1){

          sprite.sprite = destruirMoneda [indicedestruir];

          indicedestruir++;

          transform.localScale = new Vector3 (2f,2f,2f);
      }else{

          indicedestruir = 0;
      }

   }

   vidaMoneda++;

  if(vidaMoneda > 20){

      Destroy(gameObject);
  }


    }
}
