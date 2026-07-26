using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalTypes;

public class PiranhaPlant : Enemigo {
    
    public tipoenemigos _tipoEnemigos = tipoenemigos.PlantaPirana;
    
    [Header("Ajustes Piraña")]
    public float riseHeight = 1f;
    public float moveSpeed = 1.5f; 
    public float waitTime = 1.5f; 
    public float safeZoneDistance = 1.5f; 
    
    private Vector3 hiddenPosition;
    private Vector3 emergedPosition;
    
    private enum State { WaitingHidden, MovingUp, WaitingEmerged, MovingDown }
    private State currentState = State.WaitingHidden;
    private float timer = 0f;
    private bool isDead = false;

    void Start() {
        Init(); 
        
        hiddenPosition = transform.position;
        emergedPosition = hiddenPosition + new Vector3(0, riseHeight, 0);
        timer = waitTime;
    }
    
    void Update() {
        if (isDead) return;
        if (pausar != null && pausar.ps != null && !pausar.ps.enabled) return;
        
        CheckPlayerCollision();

        switch (currentState) {
            case State.WaitingHidden:
                timer -= Time.deltaTime;
                if (timer <= 0f) {
                    if (Player != null) {
                        float dist = Mathf.Abs(Player.transform.position.x - transform.position.x);
                        if (dist > safeZoneDistance) {
                            currentState = State.MovingUp;
                        } else {
                            // Mario muy cerca, esperar
                            timer = 0.2f; 
                        }
                    } else {
                        currentState = State.MovingUp;
                    }
                }
                break;
                
            case State.MovingUp:
                transform.position = Vector3.MoveTowards(transform.position, emergedPosition, moveSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, emergedPosition) < 0.01f) {
                    currentState = State.WaitingEmerged;
                    timer = waitTime;
                }
                break;
                
            case State.WaitingEmerged:
                timer -= Time.deltaTime;
                if (timer <= 0f) {
                    currentState = State.MovingDown;
                }
                break;
                
            case State.MovingDown:
                transform.position = Vector3.MoveTowards(transform.position, hiddenPosition, moveSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, hiddenPosition) < 0.01f) {
                    currentState = State.WaitingHidden;
                    timer = waitTime;
                }
                break;
        }
    }
    
    void CheckPlayerCollision() {
        if (isDead || boxCollider == null || !boxCollider.enabled) return;

        Collider2D[] hits = Physics2D.OverlapBoxAll(boxCollider.bounds.center, boxCollider.bounds.size, 0);
        foreach (Collider2D hit in hits) {
            if (hit.tag == "Player") {
                if (player != null && player.isStarInvincible) {
                    Muerte2(100);
                    return; // Mario is invincible, kill Piranha
                }

                if (playerStates != null) {
                    if (playerStates._estadosmario == estadosmario.Normal) {
                        player.Death();
                    } else {
                        playerStates.Actualizarestado(estadosmario.Normal);
                        player.Corutina();
                        StartCoroutine(inmuneLocal());
                    }
                }
                break;
            }
        }
    }

    public IEnumerator inmuneLocal() {
        if (boxCollider != null) {
            boxCollider.enabled = false;
            yield return new WaitForSeconds(1.2f);
            boxCollider.enabled = true;
        }
    }
    
    public override void Muerte2(int pts = 200) {
        if (isDead) return;
        isDead = true;
        
        Manager._manager.Actualizarpuntos(pts);
        if (pausar != null) pausar.Desuscribir();
        
        // La piraña simplemente desaparece al morir por bola de fuego
        Destroy(gameObject);
    }
}
