﻿using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

[Serializable]
public class RecipeInfo
{
    public string RECIPE_ID;
    public string RECIPE_NM_KO;
    public string SUMRY;
    public string TY_NM;
    public string LEVEL_NM;
    public string IMG_URL;

    public RecipeInfo(string RECIPE_ID, string RECIPE_NM_KO, string SUMRY, string TY_NM, string LEVEL_NM, string IMG_URL)
    {
        this.RECIPE_ID = RECIPE_ID;
        this.RECIPE_NM_KO = RECIPE_NM_KO;
        this.SUMRY = SUMRY;
        this.TY_NM = TY_NM;
        this.LEVEL_NM = LEVEL_NM;
        this.IMG_URL = IMG_URL;
    }
}

[Serializable]
public class IngredientInfo
{
    public string RECIPE_ID;
    public string IRDNT_NM;
    public string IRDNT_CPCTY;
}

[Serializable]
public class CookingProcessInfo
{
    public string RECIPE_ID;
    public string COOKING_NO;
    public string COOKING_DC;
    public string STRE_STEP_IMAGE_URL;
}

[Serializable]
public class RecipeDetailedInfo
{
    public RecipeInfo RECIPE;
    public IngredientInfo[] INGREDIENT;
    public CookingProcessInfo[] PROCESS;
}

public class Recipe : MonoBehaviour
{
    public RawImage _recipeImage; // 섬네일 UI
    public TMP_Text _descUI; // 간단한 설명 UI
    public TMP_Text _recipeNameUI; // 레시피 이름
    protected RecipeInfo _info;
    protected RecipeDetailedInfoLoader _loader; // 디테일 창
    protected HistoryManager _historyManager;
    private bool _isActive = false;

    public string _loadRecipeInfoUrl = "hook.iptime.org:1080/loadRecipeInfo.php";
    public string _loadRecipeDetailInfoUrl = "hook.iptime.org:1080/loadRecipeDetailInfo.php";

    // 초기화
    public void Init(string recipeID)
    {
        StartCoroutine(LoadRecipeInfo(recipeID));
    }

    public void Link(RecipeDetailedInfoLoader loader, HistoryManager historyManager)
    {
        _historyManager = historyManager;
        _loader = loader;
    }

    /// URL 이미지 로드
    protected IEnumerator LoadImageTexture(string url)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);

        www.timeout = 10; // 타임아웃 10초
        yield return www.SendWebRequest();

        if(www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            _recipeImage.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }
    }

    // 레시피의 상세한 설명
    public void LoadRecipeDetailInfo()
    {
        if(_isActive && _loader != null)
        {
            StartCoroutine(LoadRecipeDetailInfo(_info.RECIPE_ID));
        }
    }

    virtual protected IEnumerator LoadRecipeDetailInfo(string recipeID)
    {
        WWWForm form = new WWWForm();
        form.AddField("recipeID", recipeID);

        UnityWebRequest www = UnityWebRequest.Post(_loadRecipeDetailInfoUrl, form);

        www.timeout = 10; // 타임아웃 10초
        yield return www.SendWebRequest();

        if(www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            string jsonStr = www.downloadHandler.text;

            if(jsonStr != "결과가 없습니다.")
            {
                RecipeDetailedInfo detailedInfo = JsonUtility.FromJson<RecipeDetailedInfo>(jsonStr);
                _loader.gameObject.SetActive(true);
                _loader.LoadRecipe(detailedInfo);
                _historyManager.SaveRecipe(recipeID);
            }
        }
    }

    /// 레시피 정보 불러오기
    protected IEnumerator LoadRecipeInfo(string recipeID)
    {
        WWWForm form = new WWWForm();
        form.AddField("recipeID", recipeID);

        UnityWebRequest www = UnityWebRequest.Post(_loadRecipeInfoUrl, form);

        www.timeout = 10; // 타임아웃 10초
        yield return www.SendWebRequest();

        if(www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            string jsonStr = www.downloadHandler.text;
            Debug.Log(jsonStr);

            if(jsonStr != "결과가 없습니다.")
            {
                this._info = JsonUtility.FromJson<RecipeInfo>(jsonStr);
            }
        }

        // 섬네일 로드
        yield return LoadImageTexture(this._info.IMG_URL);

        // 레시피 이름 로드
        _recipeNameUI.text = this._info.RECIPE_NM_KO;
        // 간단한 설명 로드
        _descUI.text = this._info.SUMRY;

        // 레시피 활성화
        _isActive = true;
    }
}
