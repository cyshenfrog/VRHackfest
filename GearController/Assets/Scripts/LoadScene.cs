using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public string SceneName;
    public GameObject Fade;

    // Use this for initialization
    private void Start()
    {
        Invoke("LoadTargetScene", 5);
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void LoadTargetScene()
    {
        Fade.SetActive(true);
        BgmManager.instance.VolumeFadeout(1, BgmManager.Channel.bgmSource);
        Invoke("Load", 1.5f);
    }

    private void Load()
    {
        SceneManager.LoadScene(SceneName);
    }
}