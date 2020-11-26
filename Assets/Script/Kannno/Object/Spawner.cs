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

        [Header("スポーンする敵の最大数")]
        [SerializeField, Range(0, 1000)]
        private int MaxCnt_OrdinaryPeople = 0;
        [SerializeField, Range(0, 1000)]
        private int MaxCnt_OldBattleaxe = 0;
        [SerializeField, Range(0, 1000)]
        private int MaxCnt_Yakuza = 0;

        static private int Max_OrdinaryPeople = 0;
        static private int Max_OldBattleaxe = 0;
        static private int Max_Yakuza = 0;

        // ステージ上にいる敵の数
        static private int Sum_OrdinaryPeople = 0;
        static private int Sum_OldBattleaxe = 0;
        static private int Sum_Yakuza = 0;

        [Header("生成までのクールタイム")]
        [SerializeField]
        private float time = 0f;

        private float current_time = 0f;

        /// <summary>
        /// 一般人の数を1つ減らす
        /// </summary>
        static public void Sub_OrdinaryPeople()
        {
            Sum_OrdinaryPeople--;
        }

        /// <summary>
        /// おばちゃんの数を1つ減らす
        /// </summary>
        static public void Sub_OldBattleaxe()
        {
            Sum_OldBattleaxe--;
        }

        /// <summary>
        /// ヤクザの数を1つ減らす
        /// </summary>
        static public void Sub_Yakuza()
        {
            Sum_Yakuza--;
        }

        void Start()
        {
            if(0 == Max_OrdinaryPeople && 0 == Sum_OldBattleaxe && 0 == Max_Yakuza)
            {
                Max_OrdinaryPeople = MaxCnt_OrdinaryPeople;
                Max_OldBattleaxe = MaxCnt_OldBattleaxe;
                Max_Yakuza = MaxCnt_Yakuza;
            }

            current_time = Time.timeSinceLevelLoad;

            ProbabilityList = new List<float> { Probability_OrdinaryPeople, Probability_OldBattleaxe, Probability_Yakuza };
            ProbabilityList.Sort((a, b) => a.CompareTo(b));

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
        private void Spawn()
        {
            // rand と ProbabilityListを昇順で比較して、その確率の敵を生成する

            float rand = Random.value;

            float probability_min = ProbabilityList[0];
            float probability_middle = ProbabilityList[1];
            float probability_max = ProbabilityList[2];

            if (probability_min >= rand)
            {
                if (probability_min == Probability_OrdinaryPeople && Sum_OrdinaryPeople < Max_OrdinaryPeople)
                {
                    Create_OrdinaryPeople();
                    return;
                }

                if (probability_min == Probability_OldBattleaxe && Sum_OldBattleaxe < Max_OldBattleaxe)
                {
                    Create_OldBattleaxe();
                    return;
                }

                if (probability_min == Probability_Yakuza && Sum_Yakuza < Max_Yakuza)
                {
                    Create_Yakuza();
                    return;
                }
            }

            if (probability_middle >= rand)
            {
                if (probability_middle == Probability_OrdinaryPeople && Sum_OrdinaryPeople < Max_OrdinaryPeople)
                {
                    Create_OrdinaryPeople();
                    return;
                }

                if (probability_middle == Probability_OldBattleaxe && Sum_OldBattleaxe < Max_OldBattleaxe)
                {
                    Create_OldBattleaxe();
                    return;
                }

                if (probability_middle == Probability_Yakuza && Sum_Yakuza <= Max_Yakuza)
                {
                    Create_Yakuza();
                    return;
                }
            }

            {
                if (probability_max == Probability_OrdinaryPeople && Sum_OrdinaryPeople < Max_OrdinaryPeople)
                {
                    Create_OrdinaryPeople();
                    return;
                }

                if (probability_max == Probability_OldBattleaxe && Sum_OldBattleaxe < Max_OldBattleaxe)
                {
                    Create_OldBattleaxe();
                    return;
                }

                if (probability_max == Probability_Yakuza && Sum_Yakuza < Max_Yakuza)
                {
                    Create_Yakuza();
                    return;
                }
            }
        }

        /// <summary>
        /// 一般人の生成
        /// </summary>
        private void Create_OrdinaryPeople()
        {
            OrdinaryPeople enemy = Instantiate(OrdinaryPeople, transform.position, Quaternion.identity).GetComponent<OrdinaryPeople>();

            Sum_OrdinaryPeople++;

            // 移動パターンの設定
            int cnt = Random.Range(0, OrdinaryPeople_MovePatternList.Count);

            EnemyState_Move ai = enemy.state_AI as EnemyState_Move;

            ai.Set_MovePattern(OrdinaryPeople_MovePatternList[cnt]);

            Debug.Log("一般人の生成");
        }

        /// <summary>
        /// おばちゃんの生成
        /// </summary>
        private void Create_OldBattleaxe()
        {
            OldBattleaxe enemy = Instantiate(OldBattleaxe, transform.position, Quaternion.identity).GetComponent<OldBattleaxe>();

            Sum_OldBattleaxe++;

            // 移動パターンの設定
            int cnt = Random.Range(0, OldBattleaxe_MovePatternList.Count);

            EnemyState_Move ai = enemy.state_AI as EnemyState_Move;

            ai.Set_MovePattern(OldBattleaxe_MovePatternList[cnt]);

            Debug.Log("おばちゃんの生成");
        }

        /// <summary>
        /// ヤクザの生成
        /// </summary>
        private void Create_Yakuza()
        {
            Yakuza enemy = Instantiate(Yakuza, transform.position, Quaternion.identity).GetComponent<Yakuza>();

            Sum_Yakuza++;

            // 移動パターンの設定
            int cnt = Random.Range(0, Yakuza_MovePatternList.Count);

            EnemyState_Move ai = enemy.state_AI as EnemyState_Move;

            ai.Set_MovePattern(Yakuza_MovePatternList[cnt]);

            Debug.Log("ヤクザの生成");
        }
    }
}