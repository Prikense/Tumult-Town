using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private string scene;

    public void LoadScene()
    {
        SceneManager.LoadScene(scene);
    }

}
