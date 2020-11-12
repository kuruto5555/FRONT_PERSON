using FrontPerson.Constants;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FrontPerson.Enemy;
using FrontPerson.Constants;


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

        [Header("足りないビタミンがCの時のマテリアル")]
        [SerializeField]
        Material vitaminCMat_ = null;

        [Header("足りないビタミンがDの時のマテリアル")]
        [SerializeField]
        Material vitaminDMat_ = null;

        /// <summary>
        /// サーチエリアのコライダー
        /// </summary>
        SphereCollider eria_ =null;

        /// <summary>
        /// メッシュレンダラー
        /// </summary>
        MeshRenderer mesh_ = null;

        /// <summary>
        /// エリアに入ったやつのマテリアル情報を保存。
        /// Searchをやめたときに戻して、リセットする
        /// </summary>
        List<Material[]> initMaterialsList_;

        /// <summary>
        /// Areaに入ったやつのマテリアル
        /// </summary>
        List<Renderer> rendererList_;


        // Start is called before the first frame update
        void Start()
        {
            initMaterialsList_ = new List<Material[]>();
            rendererList_ = new List<Renderer>();

            eria_ = GetComponent<SphereCollider>();
            eria_.enabled = false;

            mesh_ = GetComponent<MeshRenderer>();
            mesh_.enabled = false;

        }

        // Update is called once per frame
        void Update()
        {
            if (!eria_.enabled) return;
            if (transform.localScale.x >= areaSizeMAX_) return;

            Vector3 scl = Vector3.zero;
            scl.x = scl.y = scl.z = speed_ * Time.deltaTime;
            transform.localScale += scl;
            if(transform.localScale.x >= areaSizeMAX_)
            {
                scl.x = scl.y = scl.z = areaSizeMAX_;
                transform.localScale = scl;
            }

        }

        public void Search()
        {
            eria_.enabled = true;
            mesh_.enabled = true;
        }

        public void Stop()
        {
            //コライダーを無効にする
            eria_.enabled = false;
            mesh_.enabled = false;
            transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);

            //マテリアルを戻す
            if (initMaterialsList_.Count == 0) return;
            for (int i = 0; i < initMaterialsList_.Count; i++)
            {
                rendererList_[i].materials = initMaterialsList_[i];
            }

            rendererList_.Clear();
            initMaterialsList_.Clear();
        }

        //出てった処理
        private void OnTriggerExit(Collider other)
        {
            for (int i = 0; i < rendererList_.Count; i++)
            {
                // 違ったらコンティニュー
                if (rendererList_[i].gameObject != other.gameObject)
                    continue;

                // 一緒だったらマテリアル戻してリストから削除
                rendererList_[i].materials = initMaterialsList_[i];
                rendererList_.RemoveAt(i);
                initMaterialsList_.RemoveAt(i);
                break;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            //バグ対策
            foreach (Renderer render in rendererList_)
            {
                if(render.gameObject == other.gameObject) return;
            }

            if(other.tag == TagName.ENEMY)
            {
                Enemy enemy = other.GetComponent<Enemy>();
                initMaterialsList_.Add(other.GetComponent<Renderer>().materials);
                rendererList_.Add(other.GetComponent<Renderer>());

                // エネミーの足りないビタミンの種類によってセットするMaterialを変える
                Renderer renderer = other.GetComponent<Renderer>();
                Material[] materials = renderer.materials;
                for (int i=0; i < other.GetComponent<Renderer>().materials.Length; i++)
                {
                    // とりあえずビタミンCのいろにしちゃう
                    //materials[i] = vitaminCMat_;

                    
                    if(enemy.LackVitamins == VITAMIN_TYPE.VITAMIN_C)
                    {
                        materials[i] = vitaminCMat_;
                    }
                    else if(enemy.LackVitamins == VITAMIN_TYPE.VITAMIN_D)
                    {
                        materials[i] = vitaminDMat_;
                    }
                    
                }
                renderer.materials = materials;
            }
        }


    }
}
