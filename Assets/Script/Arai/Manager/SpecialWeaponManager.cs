using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.Weapon;

namespace FrontPerson.Manager
{
    public class SpecialWeaponManager : MonoBehaviour
    {
        [SerializeField] public List<GameObject> WeaponPrefabList;

        //public List<SpecialWeapon> WeaponList = null;

        public static SpecialWeaponManager _instance { get; private set; }

        public int _weaponNum 
        { 
            get;
            private set;
        }

        private void Awake()
        {
            if (_instance == null) _instance = this;
            //if (WeaponList == null) WeaponList = new List<SpecialWeapon>();
        }

        // Start is called before the first frame update
        void Start()
        {
            _weaponNum = WeaponPrefabList.Count;
            int cnt = 0;
            foreach(var i in WeaponPrefabList)
            {
                //WeaponList.Add(WeaponPrefabList[cnt].GetComponent<SpecialWeapon>());
                cnt++;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}