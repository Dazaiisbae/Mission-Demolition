using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum GameMode
{
    Idle,
    Playing,
    LevelEnd
}

public class MissionDemolition : MonoBehaviour
{
    static public MissionDemolition S; // Singleton
    public Text uiLevel; // UI Level Text
    public Text uiShots; // UI Shots Text
    public Vector3 castlePos; // Position of the castle
    public GameObject[] castles; // Array of castles

    public int level; // Current level
    public int levelMax; // Maximum level
    public int shotsTaken; // Number of shots taken
    public GameObject castle; // Current castle
    public GameMode mode = GameMode.Idle; // Current game mode
    public string showing = "Show Slingshot"; // Current view mode
    // Start is called before the first frame update
    void Start()
    {
        S = this; // Set the singleton
        level = 0; // Initialize level
        shotsTaken = 0; // Initialize shots taken
        levelMax = castles.Length; // Set the maximum level
        StartLevel(); // Start the first level
    }

    void StartLevel()
    {
        // If there is a castle, destroy it
        if (castle != null)
        {
            Destroy(castle);
        }
        // Destroy all projectiles
        Projectile.DESTROY_PROJECTILES(); // Assuming Projectile is a class that manages projectiles
        castle = Instantiate<GameObject>(castles[level]); // Instantiate the current castle
        castle.transform.position = castlePos; // Set the position of the castle
        Goal.goalMet = false; // Reset the goal
        UpdateGUI(); // Update the UI
        mode = GameMode.Playing; // Set the game mode to playing
    }

    void UpdateGUI()
    {
        // Update the UI text for level and shots taken
        uiLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        uiShots.text = "Shots Taken: " + shotsTaken;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGUI(); // Update the UI every frame
        // check for level end
        if (mode == GameMode.Playing && Goal.goalMet)
        {
            mode = GameMode.LevelEnd; // Set the game mode to level end
            Invoke("NextLevel", 2f); // Invoke the NextLevel method after 2 seconds
        }
    }

    void NextLevel()
    {
        level++; // Increment the level
        if (level == levelMax)
        {
            level = 0; // Reset to first level if max level is reached
        }
        StartLevel(); // Start the next level
    }

    static public void shotFired()
    {
        S.shotsTaken++; // Increment shots taken
    }

    static public GameObject GET_CASTLE()
    {
        return S.castle;
    }
}
