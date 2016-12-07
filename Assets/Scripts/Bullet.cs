using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour
{
    //Handled Server side, damage
    void OnCollisionEnter(Collision other)
    {
        if(other.transform.tag == "Player" && isServer)
        {
            other.gameObject.GetComponent<ControlledUnit>().CurrentHealth -= 50;
            Destroy(gameObject);
        }
    }
}
