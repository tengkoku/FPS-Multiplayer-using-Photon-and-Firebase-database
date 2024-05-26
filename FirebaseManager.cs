using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Firebase;
using Firebase.Database;

public class FirebaseManager : MonoBehaviour
{
    private FirebaseManager firebaseManager;
    private DatabaseReference databaseReference;
    // Start is called before the first frame update
    void Awake()
    {
        firebaseManager = FindObjectOfType<FirebaseManager>();
        FirebaseApp app = FirebaseApp.Create(new AppOptions
        {
            DatabaseUrl = new System.Uri("https://photon-f1812-default-rtdb.firebaseio.com/")
        });
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void GetUserScore(string userId, System.Action<int> onScoreRetrieved)
    {
        DatabaseReference reference = databaseReference.Child("scores").Child(userId);
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
                int score = int.Parse(snapshot.Value.ToString());
                onScoreRetrieved(score);
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
