using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Intensive.Units.Player
{
    public class PlayerUIComponent : MonoBehaviour
    {
        //Индекс текущего оружия
        private int _currentWeaponIndex;
        //Текущее время между выстрелами
        private float[] _currentCooldowns;

        //Корутина анимации значения здоровья
        private Coroutine _animationHP;
        //Корутина регенерации здоровья
        private Coroutine _regenerationHP;
        //Максимальное здоровье игрока
        private float _maxHealth;
        //Текущее здоровье игрока
        private float _currentHealth;
        //Задержка перед началом регенерации
        private float _delayRegenerationInSec;
        //Количество восстанавливаемого здоровья в секунду
        private float _hitPointRegenPerSec;


        [Tooltip("Индикаторы оружия"), SerializeField]
        private OverheatController[] _weaponIndicators;

        [Tooltip("Шкала здоровья"), SerializeField]
        private Image _HPFillImage;

        [Tooltip("Значение текущего здоровья"), SerializeField]
        private Text _HPCountText;



        /// <summary>
        /// Событие смерти игрока
        /// </summary>
        public event EventHandler OnDieEvent;

        /// <summary>
        /// Готово-ли к стрельбе оружие
        /// </summary>
        public bool WeaponIsReady => _weaponIndicators[_currentWeaponIndex].WeaponIsReady && _currentCooldowns[_currentWeaponIndex] <= 0f;

        /// <summary>
        /// Выстрел из оружия
        /// </summary>
        public void PlayerShot(float cooldown)
        {
            _weaponIndicators[_currentWeaponIndex].AddTemperature();
            _currentCooldowns[_currentWeaponIndex] = cooldown;
        }

        /// <summary>
        /// Установка максимального здоровья игрока
        /// </summary>
        /// <param name="health">Количество здоровья</param>
        public void SetHealth(int health, float delayRegenerationInSec, float hitPointRegenPerSec)
        {
            _maxHealth = health; _currentHealth = _maxHealth;
            _delayRegenerationInSec = delayRegenerationInSec;
            _hitPointRegenPerSec = hitPointRegenPerSec;

            _HPCountText.text = _currentHealth + " / " + _maxHealth;
            _HPFillImage.fillAmount = _currentHealth / _maxHealth;
        }

        /// <summary>
        /// Нанесение урона игроку
        /// </summary>
        /// <param name="damage">Количество урона</param>
        public void SetDamage(float damage)
        {
            _currentHealth -= damage;
            //Выводим на экран текущее количество здоровья
            _HPCountText.text = Mathf.RoundToInt(_currentHealth) + " / " + _maxHealth;
            //Изменяем значение шкалы
            _HPFillImage.fillAmount = _currentHealth / _maxHealth;

            //Останавливаем и запускаем анимацию значения здоровья снова
            if(_animationHP != null) StopCoroutine(_animationHP);
            _animationHP = StartCoroutine(HitPointChanged(damage));

            //Если игрок получил урон - корутина регенерации обновляется
            if(damage > 0)
            {
                if(_regenerationHP != null) StopCoroutine(_regenerationHP);
                _regenerationHP = StartCoroutine(RegenerationHP());
			}

            //При поражении игрока, оповещаем игру
            if (_currentHealth <= 0)
            {
                OnDieEvent?.Invoke(this, null);//todo подписать на это квестМенеджер
                Time.timeScale = 0f;
            }
        }

        /// <summary>
        /// Смена интерфейса оружия
        /// </summary>
        /// <param name="index">Индекс оружия</param>
        public void SwitchWeapon(int newWeaponIndex)
        {
            //Если оружие не поменялось
            if (_currentWeaponIndex == newWeaponIndex) return;

            _weaponIndicators[_currentWeaponIndex].Active(false);

            //Выводит индикатор вперед в иерархии
            _currentWeaponIndex = newWeaponIndex;
            _weaponIndicators[_currentWeaponIndex].transform.SetAsLastSibling();
            _weaponIndicators[_currentWeaponIndex].Active(true);
        }

        /// <summary>
        /// Установка параметров индикации для оружия
        /// </summary>
        public void SetIndicatorParams((string, WeaponOverheatData)[] weaponData)
        {
            for(int i = 0; i < _weaponIndicators.Length; i++)
            {
                _weaponIndicators[i].SetParams(weaponData[i].Item1, weaponData[i].Item2);
			}
		}

		private void Start()
		{
            _currentCooldowns = new float[_weaponIndicators.Length];
        }

		private void Update()
		{
            //Откат перезарядок орудий
            for (int i = 0; i < _currentCooldowns.Length; i++)
            {
                if(_currentCooldowns[i] > 0f)
                    _currentCooldowns[i] -= Time.deltaTime;
            }

            //Регенерация здоровья игрока
        }

		private void OnEnable()
		{
            //_overheatController.gameObject.SetActive(true);
        }

		private void OnDisable()
		{
            //_overheatController?.gameObject.SetActive(false);
        }

        //Анимация значения здоровья
        private IEnumerator HitPointChanged(float damage)
        {
            var timer = 0.25f;
            //Если урон - значения краснеют, иначе - зеленеют
            var endColor = damage > 0 ? Color.red : Color.green;
            var startScale = 44f;
            var endScale = 60f;
            float delta;

            while(timer > 0f)
            {
                timer -= Time.deltaTime;
                //Рассчитываем интерполяцию
                delta = 1f - timer * 4;
                //Увеличиваем размер текста
                _HPCountText.fontSize = Mathf.RoundToInt(Mathf.Lerp(startScale, endScale, delta));
                //Подкрашиваем текст
                _HPCountText.color = Color.Lerp(Color.white, endColor, delta);
                yield return null;
            }

            timer = 0.25f;
            while(timer > 0f)
            {
                timer -= Time.deltaTime;
                delta = 1f - timer * 4;
                _HPCountText.fontSize = Mathf.RoundToInt(Mathf.Lerp(endScale, startScale, delta));
                _HPCountText.color = Color.Lerp(endColor, Color.white, delta);
                yield return null;
            }

            _animationHP = null;
		}

        //Регенерация здоровья
        private IEnumerator RegenerationHP()
        {
            yield return new WaitForSeconds(_delayRegenerationInSec);

            while(_currentHealth != _maxHealth)
            {
                if (_currentHealth != _maxHealth) SetDamage(-_hitPointRegenPerSec);
                yield return new WaitForSeconds(1f);
			}
		}
    }
}