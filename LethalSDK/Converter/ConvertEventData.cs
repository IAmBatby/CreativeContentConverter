using LethalSDK.Component;
using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;
using PlayerControllerB = GameNetcodeStuff.PlayerControllerB;

namespace LethalSDK.Converter
{
    public static class ConvertEventData
    {
        public static string ConvertInteractEventData(MonoBehaviour sourceComponent, MonoBehaviour targetComponent)
        {
            string debugString = "<b>" + "Converting InteractEvent Data From: " + sourceComponent.gameObject.name + "</b>" + "\n";
            try
            {
                var targetGameObject = targetComponent.gameObject;
                var lllTrigger = targetComponent;

                using var serializedSdkObj = new SerializedObject(sourceComponent);
                using var serializedLllObj = new SerializedObject(lllTrigger);

                serializedSdkObj.Update();
                serializedLllObj.Update();


                using var sdkIterator = serializedSdkObj.GetIterator();

                while (sdkIterator.NextVisible(true))
                {
                    if (sdkIterator.name == "m_Script")
                        continue;

                    using var targetProperty = serializedLllObj.FindProperty(sdkIterator.name);
                    if (targetProperty is null)
                        continue;

                    var sdkPropType = sdkIterator.GetPropertyType();
                    var lllPropType = targetProperty.GetPropertyType();

                    // This is probably what you're looking for Batby
                    if (sdkPropType == typeof(UnityEvent<object>) && lllPropType == typeof(InteractEvent))
                    {
                        // Get the source Event
                        var sdkEvent = sdkIterator.GetPropertyValue<UnityEvent<object>>();
                        var sdkEventListenerCount = sdkEvent.GetPersistentEventCount();

                        var persistentCalls = sdkIterator.FindPropertyRelative("m_PersistentCalls.m_Calls");

                        // Get the target Event
                        var lllEvent = targetProperty.GetPropertyValue<InteractEvent>();
                        var lllEventListenerCount = lllEvent.GetPersistentEventCount();

                        // Clear the target Event listeners (Optional, I only did this to clear it when testing.)
                        for (int i = 0; i < lllEventListenerCount; i++)
                            UnityEventTools.UnregisterPersistentListener(lllEvent, i);

                        for (int i = 0; i < sdkEventListenerCount; i++)
                        {
                            // Get the listener Object and the method name from the source Event.
                            var eventTarget = sdkEvent.GetPersistentTarget(i);
                            if (eventTarget == null) continue;
                            var methodName = sdkEvent.GetPersistentMethodName(i);
                            if (methodName == null) continue;
                            var listenerType = eventTarget.GetType();
                            if (listenerType == null) continue;

                            debugString += "\n" + "Converting Event Parameter #" + i + ": Target Name: " + eventTarget.name + ", MethodName: " + methodName + ", Listener Type: " + listenerType;

                            // Get the Type of the listener Object and then get the MethodInfo of the target method.
                            var methodInfo = listenerType.GetMethod(methodName, new[] { typeof(object) });

                            if (methodInfo is null) // True when the target method doesn't accept a PlayerControllerB or object parameter.
                            {
                                var mode = (PersistentListenerMode)persistentCalls.GetArrayElementAtIndex(i).FindPropertyRelative("m_Mode").enumValueIndex;
                                //debugString += ", Parameter Type: " + mode;

                                switch (mode)
                                {
                                    case PersistentListenerMode.EventDefined:
                                        // Should already be handled, I think...
                                        break;
                                    case PersistentListenerMode.Void:
                                        var method = UnityEventBase.GetValidMethodInfo(eventTarget, methodName, Type.EmptyTypes);
                                        var action = Delegate.CreateDelegate(typeof(UnityAction), eventTarget, method) as UnityAction;
                                        UnityEventTools.AddVoidPersistentListener(lllEvent, action);
                                        break;
                                    case PersistentListenerMode.Object:
                                        var objArgument = persistentCalls.GetArrayElementAtIndex(i).FindPropertyRelative("m_Arguments.m_ObjectArgument").objectReferenceValue;
                                        var objMethod = UnityEventBase.GetValidMethodInfo(eventTarget, methodName, new[] { typeof(Object) });
                                        var objAction = Delegate.CreateDelegate(typeof(UnityAction<Object>), eventTarget, objMethod) as UnityAction<Object>;
                                        UnityEventTools.AddObjectPersistentListener(lllEvent, objAction, objArgument);
                                        break;
                                    case PersistentListenerMode.Int:
                                        var intArgument = persistentCalls.GetArrayElementAtIndex(i).FindPropertyRelative("m_Arguments.m_IntArgument").intValue;
                                        var intMethod = UnityEventBase.GetValidMethodInfo(eventTarget, methodName, new[] { typeof(int) });
                                        var intAction = Delegate.CreateDelegate(typeof(UnityAction<int>), eventTarget, intMethod) as UnityAction<int>;
                                        UnityEventTools.AddIntPersistentListener(lllEvent, intAction, intArgument);
                                        break;
                                    case PersistentListenerMode.Float:
                                        var floatArgument = persistentCalls.GetArrayElementAtIndex(i).FindPropertyRelative("m_Arguments.m_FloatArgument").floatValue;
                                        var floatMethod = UnityEventBase.GetValidMethodInfo(eventTarget, methodName, new[] { typeof(float) });
                                        var floatAction = Delegate.CreateDelegate(typeof(UnityAction<float>), eventTarget, floatMethod) as UnityAction<float>;
                                        UnityEventTools.AddFloatPersistentListener(lllEvent, floatAction, floatArgument);
                                        break;
                                    case PersistentListenerMode.String:
                                        var stringArgument = persistentCalls.GetArrayElementAtIndex(i).FindPropertyRelative("m_Arguments.m_StringArgument").stringValue;
                                        var stringMethod = UnityEventBase.GetValidMethodInfo(eventTarget, methodName, new[] { typeof(string) });
                                        var stringAction = Delegate.CreateDelegate(typeof(UnityAction<string>), eventTarget, stringMethod) as UnityAction<string>;
                                        UnityEventTools.AddStringPersistentListener(lllEvent, stringAction, stringArgument);
                                        break;
                                    case PersistentListenerMode.Bool:
                                        var boolArgument = persistentCalls.GetArrayElementAtIndex(i).FindPropertyRelative("m_Arguments.m_BoolArgument").boolValue;
                                        var boolMethod = UnityEventBase.GetValidMethodInfo(eventTarget, methodName, new[] { typeof(bool) });
                                        var boolAction = Delegate.CreateDelegate(typeof(UnityAction<bool>), eventTarget, boolMethod) as UnityAction<bool>;
                                        UnityEventTools.AddBoolPersistentListener(lllEvent, boolAction, boolArgument);
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                            }
                            else
                            {
                                // Create a Persistent Delegate that takes in a PlayerControllerB instead of an object.
                                // Pass in the listener Object and the MethodInfo.
                                // There is an overload that allows you to pass the name of the target method instead of the MethodInfo but I had issues getting it to work.
                                var action = Delegate.CreateDelegate(typeof(UnityAction<PlayerControllerB>), eventTarget, methodInfo) as UnityAction<PlayerControllerB>;

                                // Finally add the type correct UnityAction as a persistent listener to the target Event.
                                UnityEventTools.AddPersistentListener(lllEvent, action);
                            }


                            EditorSceneManager.MarkSceneDirty(targetGameObject.scene);
                        }
                    }
                    else
                    {
                        serializedLllObj.CopyFromSerializedPropertyIfDifferent(sdkIterator);
                    }
                }
                debugString += "Converted Event Data Between: " + sourceComponent.gameObject.name + " And " + targetComponent.gameObject.name;
                return (debugString);
            }
            catch (Exception e)
            {
                debugString += "\n" + "\n" + "Error! Failed To Convert Event Data Between: " + sourceComponent.gameObject.name + " And " + targetComponent.gameObject.name + ". Stacktrace Below:" + "\n" + "\n" + e;
                return (debugString);
            }
           
        }

        /// <summary>
        /// Gets an existing component or Adds the component if it doesn't exist.
        /// </summary>
        /// <typeparam name="TComponent">Type of Component to get or add</typeparam>
        private static TComponent GetOrAddComponent<TComponent>(this GameObject target)
            where TComponent : UnityEngine.Component
        {
            var comp = target.GetComponent<TComponent>();
            return comp ? comp : target.AddComponent<TComponent>();
        }

        /// <summary>
        /// Gets the underlying Type of a SerializedProperty.
        /// </summary>
        /// <returns>Type of the Field referenced by the SerializedProperty</returns>
        private static Type GetPropertyType(this SerializedProperty property)
        {
            var type = property.serializedObject.targetObject.GetType();
            var fieldInfo = type.GetField(property.propertyPath);

            return fieldInfo.FieldType;
        }

        /// <summary>
        /// Gets the underlying value of a SerializedProperty and casts it to the supplied Type.
        /// </summary>
        /// <typeparam name="TValue">Field value Type</typeparam>
        /// <returns>Value of the Field referenced by the SerializedProperty cast to the supplied Type</returns>
        private static TValue GetPropertyValue<TValue>(this SerializedProperty property)
        {
            var target = property.serializedObject.targetObject;
            var type = target.GetType();
            var fieldInfo = type.GetField(property.propertyPath);

            var value = fieldInfo.GetValue(target);
            return (TValue)value;
        }

        public static void AddTeleportPlayerEvent(InteractTrigger interactTrigger, EntranceTeleport entranceTeleport)
        {
            var method = UnityEventBase.GetValidMethodInfo(entranceTeleport, "TeleportPlayer", Type.EmptyTypes);
            var action = Delegate.CreateDelegate(typeof(UnityAction), entranceTeleport, method) as UnityAction;
            interactTrigger.onInteract = new InteractEvent();
            UnityEventTools.AddVoidPersistentListener(interactTrigger.onInteract, action);
        }
    }
}

