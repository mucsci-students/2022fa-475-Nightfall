using UnityEngine;

public class DamageTestScript : MonoBehaviour
{
    
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.P)) { PlayerHandler.AddValue("health", -25); }

    }

}
