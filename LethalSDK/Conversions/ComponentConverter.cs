using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Linq;
using LethalSDK.Component;

namespace LethalSDK.Conversions
{
    public abstract class ComponentConverter<T, P>
    {
        public int componentCount = 0;
        public int successfulConversions = 0;
        public int failedConversions = 0;
        public string debugString = string.Empty;

        public List<T> components = new List<T>();
        public Dictionary<T, P> componentsStatusDictionary = new Dictionary<T, P>();
        public Dictionary<T, string> componentLogDictionary = new Dictionary<T, string>();

        public virtual void InitializeConverter()
        {
            componentCount = 0;
            successfulConversions = 0;
            failedConversions = 0;

            foreach (T component in GetComponents())
                if (!components.Contains(component))
                    components.Add(component);

            foreach (T component in components)
                componentLogDictionary.Add(component, string.Empty);
            componentCount = components.Count;

            debugString = "Converting " + components.Count + " " + typeof(T) + " To " + typeof(P) +  " In: " + SceneManager.GetActiveScene().name + "\n" + "\n";

            if (components.Count > 0)
                foreach (T component in components)
                {
                    if (ConvertComponent(component) == true)
                        successfulConversions++;
                    else
                        failedConversions++;
                }
            else
                debugString += "No Components Found In Scene!".Bold().Color("yellow") + "\n";

            FinishConversion();
        }

        public abstract bool ConvertComponent(T component);

        public void FinishConversion()
        {
            if (successfulConversions > 0)
                debugString += "Successfuly Converted " + successfulConversions + " " + typeof(T) + " To " + typeof(P) + "\n";
            if (failedConversions > 0)
                debugString += "Failed To Convert " + failedConversions + " " + typeof(T) + " To " + typeof(P) + "\n";

            string errorsString = string.Empty;

            foreach (KeyValuePair<T, string> pair in componentLogDictionary)
                if (!string.IsNullOrEmpty(pair.Value))
                    errorsString += pair.Value + "\n";

            if (!string.IsNullOrEmpty(errorsString))
            {
                debugString += "\n" + "Reported Errors" + "\n" + "---------------" + "\n" + "\n";
                debugString += errorsString;
            }



            if (failedConversions > 0)
                Debug.LogError(debugString);
            else if (successfulConversions == 0)
                Debug.LogWarning(debugString);
            //else
                //Debug.Log(debugString);
        }

        public virtual List<T> GetComponents()
        {
            List<T> list = new List<T>();

            foreach (GameObject rootObject in GetSceneRootGameObjects())
            {
                if (rootObject.GetComponent<T>() != null)
                    list.Add(rootObject.GetComponent<T>());
                foreach(T component in rootObject.GetComponentsInChildren<T>())
                    list.Add(component);
            }

            return (list);
        }

        public List<GameObject> GetSceneRootGameObjects()
        {
            return (SceneManager.GetActiveScene().GetRootGameObjects().ToList());
        }
    }
}
