using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalTypes;

[RequireComponent(typeof(BoxCollider2D))]
public class Estrella : Powerup
{    
    public tipopowerups _tipopowerups = tipopowerups.Estrella;
    private Estrella estrella;
    public float bounceForce = 12f;

    void Start()
    {
        estrella = this;
        Init();
        controller.horizontalcolision += OnHorizontalCollisionEnter;
        controller.verticalcolision += OnVerticalCollisionEnter;
    }

    void Update()
    {
        controller.collisionMask &= ~(1 << 12);

        velocity.x = speed;
        velocity.y += gravity * Time.deltaTime;
        
        controller.Move(velocity * Time.deltaTime, m);  
        
        if (grounded) {
            velocity.y = bounceForce; // Estrella bounce
        }
        
        grounded = false;
    }

    public void Accion()
    {
        StartCoroutine(AC());
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
        if (estrella != null && estrella == this.estrella) {
            controller.horizontalcolision -= OnHorizontalCollisionEnter;
            controller.verticalcolision -= OnVerticalCollisionEnter;
        }
    }

    IEnumerator AC()
    {
        velocity.x = 0;
        speed = 0f;
        velocity.y = 1f;
        gravity = 0;
        
        controller.collisionMask = LayerMask.GetMask("Nothing");
        yield return new WaitForSeconds(0.3f);
        
        velocity.y += gravity * Time.deltaTime;
        gravity = -20;
        controller.collisionMask = prevlayer;
        
        speed = 4f; // bounce speed
        velocity.x = speed;
    }

    void OnVerticalCollisionEnter(Collider2D collider)
    {
        Player player = collider.gameObject.GetComponent<Player>();

        if (collider.tag == "Player") {
            if (player != null) {
                player.ActivarEstrella();
            }
            Manager._manager.Actualizarpuntos(1000);
            Destroy();
        }

        if (collider.tag == "Ground" || collider.tag == "Obstaculo") {
            grounded = true;
        }
    }

    void OnHorizontalCollisionEnter(Collider2D collider)
    {
        Player player = collider.gameObject.GetComponent<Player>();

        if (collider.tag == "Player") {
            if (player != null) {
                player.ActivarEstrella();
            }
            Manager._manager.Actualizarpuntos(1000);
            Destroy();
        }

        if (collider.tag == "Block") {
            Accion();
        }

        if (collider.tag != "Player") {
            speed *= -1f;
        }
    }
}
