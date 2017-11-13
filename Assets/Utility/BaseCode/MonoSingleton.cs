using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T m_Instance = null;

    public static T instance
    {
        get
        {
            if (m_Instance == null)
            {   
                m_Instance = GameObject.FindObjectOfType(typeof(T)) as T;
                if (m_Instance == null)
                {   
                    GameObject go  = new GameObject(typeof(T).ToString());
                    if (go.GetComponent<T>() == null)
                        m_Instance =  go.AddComponent<T>();
                    DontDestroyOnLoad( go );
                }   
            }
            return m_Instance;
        }
    }

    private void Awake()
    {
        if (m_Instance == null)
            m_Instance = this as T;
    }


    private void OnApplicationQuit()
    {
        m_Instance = null;
    }
}