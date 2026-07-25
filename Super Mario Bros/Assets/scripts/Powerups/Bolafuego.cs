using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolafuego : MonoBehaviour
{
    Player player;

    Rigidbody2D rb;

    float velocidad = 14f;
    Vector2 direccion;

     BoxCollider2D Pcollider;

    Collider2D circlecollider;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        rb = GetComponent<Rigidbody2D>();
        
       Pcollider = player.GetComponent<BoxCollider2D>();

       circlecollider = GetComponent<CircleCollider2D>();

              if(player.transform.localScale.x < 0){

              transform.localScale = new Vector3(-1f,1f,1f);
             velocidad = -velocidad;
             rb.AddForce(Vector2.left*velocidad);
         }
         
          if(direccion.x > 0.1f){
            
            transform.localScale = new Vector3(1f,1f,1f);
             rb.AddForce(Vector2.right*velocidad);
         }
     
    }

    // Update is called once per frame
    void Update()
    {
    
        rb.linearVelocity = new Vector2(velocidad,rb.linearVelocity.y);
            
         
       
    }

     void OnTriggerEnter2D(Collider2D col)
    {
      Colisionenemigo colisionenemigo = col.gameObject.GetComponent<Colisionenemigo>();
         if(col.tag == "Enemigo"){

             Destroy(this.gameObject);

             colisionenemigo.Muerte2();
         }

    }

   void OnCollisionEnter2D(Collision2D coll)
    {
        
        

            if(coll.gameObject.tag =="Player" || coll.gameObject.tag =="bolasdefuego"){

                  
      
          Physics2D.IgnoreCollision(coll.collider, circlecollider);
      
            }

      if(coll.gameObject.tag == "Ground" || coll.gameObject.tag == "Obstaculo" || coll.gameObject.tag == "Block"){

              foreach(ContactPoint2D hitPos in coll.contacts){


                  if(hitPos.normal.y == 0){

                      if(hitPos.normal.x > 0 || hitPos.normal.x < 0){

                          Destroy(this.gameObject);
                      }
                  }
              }
      }

    }

}
