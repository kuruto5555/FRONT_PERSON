using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.Enemy;
using FrontPerson.Constants;
using System.Linq;

namespace FrontPerson.Weapon
{
    public class Launcher : SpecialWeapon
    {
        [Header("ロックオンできる数")]
        [SerializeField, Range(1, 100)] int LockOnMax = 10;

        [Header("ロックオン時間")]
        [SerializeField, Range(0.0f, 10.0f)] float LockOnTime = 1.0f;

        [Header("ロックオン解除する時間")]
        [SerializeField, Range(1.0f, 30.0f)] float TargerLostTime = 10.0f;

        //[Header("生成するキャンバス")]
        //[SerializeField] GameObject Canvas_ = null;

        [Header("生成するロックオンカーソルプレハブ")]
        [SerializeField] GameObject Cursor_ = null;

        [Header("弾の拡散角度")]
        [SerializeField, Range(0.0f, 10.0f)] float Angle = 0.0f;

        /// <summary>
        /// 今ロックオンしてるやつ
        /// </summary>
        private Transform _lockOnTarget = null;

        /// <summary>
        /// 射出中かどうか
        /// </summary>
        private bool _isFire = false;

        private Canvas _gameUI = null;

        /// <summary>
        /// ロックオンで使う情報
        /// </summary>
        private class LockOnInfo
        {
            float _lostTime;
            LockOnUI _lockOnCursor;

            public LockOnInfo(float lostTime)
            {
                _lostTime = lostTime;
                _lockOnCursor = null;
            }  

            public void LostTimeUpdate()
            {
                _lostTime -= Time.deltaTime;
            }

            public float GetLostTime()
            {
                return _lostTime;
            }

            public void SetLostTime(float time)
            {
                _lostTime = time;
            }

            public void SetLockOnCursor(LockOnUI cursor)
            {
                _lockOnCursor = cursor;
            }

            public LockOnUI GetCursor()
            {
                return _lockOnCursor;
            }
        }

        

        /// <summary>
        /// 大体視界内に入っているEnemy
        /// </summary>
        //private Dictionary<Transform, LockOnInfo> _enemyDictionary;

        private List<Transform> _enemyList;

        /// <summary>
        /// ロックオンしたターゲットの座標
        /// </summary>
        private Dictionary<Transform, LockOnInfo> _lockOnTargetList;

        /// <summary>
        /// トリガーが引かれたかどうか
        /// </summary>
        private bool _isTrigger = false;

        /// <summary>
        /// 今のロックオン時間
        /// </summary>
        private float _nowLockOnTime = 0.0f;

        /// <summary>
        /// 画面内か判定するためのRect
        /// </summary>
        Rect _rect = new Rect(0, 0, 1, 1);

        /// <summary>
        /// カメラの参照
        /// </summary>
        private Camera _targetCamera = null;

        private new void Awake()
        {
            _enemyList = new List<Transform>();

            _lockOnTargetList = new Dictionary<Transform, LockOnInfo>();

            _canvas = GameObject.Find("WeaponUI");
        }

        // Start is called before the first frame update
        new void Start()
        {
            base.Start();

            _type = Constants.WEAPON_TYPE.MISSILE;
            
            _isTrigger = false;
            
            _nowLockOnTime = 0.0f;
            _targetCamera = Camera.main;

            _shotSoundPath = SEPath.GAME_SE_FIRE_LANCHER;

            _gameUI = GameObject.Find("GameUI_Canvas").GetComponent<Canvas>();
        }

        // Update is called once per frame
        new void Update()
        {
            if (_isAnimation) return;

            base.Update();
            EnemyListUpdate();
            LockOnStart();
            UpdateLockOn();
            LockOnTargetUpdate();

            Fire();
        }


        public override void Shot()
        {
            if (_lockOnTargetList.Count != 0 && !_isTrigger)
            {
                ammo_ = MaxAmmo_ = _lockOnTargetList.Count;
            }
            _isTrigger = true;
        }

        private void Fire()
        {
            if (!_isTrigger) return;

            //誰にもロックオンしてなかったら
            if (_lockOnTargetList.Count == 0)
            {
                //弾を撃ってたら終わり
                if (_isFire) ammo_ = 0;

                _isTrigger = false;
                return;
            }

            if (shotTime_ > 0.0f) return;

            _isFire = true;

            float randX = Random.Range(-Angle, Angle);
            float randY = Random.Range(-Angle, Angle);

            GameObject bullet = Instantiate(bullet_, Muzzle.transform.position, Muzzle.transform.rotation, null);
            bullet.transform.Rotate(randX, randY, 0.0f, Space.Self);
            ammo_--;
            
            bullet.GetComponent<Missile>().SetData(_lockOnTargetList.First().Key, _lockOnTargetList.First().Value.GetCursor());
            shotTime_ = 1.0f / rate_;
            _bountyManager.FireCount();

            _lockOnTargetList.Remove(_lockOnTargetList.FirstOrDefault().Key);
            Instantiate(MuzzleFlash, Muzzle.transform.position, Muzzle.transform.rotation);

            _audioManager.Play3DSE(transform.position, _shotSoundPath);
            _animator.Play("Shot", 0, 0);
        }

        private void UpdateLockOn()
        {
            if (_isTrigger) return;
            if (_lockOnTarget == null) return;

            if (IsDrawDisplay(_lockOnTarget.position))
            {
                if (_nowLockOnTime < LockOnTime)
                {
                    _nowLockOnTime += Time.deltaTime;
                }
                else
                {
                    LockOn();
                    _nowLockOnTime = 0;
                }
            }

            else
            {
                _lockOnTarget = null;
                _nowLockOnTime = 0;
            }   

        }

        private void LockOnStart()
        {
            if (_isTrigger) return;
            if (_lockOnTarget != null) return;
            if (_lockOnTargetList.Count >= LockOnMax) return;

            foreach(var it in _enemyList)
            {
                //重複ロックオン対策
                if (_lockOnTargetList.ContainsKey(it))
                {
                    continue;
                }

                //帰ってる途中ならロックオンしない
                if (it.GetComponent<Character.Enemy>().isDown)
                {
                    continue;
                }

                //画面内に入ってたら
                if (IsDrawDisplay(it.position))
                {
                    _lockOnTarget = it;
                    return;
                }
            }
        }

        private void LockOn()
        {
            _lockOnTargetList.Add(_lockOnTarget, new LockOnInfo(TargerLostTime));

            //適当にけつにつっこむ
            _enemyList.Remove(_lockOnTarget);
            _enemyList.Add(_lockOnTarget);

            LockOnUI ui = Instantiate(Cursor_, _canvas.transform).GetComponent<LockOnUI>();

            ui.SetData(_lockOnTarget, _gameUI);

            
            _lockOnTargetList[_lockOnTarget].SetLockOnCursor(ui);

            _lockOnTarget = null;

            //ロックオンの音ならす
            _audioManager.Play3DSE(transform.position, SEPath.GAME_SE_LOCK_ON);
        }

        private void EnemyListUpdate()
        {
            List<Transform> destroyIndexs = new List<Transform>();

            foreach (var it in _enemyList)
            {
                if(it == null)
                {
                    destroyIndexs.Add(it);
                    continue;
                }
            }

            foreach (var it in destroyIndexs)
            {
                _enemyList.Remove(it);
            }
        }

        private void LockOnTargetUpdate()
        {
            if (_lockOnTargetList.Count == 0) return;

            List<Transform> destroyIndexs = new List<Transform>();

            foreach(var it in _lockOnTargetList)
            {

                //ロックオン中の敵が返っていなくなった奴の対処
                if(it.Key == null)
                {
                    destroyIndexs.Add(it.Key);
                    continue;
                }

                if (IsDrawDisplay(it.Key.position))
                {
                    //見えていたら初期化
                    it.Value.SetLostTime(TargerLostTime);
                }
                else
                {
                    if(it.Value.GetLostTime() > 0)
                    {
                        it.Value.LostTimeUpdate();
                    }
                    else
                    {
                        destroyIndexs.Add(it.Key);
                    }
                }
            }

            foreach (var it in destroyIndexs)
            {
                _lockOnTargetList[it].GetCursor().SetDead();
                _lockOnTargetList.Remove(it);
                return;
            }

            destroyIndexs.Clear();
        }

        /// <summary>
        /// メインカメラに映っていたらtrue
        /// </summary>
        /// <param name="pos">オブジェクトのポジション</param>
        /// <returns></returns>
        private bool IsDrawDisplay(Vector3 pos)
        {
            var viewportPos = _targetCamera.WorldToViewportPoint(pos);

            if (_rect.Contains(viewportPos))
            {
                Vector3 targetToCameraDirection_N = (_targetCamera.transform.position - pos).normalized;
                //正規化したベクトルの内積が一定以下なら見たことにする
                if (Vector3.Dot(targetToCameraDirection_N, _targetCamera.transform.forward.normalized) < -0.5)
                {

                    //壁貫通対策
                    {
                        Vector3 vec = (pos - Camera.main.transform.position).normalized;
                        RaycastHit hit;
                        int layerMask = 1 << LayerNumber.ENEMY | 1 << LayerNumber.FIELD_OBJECT; //enemyとFildeObjectだけぶつける

                        if(Physics.Raycast(Camera.main.transform.position, vec, out hit, 100.0f, layerMask))
                        {
                            if (LayerNumber.ENEMY != hit.collider.gameObject.layer)
                            {
                                return false;
                            }
                        }

                        
                    }

                    return true;
                }
                
            }

            return false;
        }

        public override void ChangeWeapon()
        {
            if (Ammo <= 0)
            {
                foreach (var it in _lockOnTargetList)
                {
                    it.Value.GetCursor().SetDead();
                }

                Destroy(gameObject);
            }
        }

        public override void PutAnimation()
        {
            foreach (var it in _lockOnTargetList)
            {
                it.Value.GetCursor().SetDead();
            }

            Destroy(gameObject);
        }

        public override void WeaponForcedChange()
        {
            foreach (var it in _lockOnTargetList)
            {
                it.Value.GetCursor().SetDead();
            }

            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != TagName.ENEMY) return;
            _enemyList.Add(other.transform);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag != "Enemy") return;
            _enemyList.Remove(other.transform);
        }
    }
}