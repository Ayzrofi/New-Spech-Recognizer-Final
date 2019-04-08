using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scoreHolder : MonoBehaviour {
    public static scoreHolder instance;
    public static int PlayerScore =0;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }else
        {
            Destroy(gameObject);
        }
    }
}
