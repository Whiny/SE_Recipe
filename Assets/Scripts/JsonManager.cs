using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class JsonManager : MonoBehaviour
{
    public InputField nameInput;
    public InputField ingredientInput;
    public InputField levelInput;

    public Text textWindow;

    int inputNum;

    JObject basicInfo = new JObject();
    JObject ingredientInfo = new JObject();
    JObject processInfo = new JObject();

    JObject recipeTitle = new JObject();
    JObject recipeData = new JObject();

    string basicPath;
    string ingredientPath;
    string processPath;

    public List<BasicData> Basiclist;
    public List<IngredientData> Ingedientlist;
    public List<ProcessData> Processlist;


    // Start is called before the first frame update
    void Start()
    {
        basicPath = @"Assets/Scripts/레시피+기본정보_20201121161454.json";
        ingredientPath = @"Assets/Scripts/레시피+재료정보_20201123194915.json";
        processPath = @"Assets/Scripts/레시피+과정정보_20201123194850.json";

        basicInfo = JObject.Parse( File.ReadAllText( basicPath ) );
        ingredientInfo = JObject.Parse( File.ReadAllText( ingredientPath ) );
        processInfo = JObject.Parse( File.ReadAllText( processPath ) );

        Basiclist = JsonConvert.DeserializeObject<List<BasicData>>( basicInfo[ "data" ].ToString() );
        Ingedientlist = JsonConvert.DeserializeObject<List<IngredientData>>( ingredientInfo[ "data" ].ToString() );
        Processlist = JsonConvert.DeserializeObject<List<ProcessData>>( processInfo[ "data" ].ToString() );
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void search()
    {
        if( nameInput.text != null )
        {
            foreach( var i in Basiclist )
            {
                if( i.RECIPE_NM_KO == nameInput.text )
                {
                    printWindow( i );
                    foreach(var j in Processlist)
                    {
                        if(j.RECIPE_ID == i.RECIPE_ID )
                        {
                            textWindow.text += j.COOKING_DC;
                            textWindow.text += "\n";
                        }
                    }
                    Debug.Log( i.RECIPE_NM_KO );
                    break;
                }
            }
        }

        /*if( ingredientInput  != null)
        {
            foreach( var i in Ingedientlist )
            {
                if( i.IRDNT_NM == ingredientInput.text )
                {
                    Debug.Log( i.IRDNT_NM );
                    break;
                }
            }
        }
        else
            Debug.Log( "재료 확인 불가" );

        if( levelInput != null)
        {
            foreach( var i in Basiclist )
            {
                if( i.LEVEL_NM == levelInput.text )
                {
                    Debug.Log( i.LEVEL_NM );
                    break;
                }
            }
        }
        else
            Debug.Log( "레벨 확인 불가" );*/
    }

    void printWindow( BasicData basicData)
    {
        textWindow.text += basicData.RECIPE_NM_KO;
        textWindow.text += "\n";
        textWindow.text += basicData.SUMRY;
        textWindow.text += "\n";
        textWindow.text += basicData.TY_NM;
        textWindow.text += "\n";
        textWindow.text += basicData.COOKING_TIME;
        textWindow.text += "\n";
        textWindow.text += basicData.CALORIE;
        textWindow.text += "\n";
        textWindow.text += basicData.LEVEL_NM;
        textWindow.text += "\n";
    }
}

public class BasicData
{
    public string DET_URL { get; set; }
    public string IRDNT_CODE { get; set; }
    public string IMG_URL { get; set; }
    public string NATION_CODE { get; set; }
    public string SUMRY { get; set; }
    public string CALORIE { get; set; }
    public string TY_CODE { get; set; }
    public string RECIPE_NM_KO { get; set; }
    public string RN { get; set; }
    public string QNT { get; set; }
    public string PC_NM { get; set; }
    public string TY_NM { get; set; }
    public string LEVEL_NM { get; set; }
    public string RECIPE_ID { get; set; }
    public string NATION_NM { get; set; }
    public string COOKING_TIME { get; set; }
}

public class IngredientData
{
    public string IRDNT_CPCTY { get; set; }
    public string IRDNT_NM { get; set; }
    public string IRDNT_SN { get; set; }
    public string RN { get; set; }
    public string IRDNT_TY_NM { get; set; }
    public string RECIPE_ID { get; set; }
    public string IRDNT_TY_CODE { get; set; }
}

public class ProcessData
{
    public string STRE_STEP_IMAGE_URL { get; set; }
    public string STEP_TIP { get; set; }
    public string RN { get; set; }
    public string RECIPE_ID { get; set; }
    public string COOKING_DC { get; set; }
    public string COOKING_NO { get; set; }
}