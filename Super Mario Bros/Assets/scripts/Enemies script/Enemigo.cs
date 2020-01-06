using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    
    protected Controller2D controller;

     protected  PlayerStates playerStates;

    protected   	Animator anim;

 protected   BoxCollider2D boxCollider;



 protected GameObject Player;

   protected   Player player;

  protected Velocidadenemigo velocidadenemigo;

  protected Pausaenemigo pausar;

protected Enemigo enemigo;
          // Start is called before the first frame update
    protected void Init()
    {

        	controller = GetComponent<Controller2D> ();
        anim = GetComponent<Animator>();
		boxCollider = GetComponent<BoxCollider2D>();

        Player = GameObject.FindGameObjectWithTag("Player");

       player = Player.GetComponent<Player>();
        	 playerStates = Player.GetComponent<PlayerStates>();

        velocidadenemigo = GetComponent<Velocidadenemigo>();

        pausar = GetComponent<Pausaenemigo>();

     
    }



}
