using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace DemoObserver
{

    public class Observer : MonoBehaviour
    {
        #region Singleton
        static Observer s_instance;
        public static Observer Instance
        {
            get
            {
                // instance not exist, then create new one
                if (s_instance == null)
                {
                    // create new Gameobject, and add EventDispatcher component
                    GameObject singletonObject = new GameObject();
                    s_instance = singletonObject.AddComponent<Observer>();
                    singletonObject.name = "Singleton - EventDispatcher";
                }
                return s_instance;
            }
            private set { }
        }

        public static bool HasInstance()
        {
            return s_instance != null;
        }

        void Awake()
        {
            // check if there's another instance already exist in scene
            if (s_instance != null && s_instance.GetInstanceID() != this.GetInstanceID())
            {
                // Destroy this instances because already exist the singleton of EventsDispatcer
         
                Destroy(gameObject);
            }
            else
            {
                // set instance
                s_instance = this as Observer;
            }
        }


        void OnDestroy()
        {
            // reset this static var to null if it's the singleton instance
            if (s_instance == this)
            {
                ClearAllListener();
                s_instance = null;
            }
        }
        #endregion


        #region Fields
        /// Store all "listener"
        Dictionary<EventID, Action<object>> _listeners = new Dictionary<EventID, Action<object>>();
        #endregion


        #region Add Listeners, Post events, Remove listener

        public void RegisterListener(EventID eventID, Action<object> callback)
        {
            // check if listener exist in distionary
            if (_listeners.ContainsKey(eventID))
            {
                // add callback to our collection
                _listeners[eventID] += callback;
            }
            else
            {
                // add new key-value pair
                _listeners.Add(eventID, null);
                _listeners[eventID] += callback;
            }
        }
        public void PostEvent(EventID eventID, object param = null)
        {
            if (!_listeners.ContainsKey(eventID))
            {
                return;
            }

            // posting event
            var callbacks = _listeners[eventID];
            // if there's no listener remain, then do nothing
            if (callbacks != null)
            {
                callbacks(param);
            }
            else
            {
                _listeners.Remove(eventID);
            }
        }
        public void RemoveListener(EventID eventID, Action<object> callback)
        {


            if (_listeners.ContainsKey(eventID))
            {
                _listeners[eventID] -= callback;
            }

        }

        public void ClearAllListener()
        {
            _listeners.Clear();
        }
        #endregion
    }


    public static class EventObserver
    {
        /// Use for registering with EventsManager
        public static void RegisterListener(this MonoBehaviour listener, EventID eventID, Action<object> callback)
        {
            Observer.Instance.RegisterListener(eventID, callback);
        }

        /// Post event with param
        public static void PostEvent(this MonoBehaviour listener, EventID eventID, object param)
        {
            Observer.Instance.PostEvent(eventID, param);
        }

        /// Post event with no param (param = null)
        public static void PostEvent(this MonoBehaviour sender, EventID eventID)
        {
            Observer.Instance.PostEvent(eventID, null);
        }
    }

}