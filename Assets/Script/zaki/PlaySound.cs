using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using FrontPerson.Manager;



namespace FrontPerson.Audio
{
    public class PlaySound : MonoBehaviour
    {
        //オーディオソース
        private AudioSource audiosorce = null;

        //オーディオクリップ
        [Header("SE音源")]
        [SerializeField] List<AudioClip> audioclip = new List<AudioClip>();

        // Start is called before the first frame update
        public void SoundPlay(string directory_path)
        {
            ////オーディオファイルへのパスを抽出
            //string directory_name = Path.GetFileName(directory_path); 
            //var audio_path_dict = new Dictionary<string, string>();

            audiosorce = GetComponent<AudioSource>();

            Debug.Log("AAA" + audioclip[0].ToString());

            foreach (var audio_clip in audioclip)
            {
                if(audio_clip.ToString()==directory_path)
                {
                    audiosorce.PlayOneShot(audio_clip, 1.0f * AudioManager.Instance.audio_volume_.SEVolume);

                }

                ////アセットへのパスを取得
                //var asset_path = AssetDatabase.GetAssetPath(audio_clip);
                ////オーディオ名の重複チェック
                //var audio_name = audio_clip.name;
                //if (audio_path_dict.ContainsKey(audio_name))
                //{
                //    Debug.LogError(audio_name + " is duplicate!\n1 : " + directory_name + "/" + audio_name + "\n2 : " + audio_path_dict[audio_name]);
                //}
                //audio_path_dict[audio_name] = directory_name + "/" + audio_name;
            }

            //再生
        }
    }
}