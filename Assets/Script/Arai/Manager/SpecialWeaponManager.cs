using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.Weapon;

namespace FrontPerson.Manager
{
    public class SpecialWeaponManager : MonoBehaviour
    {
        [Header("スペシャル武器を入れる")]
        [SerializeField] public List<GameObject> WeaponPrefabList;


        public static SpecialWeaponManager _instance { get; private set; }

        /// <summary>
        /// 武器種の数
        /// </summary>
        public int _weaponNum 
        { 
            get;
            private set;
        }

        private void Awake()
        {
            if (_instance == null) _instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            _weaponNum = WeaponPrefabList.Count;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}