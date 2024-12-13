using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections.Generic;

public class DATASAVER : MonoBehaviour
{
    public static float MVolume;
    public static bool IFullscreen;
    public static int SResolution;
    public static float HP;
    public static bool LVL1C = false;
    public static float LVL1;
    public static bool LVL2C = false;
    public static float LVL2;
    public static bool LVL3C = false;
    public static float LVL3;
    public static bool LVL4C = false;
    public static float LVL4;
    public static bool LVL5C = false;
    public static float LVL5;
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    void Update()
    {
        Debug.Log(LVL1);
    }
}
