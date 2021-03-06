﻿using System;
using System.Data;
using System.Timers;
using UnityEngine;
using Shooting.Systems;

namespace Shooting.Fields
{
    public class EnemyFactory : MonoBehaviour
    {
        [SerializeField] private EnemyDataTable enemyDataTable = null;
        [SerializeField] private GameObject enemyPrefab = null;
        [SerializeField] private float enemyPopInterval = 1;

        [SerializeField] private Transform enemyPopAnchor = null;

        private System.Random _random;
        private float _currentTime = 0;
        private bool _isPlaying = true;

        private void Awake()
        {
            _random = new System.Random(DateTime.Now.Millisecond);

            EventManager.GameOverEvent += () => _isPlaying = false;
        }

        private void Update()
        {
            // ゲームプレイ中でないなら何もしない
            if (!_isPlaying) return;
            
            if (_currentTime >= enemyPopInterval)
            {
                var index = _random.Next(0, enemyDataTable.table.Count);
                var data = enemyDataTable.table[index];

                var enemyObj = Instantiate(data.Model);
                enemyObj.transform.position = new Vector3(enemyPopAnchor.position.x, 
                    (float)(_random.NextDouble() - 0.5f) * 2 * enemyPopAnchor.position.y, 0);

                var enemy = enemyObj.GetComponent<Enemy>();
                enemy.Initialize(data.HP, data.DefeatPoint);

                _currentTime = 0;
            }
            else
            {
                _currentTime += Time.deltaTime;
            }
        }
    }
}
