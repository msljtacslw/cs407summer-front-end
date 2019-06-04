using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using UnityEngine;
using UnityEngine.Networking;

public class WebReq : MonoBehaviour
{
    string objectFolderPath;
    string tempZipFolderPath;
    string serverUrl;
    string bearerToken;

    // Start is called before the first frame update
    void Start()
    {
        string storageUrl = "https://cs407projectjialu.s3.us-east-2.amazonaws.com/workingspace.zip?X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Credential=AKIAYBUSCVXDJ6L4ILN6%2F20190531%2Fus-east-2%2Fs3%2Faws4_request&X-Amz-Date=20190531T184841Z&X-Amz-Expires=900&X-Amz-Signature=4eb786a1be05ca1c49de58b9fb1005442a72729565095bbc6347ea059d83368c&X-Amz-SignedHeaders=host";
        objectFolderPath = Application.dataPath + "/StreamingAssets/Games/";
        tempZipFolderPath = Application.dataPath + "/StreamingAssets/tempZips/";
        Debug.Log(objectFolderPath);
        StartCoroutine(Upload(storageUrl, "testgame1"));
    }

    IEnumerator Upload(string url, string fileName)
    {
        yield return UploadFile(url, fileName);
    }

    IEnumerator ResquestUpload(string objectName)
    {
        UnityWebRequest www = UnityWebRequest.Get(serverUrl + "/reqUpload");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
        }
    }

    IEnumerator UploadFile(string url, string fileName)
    {
        byte[] myData;
        string objectPath = objectFolderPath + fileName;
        string tempZipPath = tempZipFolderPath + fileName + ".zip";

        try
        {
            ZipFile.CreateFromDirectory(objectPath, tempZipPath);
            myData = File.ReadAllBytes(tempZipPath);
            File.Delete(tempZipPath);
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            yield break;
        }

        UnityWebRequest www = UnityWebRequest.Put(url, myData);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Upload complete!");
        }
    }

}
