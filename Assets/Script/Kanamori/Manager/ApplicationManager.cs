﻿using FrontPerson.common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FrontPerson.Data;
using FrontPerson.Character;

namespace FrontPerson.Manager
{
    public class ApplicationManager : SingletonMonoBehaviour<ApplicationManager>
    {
        /// <summary>
        /// 今回のスコア
        /// </summary>
        public int Score = 0;

        /// <summary>
        /// 今回のコンボ数
        /// </summary>
        public int ComboNum = 0;

        /// <summary>
        /// 今回のクリアしたミッション数
        /// </summary>
        public int ClearMissionNum = 0;

        // <summary>
        /// セーブデータ
        /// </summary>
        public SaveDatas save_data_= null;

        /// <summary>
        /// 入力を受け付けるかどうか
        /// true -> 受け付ける
        /// false -> 受け付けない
        /// </summary>
        public bool IsInput { get; private set; } = true;
        /// <summary>
        /// 入力を受け付けるかどうかのセット
        /// true -> 受け付ける
        /// false -> 受け付けない
        /// </summary>
        public void SetIsInput(bool value) { IsInput = value; }

        /// <summary>
        /// ゲーム開始フラグ
        /// true -> ゲームプレイ中
        /// false -> ゲームプレイ外
        /// </summary>
        public bool IsGamePlay { get; private set; } = false;
        /// <summary>
        /// ゲーム開始フラグのセット
        /// true -> ゲームプレイ中
        /// false -> ゲームプレイ外
        /// </summary>
        public void SetIsGamePlay(bool value) { IsGamePlay = value; }


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void RuntimeInit()
        {
            var go = new GameObject("ApplicationManager", typeof(ApplicationManager));
            var appm = go.GetComponent<ApplicationManager>(); 

            go.tag = FrontPerson.Constants.TagName.MANAGER;

            // オーディオマネージャーを追加
            {
                var am = go.AddComponent<AudioManager>();
                am.Init();

                go.AddComponent<SceneManager>();
                
                var fm = go.AddComponent<FadeManager>();
                fm.SetAppManager(appm);
            }

            DontDestroyOnLoad(go);

            appm.Load();
        }

        /// <summary>
        /// アプリ終了時呼ばれる
        /// </summary>
        private void OnApplicationQuit()
        {
            Save();
        }

        /// <summary>
        /// データをセーブする
        /// </summary>
        public void Save()
        {
            save_data_.SoundData = GetComponent<AudioManager>().audio_volume_;
            save_data_.VerticalRotationSpeed = Player.VerticalRotetaSpeed;
            save_data_.HorizontalRotationSpeed = Player.HorizontalRotetaSpeed;

            DataManager.Save(save_data_, SaveDatas.SAVE_DATA_NAME);
        }

        /// <summary>
        /// データをロードする
        /// </summary>
        private void Load()
        {
            try
            {
                // ２回目以降のプレイ
                save_data_ = DataManager.Load<SaveDatas>(SaveDatas.SAVE_DATA_NAME);

                GetComponent<AudioManager>().audio_volume_ = save_data_.SoundData;

                Player.HorizontalRotetaSpeed = save_data_.HorizontalRotationSpeed;
                Player.VerticalRotetaSpeed = save_data_.VerticalRotationSpeed;
            }
            catch (System.Exception)
            {
                // 初プレイ時はロードするデータが無いため生成
                save_data_ = new SaveDatas();

                GetComponent<AudioManager>().audio_volume_ = new AudioVolume();
            }
        }
    }
}
