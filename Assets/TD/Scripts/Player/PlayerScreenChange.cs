
using UnityEngine;



public class PlayerScreenChange : MonoBehaviour
{
    public GameObject Shop;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (PlayerMovement.moveOn)
            {
                PlayerMovement.moveOn = false;
                Cursor.lockState = CursorLockMode.None;
                Shop.SetActive(true);
            }
            else
            {
                PlayerMovement.moveOn = true;
                Cursor.lockState = CursorLockMode.Locked;
                Shop.SetActive(false);
            }

            
        }

        
    }

}
