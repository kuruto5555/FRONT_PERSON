using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FrontPerson.Constants;
using FrontPerson.Manager;


namespace FrontPerson.Character.Skill
{
    public class SearchArea : MonoBehaviour
    {
        [Header("エリアの広がる速度")]
        [SerializeField, Range(0f, 100f)]
        float speed_ = 1f;

        [Header("エリアの広がる最大距離")]
        [SerializeField, Range(1f, 100f)]
        float areaSizeMAX_ = 50f;

        [Header("サーチ使用のインターバル")]
        [SerializeField, Range(1f, 30f)]
        float skillIntervalTime_ = 15f;

        [Header("サーチをやめたときの効果時間")]
        [SerializeField, Range(1f, 20f)]
        float skillEffectTime_ = 10f;

        [Header("足りないビタミンがCの時のマテリアル")]
        [SerializeField]
        Material vitaminCMat_ = null;

        [Header("足りないビタミンがDの時のマテリアル")]
        [SerializeField]
        Material vitaminDMat_ = null;

        /// <summary>
        /// Search中かどうか
        /// true -> サーチ中
        /// false -> サーチしていない
        /// </summary>
        public bool IsSearch { get; private set; } = false;

        /// <summary>
        /// 使えるかどうか
        /// true -> 使える
        /// false -> 使えない
        /// </summary>
        public bool IsUse { get; private set; } = true;

        /// <summary>
        /// サーチエリアのコライダー
        /// </summary>
        SphereCollider eria_ =null;

        /// <summary>
        /// サーチエリアのメッシュレンダラー
        /// </summary>
        MeshRenderer mesh_ = null;

        /// <summary>
        /// スキルのUI
        /// </summary>
        UI_Skill skillDraw_ = null;

        /// <summary>
        /// エリアに入ったやつのマテリアル情報を保存。
        /// Searchをやめたときに戻して、リセットする
        /// </summary>
        List<Material[]> initMaterialsList_;

        /// <summary>
        /// Areaに入ったやつのマテリアル
        /// </summary>
        List<Renderer> rendererList_;

        /// <summary>
        /// 敵ごとのスキル効果時間
        /// </summary>
        List<float> skillEffectTime_of_Enemys_;

        /// <summary>
        /// スキルのインターバルの計測用変数
        /// </summary>
        public float SkillIntervalTimeCount_ { get; private set; } = 0f;
        
        /// <summary>
        /// スキルのインターバル定数
        /// </summary>
        public float SkillIntervalTime { get { return skillIntervalTime_; } }


        // Start is called before the first frame update
        void Start()
        {
            IsSearch = false;
            IsUse = true;

            initMaterialsList_ = new List<Material[]>();
            rendererList_ = new List<Renderer>();
            skillEffectTime_of_Enemys_ = new List<float>();

            eria_ = GetComponent<SphereCollider>();
            eria_.enabled = false;

            mesh_ = GetComponent<MeshRenderer>();
            mesh_.enabled = false;

            skillDraw_ = FindObjectOfType<UI_Skill>();

        }

        // Update is called once per frame
        void Update()
        {
            // スキルのインターバル更新
            if(SkillIntervalTimeCount_ > 0)
            {
                SkillIntervalTimeCount_ -= Time.deltaTime;
                if (SkillIntervalTimeCount_ <= 0)
                    IsUse = true;
            }


            // リストのやつの状態を確認
            for (int i = 0; i < rendererList_.Count; i++)
            {
                // 元気だったら
                if (rendererList_[i].gameObject.GetComponent<Enemy>().isDown)
                {
                    // マテリアル戻してリストから削除
                    ReleaseMaterial(i);
                    continue;
                }

                // スキル効果時間が０より大きかったら更新する
                else if(skillEffectTime_of_Enemys_[i] > 0f)
                {
                    // 効果時間を減らす
                    skillEffectTime_of_Enemys_[i] -= Time.deltaTime;

                    // 効果時間が切れたら
                    if(skillEffectTime_of_Enemys_[i] <= 0f)
                    {
                        // マテリアル戻してリストから削除
                        ReleaseMaterial(i);
                        continue;
                    }
                }
            }


            // エリアが無効なら帰る
            if (!eria_.enabled) return;

            // エリアサイズが最大以上だったら帰る
            if (transform.localScale.x >= areaSizeMAX_) return;

            // エリアサイズ更新
            Vector3 scl = Vector3.zero;
            scl.x = scl.y = scl.z = speed_ * Time.deltaTime;
            transform.localScale += scl;

            // エリアサイズが最大値を超えたら最大値に矯正
            if(transform.localScale.x >= areaSizeMAX_)
            {
                scl.x = scl.y = scl.z = areaSizeMAX_;
                transform.localScale = scl;
            }

        }


        /// <summary>
        /// サーチ発動
        /// </summary>
        public void Search()
        {
            // スキルのインターバルが回復していなけらば帰る
            if (SkillIntervalTimeCount_ > 0 || !IsUse)
            {
                AudioManager.Instance.Play3DSE(transform.position, SEPath.GAME_SE_SCAN_ERROR);
                return;
            }


            // 発動
            IsUse = false;
            IsSearch = true;
            eria_.enabled = true;
            mesh_.enabled = true;
            
            AudioManager.Instance.Play3DSE(transform.position, SEPath.GAME_SE_SCAN);
        }


        /// <summary>
        /// サーチ停止
        /// </summary>
        public void Stop()
        {
            if (IsUse) return;
            if (!IsSearch) return;

            //コライダーを無効にする
            eria_.enabled = false;
            mesh_.enabled = false;
            transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
            
            // スキルのインターバル設定
            SkillIntervalTimeCount_ = skillIntervalTime_;

            // UIにサーチがクールタイムになったことを知らせる
            skillDraw_.StartCoolTime();

            // サーチ中フラグを無効化
            IsSearch = false;

            // スキル効果時間の設定
            for(int i=0;i< rendererList_.Count; i++)
            {
                // 既に効果時間が設定されているものはパス
                if (skillEffectTime_of_Enemys_[i] > 0f)
                    continue;

                // そうでなければスキル効果時間を設定
                skillEffectTime_of_Enemys_[i] = skillEffectTime_;
            }
        }


        /// <summary>
        /// サーチエリアに入ったとき
        /// </summary>
        /// <param name="other">エリアに入ったやつ</param>
        private void OnTriggerEnter(Collider other)
        {
            // エリアに入ったやつがEnemyじゃなかったら帰る
            if (other.tag != TagName.ENEMY) return;

            // EnemyComponentの取得
            Enemy enemy = other.GetComponent<Enemy>();
            // エネミーがすでに元気になっていたら帰る
            if (enemy.isDown) return;
            
            
            //同じオブジェクトがもしあった場合効果時間を更新して帰る
            for (int i = 0; i < rendererList_.Count;i++)
            {
                if (rendererList_[i].gameObject == other.gameObject)
                {
                    skillEffectTime_of_Enemys_[i] = skillEffectTime_;
                    return;
                }
            }

            // リストになかったので追加
            initMaterialsList_.Add(other.GetComponent<Renderer>().materials);
            rendererList_.Add(other.GetComponent<Renderer>());
            skillEffectTime_of_Enemys_.Add(0f);

            // エネミーの足りないビタミンの種類によってセットするMaterialを変える
            Renderer renderer = other.GetComponent<Renderer>();
            Material[] materials = renderer.materials;
            for (int i=0; i < other.GetComponent<Renderer>().materials.Length; i++)
            {                
                if(enemy.LackVitamins == NUTRIENTS_TYPE._A)
                {
                    materials[i] = vitaminCMat_;
                }
                else if(enemy.LackVitamins == NUTRIENTS_TYPE._B)
                {
                    materials[i] = vitaminDMat_;
                }
                
            }
            renderer.materials = materials;
        }



        /// <summary>
        /// サーチエリアから出てった時
        /// </summary>
        /// <param name="other">エリアから出たやつ</param>
        private void OnTriggerExit(Collider other)
        {
            for (int i = 0; i < rendererList_.Count; i++)
            {
                // 違ったらコンティニュー
                if (rendererList_[i].gameObject != other.gameObject)
                    continue;

                // 一緒だったらスキルの効果時間を設定
                skillEffectTime_of_Enemys_[i] = skillEffectTime_;
                break;
            }
        }


        void ReleaseMaterial(int index)
        {
            // マテリアル戻してリストから削除
            rendererList_[index].materials = initMaterialsList_[index];
            rendererList_.RemoveAt(index);
            initMaterialsList_.RemoveAt(index);
            skillEffectTime_of_Enemys_.RemoveAt(index);
        }


        private IEnumerator ReleaseMaterials(float skillEfectTime)
        {
            while (skillEfectTime <= 0)
            {
                yield return null;
                skillEfectTime -= Time.deltaTime;
            }

            //マテリアルを戻す
            if (initMaterialsList_.Count != 0)
            {
                for (int i = 0; i < initMaterialsList_.Count; i++)
                {
                    if (rendererList_[i].gameObject.GetComponent<Enemy>().isDown) continue;
                    rendererList_[i].materials = initMaterialsList_[i];
                }

                rendererList_.Clear();
                initMaterialsList_.Clear();
                skillEffectTime_of_Enemys_.Clear();
            }
        }
    }
}
