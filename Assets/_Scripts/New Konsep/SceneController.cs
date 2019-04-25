using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
    public static SceneController TheInstanceOfSceneController;
    public static int MyScore;

    public int WhatSceneToLoad;

    public int JumlahSemuaPertanyaan = 3;
    public int SisaPertanyaan ;
    public int PertanyaanYgDitampilkan = 2;

    public SceneType[] JenisScene;

    private void Start()
    {
        if (TheInstanceOfSceneController == null)
        {
            TheInstanceOfSceneController = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    [ContextMenu("Load New Scene")]
    public void LoadNewScene()
    {
        if(JenisScene[WhatSceneToLoad].Name.Count > 0 && SisaPertanyaan > 0)
        {
            SisaPertanyaan--;

            int rand = Random.Range(0, JenisScene[WhatSceneToLoad].Name.Count);
            Debug.Log(JenisScene[WhatSceneToLoad].Name[rand]);
            SceneManager.LoadScene(JenisScene[WhatSceneToLoad].Name[rand]);
            JenisScene[WhatSceneToLoad].Name.RemoveAt(rand);
        }
        else if(JenisScene[WhatSceneToLoad].Name.Count <= 0 || SisaPertanyaan <= 0)
        {
            Debug.Log("Game Over");
            SceneManager.LoadScene("Score_Menu");
            RefreshListSceneName();
            MyScore = 0;
        }
    }

    [ContextMenu("Refresh Scene List")]
    public void RefreshListSceneName()
    {
        SisaPertanyaan = PertanyaanYgDitampilkan;
        JenisScene[WhatSceneToLoad].Name.Clear();
        for (int i = 0; i < JumlahSemuaPertanyaan; i++)
        {
            JenisScene[WhatSceneToLoad].Name.Add(JenisScene[WhatSceneToLoad].SceneNameType + "_" + i);
        }
    }
}
