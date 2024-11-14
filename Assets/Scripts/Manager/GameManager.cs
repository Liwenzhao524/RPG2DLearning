using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveManager
{
    public static GameManager instance;
    [SerializeField] CheckPoint[] checkPoints;
    [SerializeField] string _checkpointID;

    private void Awake ()
    {
        if (instance == null)
            instance = this;
        checkPoints = FindObjectsOfType<CheckPoint>();
    }

    public void RestartScene ()
    {
        SaveManager.instance.SaveGame();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void GamePause(bool pause)
    {
        if (pause)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    private void Update ()
    {
        //if(Input.GetKeyDown(KeyCode.M))
        //    RestartScene();
    }

    public void LoadGame (GameData gameData)
    {
        foreach (var pair in gameData.checkpoints)
        {
            foreach (var checkpoint in checkPoints)
            {
                if (checkpoint.id == pair.Key && pair.Value == true)
                    checkpoint.ActiveCheckPoint();
            }
        }

        _checkpointID = gameData.closestCheckpointID;
        PlacePlayerAtCheckpoint();
    }

    public void SaveGame (ref GameData gameData)
    {
        if(FindCloseCheckpoint() != null)
            gameData.closestCheckpointID = FindCloseCheckpoint().id;
        gameData.checkpoints.Clear();

        foreach (var checkpoint in checkPoints)
        {
            gameData.checkpoints.Add(checkpoint.id, checkpoint.isActive);
        }
    }

    private void PlacePlayerAtCheckpoint ()
    {
        foreach (var checkpoint in checkPoints)
        {
            if (checkpoint.id == _checkpointID)
                PlayerManager.instance.player.transform.position = checkpoint.transform.position;
        }
    }

    CheckPoint FindCloseCheckpoint ()
    {
        float minDis = Mathf.Infinity;
        CheckPoint target = null;
        foreach (var checkpoint in checkPoints)
        {
            float distance = Vector2.Distance(PlayerManager.instance.player.transform.position, checkpoint.transform.position);
            if (distance < minDis && checkpoint.isActive == true)
            {
                target = checkpoint;
                minDis = distance;
            }
        }
        return target;
    }

}
