using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.UI;
using TMPro;

public class OctoprintAPIController : MonoBehaviour
{
    private string baseAPICall = "http://octopi.local/api";
    private string APIKEY = "";

    //UI Texts
    public TMP_Text actualBedTemp, targetBedTemp;
    public TMP_Text actualHotEndTemp, targetHotEndTemp;
    public TMP_Text fileName, uploadDate, filamentUse, totalPrintTime, fileSize;




    public Button callAPIButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void CallAPI()
    {
        print("Return Of API Call");
        StartCoroutine(GetOctoPrintBedInfo());
        StartCoroutine(GetOctoPrintHotEndInfo());
        StartCoroutine(GetOctoPrintJobInfo());
    }

    public void ShutDownOctoPrint()
    {
        StartCoroutine(PostOctoPrintShutdown());
    }

    //Helper Functions
    private string ToFilamentLength(float filamentLength)
    {
        return ToTwoDecimalPlaces(filamentLength / 1000) + "m";
    }

    private string ToPrintTime(float printTime)
    {
        return ToTwoDecimalPlaces(printTime / 3600) + " hours";
    }

    private string ToMegaBytes(float bytes)
    {
        return ToTwoDecimalPlaces(bytes / 1000000) + "MB";
    }

    private float ToTwoDecimalPlaces(float num)
    {
        return num = Mathf.Round(num * 100.0f) * 0.01f;
    }

    private System.DateTime ToDate(long unixTimeStamp)
    {
        System.DateTime dtDateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddSeconds(unixTimeStamp);
        return dtDateTime;
    }


    IEnumerator GetOctoPrintBedInfo()
    { 

        UnityWebRequest printerBedInfo = UnityWebRequest.Get(baseAPICall + "/printer/bed");
        printerBedInfo.SetRequestHeader("Authorization", "Bearer " + APIKEY);

        yield return printerBedInfo.SendWebRequest();

        if(printerBedInfo.result ==  UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError(printerBedInfo.error);
            yield break;
        }

        JSONNode info = JSON.Parse(printerBedInfo.downloadHandler.text);

        Debug.Log(info);

        float bedActualTemp = float.Parse(info["bed"]["actual"]);
        float bedTargetTemp = float.Parse(info["bed"]["target"]);

        actualBedTemp.text = "Actual Bed Temp : " + bedActualTemp.ToString();
        targetBedTemp.text = "Target Bed Temp : " + bedTargetTemp.ToString();

        Debug.Log(bedActualTemp);
        Debug.Log(bedTargetTemp);
    }

    IEnumerator GetOctoPrintHotEndInfo()
    {

        UnityWebRequest printerHotEndInfo = UnityWebRequest.Get(baseAPICall + "/printer/tool");
        printerHotEndInfo.SetRequestHeader("Authorization", "Bearer " + APIKEY);

        yield return printerHotEndInfo.SendWebRequest();

        if (printerHotEndInfo.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError(printerHotEndInfo.error);
            yield break;
        }

        JSONNode info = JSON.Parse(printerHotEndInfo.downloadHandler.text);

        Debug.Log(info);

        float hotEndActualTemp = float.Parse(info["tool0"]["actual"]);
        float hotEndTargetTemp = float.Parse(info["tool0"]["target"]);

        actualHotEndTemp.text = "Actual Bed Temp : " + hotEndActualTemp.ToString();
        targetHotEndTemp.text = "Target Bed Temp : " + hotEndTargetTemp.ToString();

        Debug.Log(hotEndActualTemp);
        Debug.Log(hotEndTargetTemp);
    }

    IEnumerator GetOctoPrintJobInfo()
    {

        UnityWebRequest printerHotEndInfo = UnityWebRequest.Get(baseAPICall + "/job");
        printerHotEndInfo.SetRequestHeader("Authorization", "Bearer " + APIKEY);

        yield return printerHotEndInfo.SendWebRequest();

        if (printerHotEndInfo.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError(printerHotEndInfo.error);
            yield break;
        }

        JSONNode info = JSON.Parse(printerHotEndInfo.downloadHandler.text);

        Debug.Log(info);

        string jobName = info["job"]["file"]["name"];
        float filament = float.Parse(info["job"]["filament"]["tool0"]["length"]);
        long uploadTime = long.Parse(info["job"]["file"]["date"]);
        float printTime = float.Parse(info["job"]["estimatedPrintTime"]);
        float size = float.Parse(info["job"]["file"]["size"]);

        fileName.text = "File : " + jobName;
        filamentUse.text = "Filament : " + ToFilamentLength(filament);
        uploadDate.text = "Uploaded : " + ToDate(uploadTime);
        totalPrintTime.text = "Approx Total Print Time : " + ToPrintTime(printTime);
        fileSize.text = "File Size : " + ToMegaBytes(size);
    }

    IEnumerator PostOctoPrintShutdown()
    {

        UnityWebRequest printerBedInfo = UnityWebRequest.Post(baseAPICall + "/system/commands/core/shutdown","");
        printerBedInfo.SetRequestHeader("Authorization", "Bearer " + APIKEY);

        yield return printerBedInfo.SendWebRequest();

        if (printerBedInfo.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError(printerBedInfo.error);
            yield break;
        }

        print("Shutting Down OctoPrint");
    }

}

