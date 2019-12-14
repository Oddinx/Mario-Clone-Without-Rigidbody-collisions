using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameraplayer : MonoBehaviour
{

  public GameObject follow;

  public Vector2 minCamPos,MaxCamPos;

  public float smoothTime;


  private Vector2 velocity;
   
    // Start is called before the first frame update
 
    // Update is called once per frame
    void FixedUpdate()
    {
      float posX = Mathf.SmoothDamp(transform.position.x,
      follow.transform.position.x,ref velocity.x, smoothTime);
    

    float posY = Mathf.SmoothDamp(transform.position.y,
      follow.transform.position.y,ref velocity.y, smoothTime);
 
     transform.position = new Vector3(Mathf.Clamp(posX,minCamPos.x,MaxCamPos.x),
     Mathf.Clamp(posY,minCamPos.y,MaxCamPos.y),transform.position.z);   

    }
}
