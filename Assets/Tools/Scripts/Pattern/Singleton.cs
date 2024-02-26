using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace QuocAnh.pattern
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance; // instance
        private static object _lock = new object(); // create new object for locking task
        public static T Instance // instance
        {
            get
            {
                //locking task
                lock (_lock)
                {
                    //if instance does not exist
                    if (_instance == null)
                    {
                        //find it
                        _instance = FindObjectOfType<T>();
                    }

                    //if it still not exist
                    if (_instance == null)
                    {
                        //make a new instance
                        _instance = new GameObject(typeof(T).Name).AddComponent<T>();
                        //DontDestroyOnLoad(_instance);
                    }

                    //return found instance
                    return _instance;
                }

            }
        }

        public virtual void Awake()
        {
            //if instnace does exist but not same as this  ->  destroy it
            if (Instance != null && Instance != this) { Destroy(this.gameObject); }
            //if dev wanted this singleton do be dont destroy onload  -> set it
            DontDestroyOnLoad(this);
        }
    }
}

