using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FrontPerson.Constants;
using FrontPerson.Manager;
using FrontPerson.UI;


namespace FrontPerson.Character.Skill
{
    public class SearchArea : MonoBehaviour
    {
        /// <summary>
        /// エリアに入った敵の情報
        /// </summary>
        class SearchInfo
        {
            /// <summary>
            /// サーチエリア内かどうか
            /// </summary>
            public bool isInSearchArea = false;

            /// <summary>
            /// 敵
            /// </summary>
            public Enemy enemy_ = null;

            /// <summary>
            /// サーチエリアに入った敵の初期のマテリアル保存用
            /// </summary>
            public Material[] initMaterials_ = null;

            /// <summary>
            /// Areaに入ったやつのマテリアル
            /// </summary>
            public Renderer renderer = null;

            /// <summary>
            /// 敵ごとのスキル効果時間
            /// </summary>
            public float skillEffectTime_ = 0f;
        }

        [Header("エリアが最大になる時間(s)")]
        [SerializeField, Range(0f, 5f)]
        float areaMaxTime_ = 1f;

        [Header("サーチをキャンセルできるまでの時間")]
        [SerializeField, Range(0f, 5f)]
        float cancelTime_ = 1f;

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
        /// エリアにはいいた敵の情報のリスト
        /// </summary>
        List<SearchInfo> enemySearchInfo_ = new List<SearchInfo>();

        /// <summary>
        /// エリアサイズを更新する大きさ
        /// </summary>
        float updateAreaSize_ = 0f;

        /// <summary>
        /// キャンセル可能のサイズ
        /// </summary>
        float canselSize_ = 0f;

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

            eria_ = GetComponent<SphereCollider>();
            eria_.enabled = false;

            mesh_ = GetComponent<MeshRenderer>();
            mesh_.enabled = false;

            skillDraw_ = FindObjectOfType<UI_Skill>();

            updateAreaSize_ = areaSizeMAX_ / areaMaxTime_;
            canselSize_ = areaSizeMAX_ / (areaMaxTime_ / cancelTime_);
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
            for (int i = 0; i < enemySearchInfo_.Count; i++)
            {
                // 元気だったら
                if (enemySearchInfo_[i].enemy_.isDown)
                {
                    // マテリアル戻してリストから削除
                    ReleaseMaterial(i);
                    continue;
                }

                // サーチエリア外だったら効果時間を減らす
                else if(!enemySearchInfo_[i].isInSearchArea)
                {
                    // 効果時間を減らす
                    enemySearchInfo_[i].skillEffectTime_ -= Time.deltaTime;

                    // 効果時間が切れたら
                    if(enemySearchInfo_[i].skillEffectTime_ <= 0f)
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
            scl.x = scl.y = scl.z = updateAreaSize_ * Time.deltaTime;
            transform.localScale += scl;

            // エリアサイズが最大値を超えたらスキャン終了
            if(transform.localScale.x >= areaSizeMAX_)
            {
                scl.x = scl.y = scl.z = areaSizeMAX_;
                transform.localScale = scl;
                Stop();
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
            if (canselSize_ > transform.localScale.x) return;

            //コライダーを無効にする
            eria_.enabled = false;
            mesh_.enabled = false;
            transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
            
            // スキルのインターバル設定
            SkillIntervalTimeCount_ = skillIntervalTime_;

            // UIにサーチがクールタイムになったことを知らせる
            if(skillDraw_ == null)
            {
                skillDraw_ = FindObjectOfType<UI_Skill>();
            }
            skillDraw_.StartCoolTime();

            // サーチ中フラグを無効化
            IsSearch = false;

            // スキル効果時間の設定
            for(int i=0;i< enemySearchInfo_.Count; i++)
            {
                // 既に効果時間が設定されているものはパス
                if (enemySearchInfo_[i].skillEffectTime_ > 0f)
                    continue;

                // そうでなければスキル効果時間を設定
                enemySearchInfo_[i].isInSearchArea = false;
                enemySearchInfo_[i].skillEffectTime_ = skillEffectTime_;
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
            // ヤクザは色を変える必要が無いので帰る
            if (enemy.Type == FrontPerson.Enemy.EnemyType.YAKUZA) return;
            
            
            //同じオブジェクトがもしあった場合効果時間を更新して帰る
            for (int i = 0; i < enemySearchInfo_.Count;i++)
            {
                if (enemySearchInfo_[i].enemy_.gameObject == other.gameObject)
                {
                    // エリア内フラグをtrueに戻す
                    enemySearchInfo_[i].isInSearchArea = true;
                    //効果時間を０にリセット
                    enemySearchInfo_[i].skillEffectTime_ = 0f;
                    return;
                }
            }

            // リストになかったので追加
            Renderer renderer = other.GetComponent<Renderer>();
            if (renderer == null)
            {
                // 見つからなかったから子供の方も探す
                renderer = other.GetComponentInChildren<Renderer>();
                if (renderer == null)
                {
                    Debug.LogError("サーチエリアに入ったオブジェクトにRendererが見つかりません。\nオブジェクト名:" + other.gameObject.name);
                    return;
                }
            }
            SearchInfo newSearchInfo = new SearchInfo();
            newSearchInfo.enemy_ = enemy;
            newSearchInfo.isInSearchArea = true;
            newSearchInfo.initMaterials_ = (renderer.materials);
            newSearchInfo.renderer = renderer;
            newSearchInfo.skillEffectTime_ = 0f;
            enemySearchInfo_.Add(newSearchInfo);

            // エネミーの足りないビタミンの種類によってセットするMaterialを変える
            Material[] materials = new Material[renderer.materials.Length];
            for (int i=0; i < materials.Length; i++)
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
            for (int i = 0; i < enemySearchInfo_.Count; i++)
            {
                // 違ったらコンティニュー
                if (enemySearchInfo_[i].enemy_.gameObject != other.gameObject)
                    continue;

                // 一緒だったらエリア内フラグをfalseにしてスキルの効果時間を設定
                enemySearchInfo_[i].isInSearchArea = false;
                enemySearchInfo_[i].skillEffectTime_ = skillEffectTime_;
                break;
            }
        }


        void ReleaseMaterial(int index)
        {
            // マテリアル戻してリストから削除
            enemySearchInfo_[index].renderer.materials = enemySearchInfo_[index].initMaterials_;
            enemySearchInfo_.RemoveAt(index);
        }



    }
}
