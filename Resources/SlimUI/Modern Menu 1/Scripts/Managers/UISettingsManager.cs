using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

namespace SlimUI.ModernMenu
{
    public class UISettingsManager : MonoBehaviour
    {

        public enum Platform { Desktop, Mobile };
        public Platform platform;
        // toggle buttons
        [Header("MOBILE SETTINGS")]
        public GameObject mobileSFXtext;
        public GameObject mobileMusictext;
        public GameObject mobileShadowofftextLINE;
        public GameObject mobileShadowlowtextLINE;
        public GameObject mobileShadowhightextLINE;

        [Header("VIDEO SETTINGS")]
        public GameObject fullscreentext;
        public GameObject ambientocclusiontext;
        public GameObject vsynctext;
        public GameObject motionblurtext;
        public GameObject texturelowtextLINE;
        public GameObject texturemedtextLINE;
        public GameObject texturehightextLINE;
        //public GameObject cameraeffectstext;

        [Header("GAME SETTINGS")]
        public GameObject showhudtext;
        public GameObject tooltipstext;

        [Header("CONTROLS SETTINGS")]
        public GameObject invertmousetext;

        // sliders
        public GameObject musicSlider;
        public GameObject sensitivityXSlider;
        public GameObject sensitivityYSlider;
        public GameObject mouseSmoothSlider;

        //private float sliderValue = 0.0f;
        private float sliderValueXSensitivity = 0.0f;
        private float sliderValueYSensitivity = 0.0f;
        private float sliderValueSmoothing = 0.0f;


        public void Start()
        {
            // check slider values
            musicSlider.GetComponent<Slider>().value = AudioListener.volume;
            sensitivityXSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("XSensitivity");
            sensitivityYSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("YSensitivity");
            mouseSmoothSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("MouseSmoothing");

            // check full screen
            if (Screen.fullScreen == true)
            {
                fullscreentext.GetComponent<TMP_Text>().text = "on";
            }
            else if (Screen.fullScreen == false)
            {
                fullscreentext.GetComponent<TMP_Text>().text = "off";
            }
        }

        public void Update()
        {
            //sliderValue = musicSlider.GetComponent<Slider>().value;
            sliderValueXSensitivity = sensitivityXSlider.GetComponent<Slider>().value;
            sliderValueYSensitivity = sensitivityYSlider.GetComponent<Slider>().value;
            sliderValueSmoothing = mouseSmoothSlider.GetComponent<Slider>().value;
        }

        public void FullScreen()
        {

            if (Screen.fullScreen == true)
            {
                fullscreentext.GetComponent<TMP_Text>().text = "off";
                Screen.fullScreen = false;
            }
            else if (Screen.fullScreen == false)
            {
                fullscreentext.GetComponent<TMP_Text>().text = "on";
                Screen.fullScreen = true;
            }
        }

        public void MusicSlider()
        {
            //PlayerPrefs.SetFloat("MusicVolume", sliderValue);
            //PlayerPrefs.SetFloat("MusicVolume", musicSlider.GetComponent<Slider>().value);
            AudioListener.volume = musicSlider.GetComponent<Slider>().value;
        }

    }
}