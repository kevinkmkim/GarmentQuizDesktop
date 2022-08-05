using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
 
public class InstantiateButton : MonoBehaviour
{ 
    public VideoPlayer VideoPlayer;
    public string SceneName;

    void Start()
    {
        VideoPlayer.loopPointReached += LoadScene;
    }

    void LoadScene(VideoPlayer vp)
    {
        SceneManager.LoadScene( SceneName );
    }
    // void EndReached(UnityEngine.Video.VideoPlayer vp)
    // {
    //     UnityEngine.SceneManagement.SceneManager.LoadScene (“NewScene”);
    // }
}