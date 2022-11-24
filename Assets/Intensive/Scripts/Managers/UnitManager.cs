using Intensive.GameObjects;
using Intensive.Units.Player;
using Intensive.ScriptableObjects;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Zenject;
using Intensive.Units;
using System;

namespace Intensive.Managers
{
    public class UnitManager : MonoBehaviour
	{
		//Словарь с шаблонами игровых объектов, сортированных по их типам
		[Inject(Id = "Configuration")]
		private Dictionary<PrefabType, GameObject> _dictionary;
		[Inject]
		private QuestManager _questManager;
		[Inject]//Коллекция всех спаунеров на игровой карте
		private LinkedList<SpawnComponent> _spawners;
		[Inject]
		private LinkedList<EnemyData> _enemies;
		//Главный компонент игрока
		[Inject]
		private PlayerControllerComponent _player;

		//Игровой объект, в котором размещаются противники
		private Transform _enemiesPool;
		//Игровой объект, в котором размещаются снаряды игрока
		private Transform _projectilesPool;

		//Массивы типовых сущностей
		private LinkedList<BaseProjectileComponent> _bullets = new LinkedList<BaseProjectileComponent>();



		/// <summary>
		/// Делегат для создания событий инстанцирования игровых сущностей
		/// </summary>
		/// <param name="type">Тип игровой сущности</param>
		/// <param name="position">Положение сущности при создании</param>
		/// <param name="rotation">Поворот сущности при создании</param>
		/// <param name="data">Дополнительные данные о сущности</param>
		public delegate void SpawnGameObjectEventHandler(Vector3 position, Quaternion rotation, PrefabType type, MonoBehaviour source = null, ProjectileData data = default);

		private void Start()
		{
			//Добавление в контейнер всех энеми
			foreach (var enemy in _enemiesPool.GetComponentsInChildren<EnemyData>()) _enemies.AddLast(enemy);
			foreach (var spawn in transform.Find("SpawnsPool").GetComponentsInChildren<SpawnComponent>()) _spawners.AddLast(spawn);

			//Включение спаунеров и подписка на создание новых противников
			foreach (var spawner in _spawners)
			{
				spawner.OnSpawnEnemyEvent += (position, rotation, type, source, data) => StartCoroutine(OnSpawnEnemy(position, rotation, type, source));
				spawner.OnDiactivatedEvent += (q, qq) => _spawners.Remove((SpawnComponent)q);
			}

			//Подписываемся на событие создания снарядов
			_player.OnSpawnProjectileEvent += OnSpawnProjectile;
		}

		private void OnValidate()
		{
			//Находим игровой объект, вмещающий в себя всех противников
			_enemiesPool = GetComponentsInChildren<Transform>(true).First(t => t.name == "EnemiesPool");
			//Находим игровой объект, вмещающий в себя все снаряды игрока
			_projectilesPool = GetComponentsInChildren<Transform>(true).First(t => t.name == "ProjectilesPool");
		}

		#region Логика обновления состояния игровых объектов в каждом кадре

		private void Update()
		{
			UpdateEnemy();
			UpdateBullets();
		}

		//Обновление пуль
		private void UpdateBullets()
		{
			var removeList = new List<BaseProjectileComponent>();
			var time = Time.deltaTime;

			//Перемещение снарядов
			foreach (var proj in _bullets)
			{
				proj.LifeTime -= time;
				//Отметка, если снаряд должен уничтожиться
				if (proj.LifeTime <= 0f)
				{
					removeList.Add(proj);
					continue;
				}
				proj.transform.position += proj.transform.forward * proj.Speed * time;
			}

			//Удаление отработавших снарядов
			for (int i = 0; i < removeList.Count; i++)
			{
				_bullets.Remove(removeList[i]);
				Destroy(removeList[i].gameObject);
			}
		}

		//Логика противников
		private void UpdateEnemy()
		{
			var dieList = new LinkedList<EnemyData>();

			foreach (var enemy in _enemies)
			{
				//Атака или перемещение юнитом
				MoveOrAttackUnit(enemy);

				//Добавление погибающих юнитов в список погибших
				if (enemy.Health <= 0)
				{
					dieList.AddLast(enemy);
				}
			}

			//Удаление погибших юнитов
			foreach (var unit in dieList)
			{
				_enemies.Remove(unit);
				unit.State = EnemyState.Die;
				StartCoroutine(unit.OnDie());
				_questManager.OnCheckQuest();
			}
		}

		//Атака или перемещение юнитом
		private void MoveOrAttackUnit(EnemyData enemy)
		{
			var offset = _player.transform.position - enemy.transform.position;

			//Перемещение к игроку
			if (Vector3.SqrMagnitude(offset) > 3.5f * 3.5f)
			{
				//Перемещение к игроку юнитом
				enemy.transform.position += offset.normalized * enemy.MoveSpeed * Time.deltaTime;
				enemy.State = EnemyState.Run;
			}
			else if(enemy.AttackCooldown <= 0f)
			{
				enemy.State = EnemyState.Attack;
				enemy.ResetAttackCooldown();
				_player.SetDamage(enemy.Damage);
			}
			else enemy.State = EnemyState.Idle;

			//Нанесение ударов по игроку
			/*else if (Vector3.SqrMagnitude(offset) <= 3f * 3f)
			{
				if (enemy.AttackCooldown <= 0f)
				{
					enemy.State = EnemyState.Attack;
					enemy.ResetAttackCooldown();
					_player.SetDamage(enemy.Damage);
				}
				else enemy.State = EnemyState.Idle;
			}*/

			//Поворот противника к игроку
			enemy.transform.LookAt(_player.transform);
			//Атака восстанавливается
			if(enemy.AttackCooldown > 0f) enemy.AttackCooldown -= Time.deltaTime;
		}

		#endregion

		#region Методы создания игровых объектов

		//Создание нового противника и задержка перед началом управления
		private IEnumerator OnSpawnEnemy(Vector3 position, Quaternion rotation, PrefabType type, MonoBehaviour source)//todo ПРИДУМАТЬ КАКОЕ-НИБУДЬ БОЛЕЕ ЕМКОЕ СРЕДСТВО ПРОБРОСКИ (ФАБРИКА?)
		{
			var enemy = Instantiate(_dictionary[type], _enemiesPool).GetComponent<EnemyData>();
			_enemies.AddLast(enemy);
			enemy.transform.position = position;
			enemy.transform.rotation = rotation;
			//Помечаем 
			if (source is IWorldObject obj) enemy.QuestID = obj.QuestID;

			//Поворот противника к игроку
			enemy.transform.LookAt(_player.transform);

			var endPos = enemy.transform.position;
			var startPos = endPos;
			startPos.y = endPos.y - 2f;
			var timer = 1f;
			var coef = timer;

			while (timer > 0f)
			{
				enemy.transform.position = Vector3.Lerp(startPos, endPos, 1f - timer / coef);
				timer -= Time.deltaTime;
				yield return null;
			}
		}

		//Создает новый прожектайл и добавляет его в свою коллекцию
		private void OnSpawnProjectile(Vector3 position, Quaternion rotation, PrefabType type, MonoBehaviour source, ProjectileData data)
		{
			//Создаем новый снаряд
			var proj = Instantiate(_dictionary[type], _projectilesPool).GetComponent<BaseProjectileComponent>();
			//Устанавливаем снаряду положение, поворот и параметры
			proj.transform.position = position;
			proj.transform.rotation = rotation;
			proj.SetParams(data);

			//Добавляем в список отслеживания
			_bullets.AddLast(proj);
			//Подписываемся на попадание снарядом в игровые объекты
			proj.OnCollisionProjectile += (obj, args) =>
			{
				var projectile = (BaseProjectileComponent)obj;
				//Отыгрываем эффекты и звук от попадания
				EffectManager.PlayOneShot(projectile.EffectType, projectile.transform.position, projectile.transform.rotation);
				AudioManager.PlayAtPoint(args != null ? AudioType.ImpactEnemy : AudioType.ImpactMiss, projectile.transform.position);
				//Удаляем из списка и из игровой сцены
				_bullets.Remove(projectile);

				Destroy(projectile.gameObject);
			};
		}

		#endregion
	}
}
