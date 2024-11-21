using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResolutionDropdown : MonoBehaviour
{
    public static ResolutionDropdown instance { get; private set; }
    public TMP_Dropdown resolutionDropdown;

    private List<Resolution> resolutions = new List<Resolution>();
    private List<Resolution> predefinedResolutions = new List<Resolution>
    {
        new Resolution { width = 3840, height = 2160 }, 
        new Resolution { width = 2560, height = 1440 }, 
        new Resolution { width = 2048, height = 1080 },
        new Resolution { width = 1920, height = 1080 },
        new Resolution { width = 1600, height = 900 },
        new Resolution { width = 1366, height = 768 },
        new Resolution { width = 1280, height = 720 },
        new Resolution { width = 1024, height = 768 },
        new Resolution { width = 800, height = 600 }
    };

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("ResolutionDropdown Awake");

        if (resolutionDropdown == null)
        {
            GameObject dropdownObject = GameObject.Find("ResolutionDropdown");
            if (dropdownObject != null)
            {
                resolutionDropdown = dropdownObject.GetComponent<TMP_Dropdown>();
            }
            else
            {
                resolutionDropdown = FindInactiveObjectByName("ResolutionDropdown")?.GetComponent<TMP_Dropdown>();
            }

            if (resolutionDropdown == null)
            {
                Debug.LogError("ResolutionDropdown is not assigned in the inspector and could not be found in the scene.");
            }
        }
    }

    void Start()
    {
        Debug.Log("ResolutionDropdown Start");

        if (resolutionDropdown == null)
        {
            Debug.LogError("ResolutionDropdown is not assigned in the inspector.");
            return;
        }

        resolutions.AddRange(predefinedResolutions);
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Count; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        if (PlayerPrefs.HasKey("ResolutionIndex"))
        {
            currentResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex");
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        resolutionDropdown.onValueChanged.AddListener(delegate { SetResolution(resolutionDropdown.value); });
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
        PlayerPrefs.Save();

        Debug.Log("Resolution set to: " + resolution.width + " x " + resolution.height);
    }

    private GameObject FindInactiveObjectByName(string name)
    {
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        foreach (Transform obj in objs)
        {
            if (obj.hideFlags == HideFlags.None && obj.name == name)
            {
                return obj.gameObject;
            }
        }
        return null;
    }
}