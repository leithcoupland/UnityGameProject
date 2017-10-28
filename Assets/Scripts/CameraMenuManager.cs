using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraMenuManager : MonoBehaviour {

    public Transform currentMount;
    public float speedCam;
    public Slider[] volumeSliders;



    //Initialise all sliders
    void Start () {
        volumeSliders[0].value = AudioManager.instance.masterVolumePercent;
        volumeSliders[1].value = AudioManager.instance.musicVolumePercent;
        volumeSliders[2].value = AudioManager.instance.sfxVolumePercent;
    }
	
	//Transform position and rotation of camera to current menu at each update
	void Update () {
        transform.position = Vector3.Lerp(transform.position, currentMount.position, speedCam);
        transform.rotation = Quaternion.Slerp(transform.rotation, currentMount.rotation, speedCam);
    }

    //Change mount to mount at new menu
    public void setMount(Transform newMount)
    {
        currentMount = newMount;
    }

    //Load next screen where the game is played
    public void Play()
    {
        SceneManager.LoadScene("Main");
    }

    //Exit application
    public void Quit()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    //Change colume of all sounds according to slider
    public void SetMasterVolume(float value)
    {
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Master);
    }

    //Change colume of music according to slider
    public void SetMusicVolume(float value)
    {
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Music);
    }

    //Change colume of sound effects according to slider
    public void SetSfxVolume(float value)
    {
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Sfx);
    }

}
