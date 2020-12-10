using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontPerson.Weapon
{

    public class ShotGun : SpecialWeapon
    {
        [Header("発射するペレットの数")]
        [SerializeField, Range(0, 20)] int PelletNum = 12;

        [Header("弾の拡散角度")]
        [SerializeField, Range(0.0f, 30.0f)] float Angle = 0.0f;

        // Start is called before the first frame update
        void Start()
        {
            base.Start();
            _type = Constants.WEAPON_TYPE.SHOT_GUN;
        }

        // Update is called once per frame
        void Update()
        {
            base.Update();
        }

        public override void Shot()
        {
            if (shotTime_ > 0.0f) return;
            if (Ammo < 1) return;

            for(int i = 0; i < PelletNum; i++)
            { 
                float randX = Random.Range(-Angle, Angle);
                float randY = Random.Range(-Angle, Angle);
                Debug.Log(randX);
                Debug.Log(randY);

                GameObject bullet = Instantiate(bullet_, Muzzle.transform.position, Muzzle.transform.rotation, null);
                bullet.transform.Rotate(randX, randY, 0.0f, Space.World);
            }
            
            shotTime_ = 1.0f / rate_;
            ammo_--;
            _bountyManager.FireCount();
            Instantiate(MuzzleFlash, Muzzle.transform.position, Quaternion.identity, transform);
        }
    }
}