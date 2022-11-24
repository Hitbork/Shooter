
namespace Intensive
{
	/// <summary>
	/// Перечисления состояний противников
	/// </summary>
	public enum EnemyState : byte
	{
		Idle = 0,
		Run = 1,
		Attack = 10,
		Die = 255
	}

	/// <summary>
	/// Перечисление типов шаблонов объектов
	/// </summary>
	public enum PrefabType : byte
	{
		/// <summary>
		/// Стандартная пуля
		/// </summary>
		Bullet = 0,
		/// <summary>
		/// Подствольная граната
		/// </summary>
		Grenade = 1,
		/// <summary>
        /// Дробинка
        /// </summary>
		SmallBullet = 2,
		/// <summary>
		/// Зеленый противник
		/// </summary>
		GreenEnemy = 50,
	}

	/// <summary>
	/// Перечисление типов шаблонов эффектов
	/// </summary>
	public enum EffectType : byte
	{
		/// <summary>
		/// Маленький взрыв
		/// </summary>
		SmallExposion = 0,
		/// <summary>
		/// Большой взрыв
		/// </summary>
		BigExposion = 1,
	}

	/// <summary>
	/// Состояние событий квестов
	/// </summary>
	public enum QuestEventType : byte
	{
		Inactive = 0,
		Start = 1,
		Completed = 2,
	}

	/// <summary>
	/// Перечисление типов звуков
	/// </summary>
	public enum AudioType : byte
	{
		/// <summary>
		/// Простой выстрел игрока
		/// </summary>
		PlayerShot = 0,
		/// <summary>
		/// Попадание по противнику
		/// </summary>
		ImpactEnemy = 10,
		/// <summary>
		/// Промах простым выстрелом
		/// </summary>
		ImpactMiss = 11,
		/// <summary>
		/// Основной сандтрек
		/// </summary>
		MainSoundtrack = 254
	}

	/// <summary>
	/// Перечисление типов событий
	/// </summary>
	public enum EventType : byte
	{
		/// <summary>
		/// Стартовый диалог
		/// </summary>
		Dialog_1 = 0,
		/// <summary>
		/// Уничтожение стартовых противников
		/// </summary>
		Dialog_2 = 1,
		/// <summary>
		/// Движение в сторону точки связи
		/// </summary>
		Dialog_3 = 2,
		/// <summary>
		/// Диагностика точки связи
		/// </summary>
		Dialog_4 = 3,
		/// <summary>
		/// Нахождение коммутатора
		/// </summary>
		Dialog_5 = 4,
		/// <summary>
		/// Починка точки связи
		/// </summary>
		Dialog_6 = 5,
		/// <summary>
		/// Завершение миссии
		/// </summary>
		Dialog_7 = 6,
		Quest_1 = 150,
		Quest_2 = 151,
		Quest_3 = 152,
		Quest_4 = 153,
		CreateSpawner = 250,
		CheckPoint = 251,
	}

	/// <summary>
	/// Тэг кинематографичной камеры
	/// </summary>
	public enum CameraTagType : byte
	{
		Start = 0,
		Start1 = 1,
		Start2 = 2,
		Start3 = 3,
		Start4 = 4,
		Start5 = 5,
		Start6 = 6,
		Start7 = 7,
		Start8 = 8,
		Start9 = 9,
		Start10 = 10,
		Start11 = 11,
		Start12 = 12,
		Start13 = 13,
		Start14 = 14,
		Start15 = 15,
		Start16 = 16,
		Start17 = 17,
		Start18 = 18,
		Start19 = 19,
		Start20 = 20,
		Start21 = 21,
		Start22 = 22,
		Start23 = 23,
		Start24 = 24,
		Start25 = 25,
		Start26 = 26,
		Start27 = 27,
		Start28 = 28,
		Start29 = 29,
		Start30 = 30,
		Start31 = 31,
		Start32 = 32
	}
}
