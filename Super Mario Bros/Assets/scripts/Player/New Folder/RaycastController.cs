﻿using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider2D))]
public class RaycastController : MonoBehaviour {

	//public LayerMask collisionMask;




	
	public const float skinWidth = .015f;
	public int horizontalRayCount = 4;
	public int verticalRayCount = 4;
		 const float dstBetweenRays = .25f;

	[HideInInspector]
	public float horizontalRaySpacing;
	[HideInInspector]
	public float verticalRaySpacing;

	[HideInInspector]
	public BoxCollider2D collider;
	public RaycastOrigins raycastOrigins;

	public virtual void Start() {
		collider = GetComponent<BoxCollider2D> ();
		CalculateRaySpacing ();

		
	}

	public void UpdateRaycastOrigins() {
		Bounds bounds = collider.bounds;
		bounds.Expand (skinWidth * -2);
		
		raycastOrigins.bottomLeft = new Vector2 (bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight = new Vector2 (bounds.max.x, bounds.min.y);
		raycastOrigins.topLeft = new Vector2 (bounds.min.x, bounds.max.y);
		raycastOrigins.topRight = new Vector2 (bounds.max.x, bounds.max.y);
	}
	
	public void CalculateRaySpacing() {
		Bounds bounds = collider.bounds;
		bounds.Expand (skinWidth * -2);
		
		float boundsWidth = bounds.size.x;
        float boundsHeight = bounds.size.y;
       

	   horizontalRayCount = Mathf.RoundToInt(boundsHeight/dstBetweenRays);
	   verticalRayCount   = Mathf.RoundToInt(boundsWidth/dstBetweenRays);
		
		horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
		verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
	}
	
	public struct RaycastOrigins {
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}
}