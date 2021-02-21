using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CamereMovement : MonoBehaviour {


    private float counter = 0F;
    private const float SQRT2 = 1.414f;
    
    

    public GameObject GameManager;
    private GameStateManager gameStateManager;
    public GameObject cam;

    float x, y, z;
    float xRot, yRot, zRot;
    private float cameraMovementRate = .002F;
    private float maxCamMovementRate = 3;
    private float rateofIncrease = .0006F;

    private bool finishedIncreasingSpeed = false;

    // Initialization
    void Start () {
        x = 245.5F;
        y = 1.55F;
        z = 247f;

        xRot = 4.5F;
        yRot = 0;
        zRot = 0;

        gameStateManager = GameManager.GetComponent<GameStateManager>();
	}
	
	// Update is called once per frame
    // Camera should oscillate side to side and pan right/left in an oscillating pattern
	void Update () {

        float time = Time.time;
        if (!finishedIncreasingSpeed)
        {
            if (gameStateManager.gameState == GameStateManager.GameState.BattleStart)
            {
                cameraMovementRate = 4;
            }
            else
            {
                if (cameraMovementRate > maxCamMovementRate)
                    cameraMovementRate -= rateofIncrease;
                else
                    finishedIncreasingSpeed = true;
            }
        }

        float displacement = -2.5F*Mathf.Cos(time/cameraMovementRate + 1.7F) + x;
        float rotation = 14.5F * Mathf.Cos(time/cameraMovementRate + 1.7F) + yRot;
        transform.position = new Vector3(displacement, y, z);
        transform.rotation = Quaternion.Euler(xRot, rotation, zRot);
        counter += cameraMovementRate;

    }
}
