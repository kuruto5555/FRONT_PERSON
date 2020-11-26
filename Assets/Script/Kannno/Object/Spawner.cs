using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

using FrontPerson.Enemy.AI;

namespace FrontPerson.Enemy
{
    public class Spawner : MonoBehaviour
    {
        [Header("スポーンする敵オブジェクト")]
        [SerializeField]
        private GameObject OrdinaryPeople = null;
        [SerializeField]
        private GameObject OldBattleaxe = null;
        [SerializeField]
        private GameObject Yakuza = null;

        [Header("スポーンする敵の確率")]
        [SerializeField, Range(0f, 1f)]
        private float Probability_OrdinaryPeople = 0f;
        [SerializeField, Range(0f, 1f)]
        private float Probability_OldBattleaxe = 0f;
        [SerializeField, Range(0f, 1f)]
        private float Probability_Yakuza = 0f;

        // 確率のリスト(計算用)
        private List<float> ProbabilityList = null;

        [Header("一般人のMovePatternのリスト")]
        [SerializeField]
        private List<MovePattern> OrdinaryPeople_MovePatternList = new List<MovePattern>();

        [Header("おばちゃんのMovePatternのリスト")]
        [SerializeField]
        private List<MovePattern> OldBattleaxe_MovePatternList = new List<MovePattern>();

        [Header("ヤクザのMovePatternのリスト")]
        [SerializeField]
        private List<MovePattern> Yakuza_MovePatternList = new List<MovePattern>();

        [Header("生成までのクールタイム")]
        [SerializeField]
        private float time = 0f;

        private float current_time = 0f;

        void Start()
        {
            current_time = Time.timeSinceLevelLoad;

            ProbabilityList = new List<float> { Probability_OrdinaryPeople, Probability_OldBattleaxe, Probability_Yakuza };
            ProbabilityList.Sort();

            Spawn();
        }

        void Update()
        {
            if (time <= (Time.timeSinceLevelLoad - current_time))
            {
                current_time = Time.timeSinceLevelLoad;

                Spawn();
            }
        }

#if UNITY_EDITOR
        // 場所がわかるようにGizmo描画
        private void OnDrawGizmos()
        {
            Handles.color = Color.blue;
            Handles.DrawSolidArc(transform.position, Vector3.up, Quaternion.Euler(0f, 0f, 0f) * transform.forward, 360f, 0.5f);
        }
#endif

        /// <summary>
        /// 敵生成関数
        /// </summary>
        void Spawn()
        {
            float rand = Random.value;

            foreach(var probability in ProbabilityList)
            {
                if(probability >= rand)
                {
                    if (probability == Probability_OrdinaryPeople)
                    {
                        Create_OrdinaryPeople();
                        return;
                    }

                    if (probability == Probability_OldBattleaxe)
                    {
                        Create_OldBattleaxe();
                        return;
                    }

                    if (probability == Probability_Yakuza)
                    {
                        Create_Yakuza();
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 一般人の生成
        /// </summary>
        void Create_OrdinaryPeople()
        {
            OrdinaryPeople enemy = Instantiate(OrdinaryPeople, transform.position, Quaternion.identity).GetComponent<OrdinaryPeople>();

            // 移動パターンの設定
            int cnt = Random.Range(0, OrdinaryPeople_MovePatternList.Count);

            EnemyState_Move ai = enemy.state_AI as EnemyState_Move;

            ai.Set_MovePattern(OrdinaryPeople_MovePatternList[cnt]);

            Debug.Log("一般人の生成");
        }

        /// <summary>
        /// おばちゃんの生成
        /// </summary>
        void Create_OldBattleaxe()
        {
            OldBattleaxe enemy = Instantiate(OldBattleaxe, transform.position, Quaternion.identity).GetComponent<OldBattleaxe>();

            // 移動パターンの設定
            int cnt = Random.Range(0, OldBattleaxe_MovePatternList.Count);

            EnemyState_Move ai = enemy.state_AI as EnemyState_Move;

            ai.Set_MovePattern(OldBattleaxe_MovePatternList[cnt]);

            Debug.Log("おばちゃんの生成");
        }

        /// <summary>
        /// ヤクザの生成
        /// </summary>
        void Create_Yakuza()
        {
            Yakuza enemy = Instantiate(Yakuza, transform.position, Quaternion.identity).GetComponent<Yakuza>();

            // 移動パターンの設定
            int cnt = Random.Range(0, Yakuza_MovePatternList.Count);

            EnemyState_Move ai = enemy.state_AI as EnemyState_Move;

            ai.Set_MovePattern(Yakuza_MovePatternList[cnt]);

            Debug.Log("ヤクザの生成");
        }
    }
}