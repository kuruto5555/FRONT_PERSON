using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FrontPerson.Character.Skill
{
    public class SearchArea : MonoBehaviour
    {
        [Header("Areaの広がる速度")]
        [SerializeField, Range(0.1f, 100f)]
        float speed_ = 1f;

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
        List<List<Material>> initMaterialsList_;

        /// <summary>
        /// Areaに入ったやつのマテリアル
        /// </summary>
        List<List<Material>> materialsList_;



        // Start is called before the first frame update
        void Start()
        {
            initMaterialsList_ = new List<List<Material>>();
            materialsList_ = new List<List<Material>>();

            eria_ = GetComponent<SphereCollider>();
            eria_.enabled = false;

            mesh_ = GetComponent<MeshRenderer>();
            mesh_.enabled = false;

        }

        // Update is called once per frame
        void Update()
        {
            if (!eria_.enabled) return;

            Vector3 scl = Vector3.zero;
            scl.x = scl.y = scl.z = speed_ * Time.deltaTime;
            transform.localScale += scl;
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
            for (int i = 0; i > initMaterialsList_.Count; i++)
            {
                materialsList_[i] = initMaterialsList_[i];
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Enemy")
            {
                Enemy01 enemy = other.GetComponent<Enemy01>();
                initMaterialsList_.Add(other.GetComponent<MeshRenderer>().materials.ToList());
                materialsList_.Add(other.GetComponent<MeshRenderer>().materials.ToList());

                // エネミーの足りないビタミンの種類によってセットするMaterialを変える
                foreach(Material material in other.GetComponent<MeshRenderer>().materials)
                {
                }
            }
        }


    }
}
