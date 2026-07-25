using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConstants 
{
     public enum MoveDirection {
		Forward  =  1,
		Backward = -1
	}

	public enum EnemyState {
		Move,
		Dead
	}

	public enum PlayerState {
		Short,
		GrownUp,
		Dead
	}

	public enum GameWorldSoundEffects {
		Coin,
		Death,
		BreakBlock
	}


	public static readonly float maxVelocityX           = 10f;
	public static readonly float maxVelocityY           = 3f;
	public static readonly float timeToReloadAfterDeath = 3.5f;
	public static readonly float playerSkin             = .015f;
	public static readonly float collisionThresholdX    = 0.05f;
	public static readonly float collisionThresholdY    = 0.01f;
	public static readonly float stopingSpeed           = 0.1f;

    	public static readonly Vector2 gravity = new Vector2(0f, -7f);


    // Update is called once per frame
}
