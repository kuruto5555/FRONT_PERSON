using FrontPerson.common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FrontPerson.Data;

namespace FrontPerson.Manager
{
    public class ApplicationManager : MonoBehaviour
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
        /// </summary>
        public bool IsInput { get; private set; } = true;
        public void SetIsInput(bool value) { IsInput = value; }

        /// <summary>
        /// ゲーム開始フラグ
        /// </summary>
        public bool IsGamePlay { get; private set; } = false;
        public void SetIsGamePlay(bool value) { IsGamePlay = value; }


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void RuntimeInit()
        {
            var go = new GameObject("ApplicationManager", typeof(ApplicationManager));

            go.tag = FrontPerson.Constants.TagName.MANAGER;

            // オーディオマネージャーを追加
            {
                var am = go.AddComponent<AudioManager>();
                am.Init();

                go.AddComponent<SceneManager>();
                go.AddComponent<FadeManager>();
            }

            DontDestroyOnLoad(go);

            go.GetComponent<ApplicationManager>().Load();
        }

        /// <summary>
        /// アプリ終了時呼ばれる
        /// </summary>
        private void OnApplicationQuit()
        {
        }

        /// <summary>
        /// データをセーブする
        /// </summary>
        public void Save()
        {
            //save_data_.sound_data_ = SoundManagerSetting.Instance.GetSoundVolumeData();
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

                //SoundManagerSetting.Instance.ValueSetting(save_data_.sound_data_);
            }
            catch (System.Exception)
            {
                // 初プレイ時はロードするデータが無いため生成
                save_data_ = new SaveDatas();

                //save_data_.sound_data_ = new SoundVolumeData();
                //SoundManagerSetting.Instance.ValueSetting(save_data_.sound_data_);
            }
        }
    }
}
