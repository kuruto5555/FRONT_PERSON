using System;
using UnityEngine;

namespace FrontPerson.common
{
    /// <summary>
    /// シングルトンベース
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance_;
        public static T Instance
        {
            get
            {
                if (instance_ == null)
                {
                    Type t = typeof(T);

                    instance_ = (T)FindObjectOfType(t);
                    if (instance_ == null)
                    {
                        Debug.LogError(t + "をアタッチしているGameObjectはありません");
                    }
                }

                return instance_;
            }
        }

        protected virtual void Awake()
        {
            if (Instance != this)
            {
                Destroy(this);

                Debug.LogError(typeof(T) + "は既に他のGameObjectにアタッチされているため、コンポーネントを破棄しました。アタッチされているGameObjectは" + instance_.gameObject.name + "です。");
                return;
            }
        }
    }
}
