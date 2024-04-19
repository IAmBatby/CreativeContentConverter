using System.Reflection;
using System;
using UnityEditor;
using UnityEngine.Events;

public class TestClass
{
    /// <returns>A log of all transferred calls, empty if there were none.</returns>
    public static
    string TransferPersistentCalls(
        UnityEngine.Object item, in string fromName, in string toName,
        in bool removeOldCalls = false, in bool dryRun = true)
    {
        var serializedItem = new SerializedObject(item);
        const string CallsPropertyPathFormat = "{0}.m_PersistentCalls.m_Calls";
        var srcCalls = serializedItem.FindProperty(string.Format(CallsPropertyPathFormat, GetValidFieldName(fromName.Trim())));
        var dstCalls = serializedItem.FindProperty(string.Format(CallsPropertyPathFormat, GetValidFieldName(toName.Trim())));
        var dstOffset = dstCalls.arraySize;

        var log = string.Empty;
        for (var srcIndex = 0; srcIndex < srcCalls.arraySize; srcIndex++)
        {
            #region Log Source Properties
            var srcCall = srcCalls.GetArrayElementAtIndex(srcIndex);

            var srcTarget = GetCallTarget(srcCall);
            var srcMethodName = GetCallMethodName(srcCall);
            var srcMode = GetCallMode(srcCall);
            var srcCallState = GetCallState(srcCall);

            var srcArgs = GetCallArgs(srcCall);
            var srcObjectArg = GetCallObjectArg(srcArgs);
            var srcObjectArgType = GetCallObjectArgType(srcArgs);
            var srcIntArg = GetCallIntArg(srcArgs);
            var srcFloatArg = GetCallFloatArg(srcArgs);
            var srcStringArg = GetCallStringArg(srcArgs);
            var srcBoolArg = GetCallBoolArg(srcArgs);

            log += $"At index {srcIndex}:\n";
            log += $"\t{srcTarget.displayName}: {srcTarget.propertyType} = {srcTarget.objectReferenceValue}\n";
            log += $"\t{srcMethodName.displayName}: {srcMethodName.propertyType} = {srcMethodName.stringValue}\n";
            log += $"\t{srcMode.displayName}: {srcMode.propertyType} = {srcMode.enumValueIndex}\n";
            log += $"\t{srcCallState.displayName}: {srcCallState.propertyType} = {srcCallState.enumValueIndex}\n";

            log += $"\t{srcArgs.displayName}: {srcArgs.propertyType} =\n";
            log += $"\t\t{srcObjectArg.displayName}: {srcObjectArg.propertyType} = {srcObjectArg.objectReferenceValue}\n";
            log += $"\t\t{srcObjectArgType.displayName}: {srcObjectArgType.propertyType} = {srcObjectArgType.stringValue}\n";
            log += $"\t\t{srcIntArg.displayName}: {srcIntArg.propertyType} = {srcIntArg.intValue}\n";
            log += $"\t\t{srcFloatArg.displayName}: {srcFloatArg.propertyType} = {srcFloatArg.floatValue}\n";
            log += $"\t\t{srcStringArg.displayName}: {srcStringArg.propertyType} = {srcStringArg.stringValue}\n";
            log += $"\t\t{srcBoolArg.displayName}: {srcBoolArg.propertyType} = {srcBoolArg.boolValue}\n\n";
            #endregion

            if (!dryRun)
            {
                SerializedProperty dstCall;

                SerializedProperty dstTarget;
                SerializedProperty dstMethodName;
                SerializedProperty dstMode;
                SerializedProperty dstCallState;

                SerializedProperty dstArgs;
                SerializedProperty dstObjectArg;
                SerializedProperty dstObjectArgType;
                SerializedProperty dstIntArg;
                SerializedProperty dstFloatArg;
                SerializedProperty dstStringArg;
                SerializedProperty dstBoolArg;

                if (dstOffset > 0)
                {
                    #region Check if the Call already Exists in the Destination
                    dstCall = dstCalls.GetArrayElementAtIndex(srcIndex);

                    // If we are satisfied that the call is exactly the same, skip ahead.
                    if (SerializedProperty.DataEquals(srcCall, dstCall))
                    {
                        log += $"(Already present in {toName}.)\n\n";
                        continue;
                    }
                    #endregion
                }

                // Only unique properties beyond this point. Append with care.

                #region Copy Properties from Source to Destination
                dstCalls.InsertArrayElementAtIndex(dstOffset + srcIndex);
                dstCall = dstCalls.GetArrayElementAtIndex(dstOffset + srcIndex);

                dstTarget = GetCallTarget(dstCall);
                dstMethodName = GetCallMethodName(dstCall);
                dstMode = GetCallMode(dstCall);
                dstCallState = GetCallState(dstCall);

                dstArgs = GetCallArgs(dstCall);
                dstObjectArg = GetCallObjectArg(dstArgs);
                dstObjectArgType = GetCallObjectArgType(dstArgs);
                dstIntArg = GetCallIntArg(dstArgs);
                dstFloatArg = GetCallFloatArg(dstArgs);
                dstStringArg = GetCallStringArg(dstArgs);
                dstBoolArg = GetCallBoolArg(dstArgs);

                dstTarget.objectReferenceValue = srcTarget.objectReferenceValue;
                dstMethodName.stringValue = srcMethodName.stringValue;
                dstMode.enumValueIndex = srcMode.enumValueIndex;
                dstCallState.enumValueIndex = srcCallState.enumValueIndex;

                dstObjectArg.objectReferenceValue = srcObjectArg.objectReferenceValue;
                dstObjectArgType.stringValue = srcObjectArgType.stringValue;
                dstIntArg.intValue = srcIntArg.intValue;
                dstFloatArg.floatValue = srcFloatArg.floatValue;
                dstStringArg.stringValue = srcStringArg.stringValue;
                dstBoolArg.boolValue = srcBoolArg.boolValue;
                #endregion
            }
        }

        #region Remove Old Calls from Source
        if (!dryRun && removeOldCalls)
        {
            srcCalls.ClearArray();
        }
        #endregion

        serializedItem.ApplyModifiedProperties();
        return log;

        #region Local Functions
        /// <summary></summary>
        /// <returns>The original name if it's a regular Unity Event.</returns>
        string GetValidFieldName(in string name)
        {
            var field = item.GetType().GetField(name, BindingFlags.NonPublic | BindingFlags.Instance);
            var value = field?.GetValue(item);
            if (value is UnityEventBase)
            {
                return name;
            }
            else
            {
                throw new FieldAccessException("Incorrect event name.");
            }
        }

        SerializedProperty GetCallTarget(in SerializedProperty callProperty) => callProperty?.FindPropertyRelative("m_Target");
        SerializedProperty GetCallMethodName(in SerializedProperty callProperty) => callProperty?.FindPropertyRelative("m_MethodName");
        SerializedProperty GetCallMode(in SerializedProperty callProperty) => callProperty?.FindPropertyRelative("m_Mode");
        SerializedProperty GetCallState(in SerializedProperty callProperty) => callProperty?.FindPropertyRelative("m_CallState");

        SerializedProperty GetCallArgs(in SerializedProperty callProperty) => callProperty?.FindPropertyRelative("m_Arguments");
        SerializedProperty GetCallObjectArg(in SerializedProperty argsProperty) => argsProperty?.FindPropertyRelative("m_ObjectArgument");
        SerializedProperty GetCallObjectArgType(in SerializedProperty argsProperty) => argsProperty?.FindPropertyRelative("m_ObjectArgumentAssemblyTypeName");
        SerializedProperty GetCallIntArg(in SerializedProperty argsProperty) => argsProperty?.FindPropertyRelative("m_IntArgument");
        SerializedProperty GetCallFloatArg(in SerializedProperty argsProperty) => argsProperty?.FindPropertyRelative("m_FloatArgument");
        SerializedProperty GetCallStringArg(in SerializedProperty argsProperty) => argsProperty?.FindPropertyRelative("m_StringArgument");
        SerializedProperty GetCallBoolArg(in SerializedProperty argsProperty) => argsProperty?.FindPropertyRelative("m_BoolArgument");
        #endregion
    }
}