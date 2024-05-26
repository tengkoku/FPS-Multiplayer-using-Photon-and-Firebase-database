using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;
    public GameObject pauseCanvas;
    public bool paused = false;

    public static GameManager instance;
    public static int blueScore = 0;
    public static int redScore = 0;

    private DatabaseReference databaseReference;

    void Awake()
    {
        FirebaseApp app = FirebaseApp.Create(new AppOptions
        {
            DatabaseUrl = new System.Uri("https://photon-f1812-default-rtdb.firebaseio.com/")
        });
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // Start is called before the first frame update
    void Start()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.GetReference("scores");
        PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 5, 0), Quaternion.identity);
        SetPaused();
    }

    public void Quit()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
            SetPaused();
        }
    }

    void SetPaused()
    {
        //set the canvas
        pauseCanvas.SetActive(paused);
        //set the cursoro lock
        Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
        //set the cursoro visible
        Cursor.visible = paused;
    }

    public void GetUserScore(string userId, Action<int> onScoreRetrieved)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("scores").Child(userId);
        reference.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to retrieve user score from Firebase: " + task.Exception);
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                int score = Convert.ToInt32(snapshot.Value);
                onScoreRetrieved(score);
            }
        });
    }

}