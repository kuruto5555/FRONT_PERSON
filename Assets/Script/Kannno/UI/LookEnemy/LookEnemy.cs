using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using FrontPerson.Character;

namespace FrontPerson.UI
{
    public class LookEnemy : MonoBehaviour
    {
        [SerializeField]
        private List<Image> Images = new List<Image>();

        private Player Player = null;

        private List<Character.Enemy> Enemies = new List<Character.Enemy>();

        void Start()
        {
            Player = GameObject.FindGameObjectWithTag(Constants.TagName.PLAYER).GetComponent<Player>();

#if UNITY_EDITOR
            if(0 == Images.Count)
            {
                Debug.Log("Images が設定されていません");
            }
#endif

            foreach(var image in Images)
            {
                image.gameObject.SetActive(false);
            }
        }

        void Update()
        {
            //if(Player.isAlart)
            //{
            //    Look();
            //}
        }

        private void Look()
        {

        }

        public void AddEnemy(Character.Enemy enemy)
        {
            if(false == Enemies.Contains(enemy))
            {
                Enemies.Add(enemy);
            }
        }

        public void DeleteEnemy(Character.Enemy enemy)
        {
            if (Enemies.Contains(enemy))
            {
                Enemies.Remove(enemy);
            }
        }
    }
}