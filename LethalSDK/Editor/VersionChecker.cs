using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace LethalSDK.Editor
{
    public class VersionChecker : UnityEditor.Editor
    {
        [InitializeOnLoadMethod]
        public static void CheckVersion()
        {
            const string url = "https://raw.githubusercontent.com/HolographicWings/LethalSDK-Unity-Project/main/last.txt";
            UnityWebRequest www = UnityWebRequest.Get(url);
            var operation = www.SendWebRequest();

            EditorApplication.CallbackFunction callback = null;
            callback = () =>
            {
                if (operation.isDone)
                {
                    EditorApplication.update -= callback;
                    OnRequestComplete(www);
                }
            };

            EditorApplication.update += callback;
        }
        private static void OnRequestComplete(UnityWebRequest www)
        {
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error when getting last version number: " + www.error);
                return;
            }

            CompareVersions(www.downloadHandler.text);
        }
        private static void CompareVersions(string onlineVersion)
        {
            /*if (Version.Parse(PlayerSettings.bundleVersion) < Version.Parse(onlineVersion))
            {
                int option = EditorUtility.DisplayDialogComplex("Warning",
                    "The SDK is not up to date: " + onlineVersion,
                    "Update",
                    "Ignore",
                    "");

                if (option == 0) // Update
                {
                    Application.OpenURL("https://thunderstore.io/c/lethal-company/p/HolographicWings/LethalSDK/");
                }
            }*/
        }
    }
}