using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Firebase.Database;

public class Health : MonoBehaviourPunCallbacks, IPunObservable
{
    private DatabaseReference databaseReference;

    Renderer[] visuals;
    public int health = 100;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //sync health
        if (stream.IsWriting)
        {
            stream.SendNext(health);
        }
        else
        {
            //we are reading
            health = (int)stream.ReceiveNext();
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        // Save the updated health score to Firebase
        databaseReference.SetValueAsync(health);
    }

    IEnumerator Respawn()
    {
        SetRenderers(false);
        health = 100;
        GetComponent<CharacterController>().enabled = false;
        transform.position = new Vector3(0, 10, 0);
        yield return new WaitForSeconds(1);
        GetComponent<CharacterController>().enabled = true;
        SetRenderers(true);
    }

    void SetRenderers(bool state)
    {
        foreach (var renderer in visuals)
        {
            renderer.enabled = state;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.GetReference("scores").Child(PhotonNetwork.LocalPlayer.UserId);
        visuals = GetComponentsInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            StartCoroutine(Respawn());
        }
    }
}