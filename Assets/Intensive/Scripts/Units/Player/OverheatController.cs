using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Intensive.Units.Player
{
    public class OverheatController : MonoBehaviour
    {
        //Допустимая температура после перегрева
        private float _mediumTemperature = 0.2f;
        //Время начала охлаждения оружия
        private float _delayCooling = .5f;
        //Время начала охлаждения оружия после перегрева
        private float _delayCoolingAfterOverheat = 2.5f;
        //Нагрев за выстрел. 1 равна 100% нагрева
        private float _warmingPerShot = 0.01f;
        //Потеря процентов в секунду
        private float _coolingInSec = 0.04f;

        private Coroutine _coroutine;

        [Tooltip("Название оружия")]
        [SerializeField]
        private Text _weaponName;

        [Tooltip("Фоновая картинка индикатора")]
        [SerializeField]
        private Image _backgroundImage;

        [Tooltip("Шкала нагрева оружия")]
        [SerializeField]
        private Image _indicator;



        /// <summary>
        /// Готово-ли оружие к стрельбе
        /// </summary>
        public bool WeaponIsReady { get; private set; } = true;

        /// <summary>
        /// Добавление температуры оружию
        /// </summary>
        public void AddTemperature()
        {
            _indicator.fillAmount += _warmingPerShot;

            //Сброс охлаждения
            if (_coroutine != null) StopCoroutine(_coroutine);

            //Перегрев оружия
            if (_indicator.fillAmount >= 1f) _coroutine = StartCoroutine(CoolingAfterOverheat());
            //Нагрев оружия
            else _coroutine = StartCoroutine(Cooling());
		}

        /// <summary>
        /// Переключение режима активности индикатора
        /// </summary>
        /// <param name="isActive">Активен-ли индикатор</param>
        public void Active(bool isActive)
        {
            var color = _backgroundImage.color;
            if (isActive)
            {
                color.a = 1f;
                _backgroundImage.color = color;
                color = _indicator.color;
                color.a = 1f;
                _indicator.color = color;
                color = _weaponName.color;
                color.a = 1f;
                _weaponName.color = color;

                transform.localScale = Vector3.one;
			}
            else
            {
                color.a = .5f;
                _backgroundImage.color = color;
                color = _indicator.color;
                color.a = .5f;
                _indicator.color = color;
                color = _weaponName.color;
                color.a = .5f;
                _weaponName.color = color;
                transform.localScale = new Vector3(.7f, .7f, 0f);
			}
		}

        /// <summary>
        /// Установка параметров индикации оружия
        /// </summary>
        public void SetParams(string name, WeaponOverheatData data)
        {
            _weaponName.text = name;
            _mediumTemperature = data.MediumTemperature;
            _delayCooling = data.DelayCooling;
            _delayCoolingAfterOverheat = data.DelayCoolingAfterOverheat;
            _warmingPerShot = data.WarmingPerShot;
            _coolingInSec = data.CoolingInSec;
        }

		//Охлаждение
		private IEnumerator Cooling()
        {
            yield return new WaitForSeconds(_delayCooling);

            while (_indicator.fillAmount > 0f)
            {
                _indicator.fillAmount -= _coolingInSec * Time.deltaTime;
                yield return null;
            }
        }

        //Охлаждение после перегрева
        private IEnumerator CoolingAfterOverheat()
        {
            WeaponIsReady = false;

            yield return new WaitForSeconds(_delayCoolingAfterOverheat);

            while(_indicator.fillAmount > 0f)
            {
                _indicator.fillAmount -= _coolingInSec * Time.deltaTime;
                //Оружие пришло в норму
                WeaponIsReady = _indicator.fillAmount < _mediumTemperature;
                yield return null;
			}
		}
    }
}
