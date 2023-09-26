using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public string sceneToLoad; // The name of the scene you want to switch to

    // This method is called when a trigger collision occurs
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collision is with a player or another object with a specific tag
        if (other.CompareTag("Player"))
        {
            // Load the new scene by its name
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}