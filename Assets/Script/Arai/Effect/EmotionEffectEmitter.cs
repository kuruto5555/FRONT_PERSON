using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.Constants;

public class EmotionEffectEmitter : MonoBehaviour
{
    [Header("感情エフェクトプレハブ")]
    [SerializeField] List<GameObject> EffectList = null;

    [Header("エフェクトを出す間隔")]
    [SerializeField, Range(0.1f, 1.0f)] float Rate_ = 1.0f;

    private float _nowRate = 0.0f;

    private bool _isActive = false;

    private int _effectIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        _nowRate = 0.0f;
        _isActive = false;
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.M))
        {
            OpentFire(EMOTION_INDEX.SAD, 0.3f);
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            CloseFire();
        }

#endif
            //起動してなかったらやらない
            if (!_isActive) return;

        if(Rate_ <= _nowRate)
        {
            Instantiate(EffectList[_effectIndex], transform);
            _nowRate = 0.0f;
            return;
        }

        _nowRate += Time.deltaTime;


    }

    /// <summary>
    /// 射出開始する瞬間呼んで
    /// </summary>
    /// <param name="index"></param>
    /// <param name="SetRate">発射レート設定</param>
    public void OpentFire(EMOTION_INDEX index, float SetRate)
    {
        _isActive = true;
        _effectIndex = (int)index;

        _nowRate = Rate_ = SetRate;
    }

    /// <summary>
    /// 止める瞬間呼んで
    /// </summary>
    public void CloseFire()
    {
        _isActive = false;
        _nowRate = 0;
    }
}
