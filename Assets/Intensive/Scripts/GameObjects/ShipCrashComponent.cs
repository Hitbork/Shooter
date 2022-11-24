using Intensive.Units.Player;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

using UnityEngine;

namespace Intensive
{
    public class ShipCrashComponent : MonoBehaviour
    {
        private Animator _animator;

        [Tooltip("Взрывы")]
        [SerializeField]
        private ParticleSystem[] _explosions;

        [Tooltip("Взрывы плазмы")]
        [SerializeField]
        private ParticleSystem[] _plazmas;

        [Tooltip("Мусор из корабля")]
        [SerializeField]
        private GameObject[] _obstacles;

        void Start()
        {
            _animator = GetComponent<Animator>();
        }

        /// <summary>
        /// Запуск скрипта падения корабля
        /// </summary>
        public void CrashStart()
        {
            _animator.SetBool("Crash", true);
		}

        private void BigExposions_EditorEvent(int count)
        {
            switch(count)
            {
                case 1:
                    _explosions[0].Play();
                    break;
                case 2:
                    for(int i = 0; i < 2; i++) _explosions[i].Play();
                    break;
                case 3:
                    for (int i = 0; i < 3; i++) _explosions[i].Play();
                    break;
                case 4:
                    for (int i = 0; i < 4; i++) _explosions[i].Play();
                    break;
			}
		}

        private void PlazmaExposions_EditorEvent(int count)
        {
            switch (count)
            {
                case 1:
                    _plazmas[0].Play();
                    break;
                case 2:
                    for (int i = 0; i < 2; i++) _plazmas[i].Play();
                    break;
            }
        }

        private void ObstaclesDrops()
        {

		}

		#region Parser
		/*
		
        [Tooltip("Точки постановки камеры")]
        [SerializeField]
        private Transform[] _axises;

        [Tooltip("Позиции камеры в ролике")]
        [SerializeField]
        private TransformData[] _cinemaData;

        [Tooltip("Камера ролика")]
        [SerializeField]
        private Camera _cinemaCamera;

        /// <summary>
        /// Создает файл для парсинга положений камеры
        /// </summary>
        private void CreateArchiveFile()
        {
            //Создается корневой каталог с данными модели
            var xmlRoot = new XElement("TransformData");

            //Добавляем каждую часть тела в документ
            foreach (var data in _axises)
            {
                var element = new XElement("Body");
                element.Add(new[] { new XAttribute("PosX", data.position.x), new XAttribute("PosY", data.position.y), new XAttribute("PosZ", data.position.z),
                     new XAttribute("RotX", data.eulerAngles.x),  new XAttribute("RotY", data.eulerAngles.y),  new XAttribute("RotZ", data.eulerAngles.z)});
                xmlRoot.Add(element);
            }

            var doc = new XDocument(xmlRoot);
            doc.Save(Application.dataPath + "//Intensive//Resources//CinemaData.xml");
        }

        private void Load()
        {
            var xmlRoot = XDocument.Load(Application.dataPath + "//Intensive//Resources//CinemaData.xml").Root;
            var list = new List<TransformData>();
            var type = CameraTagType.Start;
            foreach (var element in xmlRoot.Elements("Body"))
            {
                var data = new TransformData() { Tag = type };
                var x = element.Attribute("PosX").Value.Replace('.', ',');
                var y = element.Attribute("PosY").Value.Replace('.', ',');
                var z = element.Attribute("PosZ").Value.Replace('.', ',');
                data.Position = new Vector3(Convert.ToSingle(x), Convert.ToSingle(y), Convert.ToSingle(z));
                x = element.Attribute("RotX").Value.Replace('.', ',');
                y = element.Attribute("RotY").Value.Replace('.', ',');
                z = element.Attribute("RotZ").Value.Replace('.', ',');
                data.Rotation = new Vector3(Convert.ToSingle(x), Convert.ToSingle(y), Convert.ToSingle(z));
                list.Add(data);

                type++;
            }

            _cinemaData = list.ToArray();
        }

        private IEnumerator Cinematic()
        {
            //yield return new WaitForSeconds(_delay);
            //_animator.enabled = true;
            var linkedList = new LinkedList<TransformData>(_cinemaData);
            //Установили камеру в первую точку
            var nextPoint = linkedList.First;
            _cinemaCamera.transform.position = nextPoint.Value.Position;
            _cinemaCamera.transform.eulerAngles = nextPoint.Value.Rotation;
            var prefPoint = nextPoint;
            nextPoint = nextPoint.Next;

            //Пока не пройдем по всем точкам
            while(nextPoint != null)
            {
                var timer = 0f;
                //Пока не пройдет одна секунда
                while(timer < 10f)
                {
                    _cinemaCamera.transform.position = Vector3.Lerp(prefPoint.Value.Position, nextPoint.Value.Position, timer / 10);
                    _cinemaCamera.transform.eulerAngles = Vector3.Lerp(prefPoint.Value.Rotation, nextPoint.Value.Rotation, timer / 10);
                    timer += Time.deltaTime;
                    yield return null;
				}

                prefPoint = nextPoint;
                nextPoint = nextPoint.Next;
                Debug.Log(nextPoint.Value.Tag);
			}

            Destroy(_cinemaCamera.gameObject);
            var player = FindObjectOfType<PlayerControllerComponent>().enabled = true;
        }
        */
		#endregion
	}

	[Serializable]
    public struct TransformData
    {
        public Vector3 Position;
        public Vector3 Rotation;
        public CameraTagType Tag;
	}
}
