using UnityEngine;
using UnityEngine.EventSystems;
public class ContinuousLocomotion : MonoBehaviour
{
   public float moveSpeed = 3.0f;
   public Transform camera;
   bool stopMovement = false;

void Update()
{
    if (stopMovement == false)
    {
        Vector3 movement = camera.transform.forward;
        movement.y = 0; // Keep the movement in the horizontal plane
        //aply transform to the camera
        transform.Translate(movement * moveSpeed * Time.deltaTime);
    
    }
    // else{
    //     Vector3 movement = camera.transform.forward + camera.transform.right;
    //     movement.y = 0; // Keep the movement in the horizontal plane
    // }
}

public void OnPointerEnter()
    {
        stopMovement = true;
        //print("Pointer Enter" + stopMovement);
    }

public void OnPointerExit()
    {
        stopMovement = false;
    }

}
