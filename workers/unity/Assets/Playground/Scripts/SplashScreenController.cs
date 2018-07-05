using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Playground
{
    public class SplashScreenController : MonoBehaviour
    {
        [SerializeField] private InputField textInput;
        [SerializeField] private Button connectButton;

        public void AttemptSpatialOsConnection()
        {
            SaveConnectionInfo();
            StartGameScene();
        }

        private void SaveConnectionInfo()
        {
            PlayerPrefs.SetString("deployment", textInput.text);
            PlayerPrefs.Save();
        }

        private void StartGameScene()
        {
            SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
        }
    }
}
