using System;
using Common.Infrastructure.Factories.Zenject;
using UnityEngine;
using Zenject;

namespace Common.UnityLogic.Units.Health
{
    public sealed class UnitHealth : MonoBehaviour
    {
        public event Action Died;

        [SerializeField] private Transform _healthPoint;
        
        private HealthCanvas _healthCanvas;

        private IZenjectFactory _zenjectFactory;
        
        private float _maxHP;
        private float _hp;

        public bool IsAlive => _hp > 0;
        public float HP => _hp;

        [Inject]
        private void Construct(IZenjectFactory zenjectFactory)
        {
            _zenjectFactory = zenjectFactory;
            
            Init();
        }

        public void Setup(in float maxHp)
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

        private void Init()
        {
            if (_healthCanvas is null)
            {
                var asset = Resources.Load<HealthCanvas>("UnityLogic/HpBar/HpSlider");
                _healthCanvas = _zenjectFactory.Instantiate(asset, _healthPoint);
            }
        }
        private void UpdateSlider() => _healthCanvas.UpdateHP(_hp / _maxHP);
        private void OnDisable() => _healthCanvas.Hide();
    }
}