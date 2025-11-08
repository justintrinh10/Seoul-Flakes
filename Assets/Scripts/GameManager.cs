using UnityEngine;

public class GameManager : MonoBehaviour
{
    string[] bingsuBaseToppingType = { "redBean", "fruit", "mochi", "nuts" };
    Bingsu currentBingsu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    Bingsu CreateNewBingsu()
    {
        GameObject bingsuObject = new GameObject("Bingsu");
        Bingsu bingsu = bingsuObject.AddComponent<Bingsu>();
        return bingsu;
    }
}
