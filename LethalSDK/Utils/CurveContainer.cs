using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LethalSDK.Utils
{
    [Serializable]
    public class CurveContainer
    {
        public AnimationCurve curve;
        public static string SerializeCurve(AnimationCurve curve)
        {
            CurveContainer container = new CurveContainer { curve = curve };
            return JsonUtility.ToJson(container);
        }

        public static AnimationCurve DeserializeCurve(string json)
        {
            CurveContainer container = JsonUtility.FromJson<CurveContainer>(json);
            return container.curve;
        }
    }
}
