using System;
using UnityEngine;

namespace Common.UnityLogic.Units.Health
{
    public sealed class UnitHealth : MonoBehaviour
    {
        public event Action Died;
        
        [SerializeField] private HealthCanvas _healthCanvas;
        
        private float _maxHP;
        private float _hp;

        public bool IsAlive => _hp > 0;

        public void Init(in float maxHp)
        {
            _maxHP = maxHp;
            _hp = maxHp;
            
            _healthCanvas.Show();
            UpdateSlider();
        }
        
        public void TakeDamage(in float damage)
        {
            if (!IsAlive) return;

            _hp -= damage;
            if (_hp < 0) _hp = 0;
            
            UpdateSlider();
            if (_hp == 0)
            {
                _healthCanvas.Hide();
                Died?.Invoke();
            }
        }

        private void UpdateSlider() => _healthCanvas.UpdateHP(_hp / _maxHP);
        private void OnDisable() => _healthCanvas.Hide();
    }
}