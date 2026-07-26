using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{

    public float speed = 1f;


 protected BoxCollider2D boxCollider;
    

        protected Vector2 velocity,m;

        protected bool grounded;



 protected LayerMask  prevlayer;







     
   private bool destruido;

protected float gravity = -20;


protected Controller2D controller;

  protected PlayerStates playerStates;

  protected GameObject Player;
    // Start is called before the first frame update
  protected  void Init()
    {
            Player = GameObject.FindGameObjectWithTag("Player");
      playerStates = Player.GetComponent<PlayerStates>();


            boxCollider = GetComponent<BoxCollider2D>();
   

    controller = GetComponent<Controller2D>();

 prevlayer = LayerMask.GetMask("Ground","Obstaculo","Player","Premiobloque") ;
    }

    // Update is called once per frame
 
}
