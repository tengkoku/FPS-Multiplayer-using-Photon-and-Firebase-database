using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Firebase;
using Firebase.Database;

public class FirebaseInitializer : MonoBehaviour
{
    public static FirebaseApp App { get; private set; } // Declare the app variable

    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp
                App = FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app
            }
            else
            {
                Debug.LogError(string.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
