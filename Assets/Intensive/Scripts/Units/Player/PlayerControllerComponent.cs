using Intensive.Managers;
using Intensive.ScriptableObjects;

using System;
using System.Threading.Tasks;

using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

using Zenject;

using static Intensive.Managers.UnitManager;
using static UnityEngine.InputSystem.InputAction;

namespace Intensive.Units.Player
{
    public class PlayerControllerComponent : MonoBehaviour
    {
        //Аниматор, через него происходит унимирование модели игрока
        private Animator _animator;
        //Система управления, она считывает наш ввод с мышки и клавиатуры
        private WarriorControls _controls;
        //Физическое тело, компонент, отвечающий за физику модели
        //private Rigidbody _rigidBody;
        //Компонент, который регистрирует контакты с другими объектами в сцене
        private ColliderComponent _collider;

        //Коллекция оружия
        private WeaponData[] _weapons;
        //Номер текущего выбранного оружия
        private int _currentWeaponIndex;
        private bool _isFalling;
        //Наш скелет смаштабирован, нужно приведение масштабов
        private Vector3 _rescaleConst = Vector3.one;

        //Скорость перемещения персонажа
        private float _moveSpeed;
        //Сила прыжка
        private float _jumpForce;
        //Скорость вращения прицела
        private float _focusSpeed;

        [Header("Конфигурация боевой системы")]
        [Tooltip("Ось лазерного прицела")]
        [SerializeField]
        private Transform _laserAxis;

        [Tooltip("Луч лазера")]
        [SerializeField]
        private Transform _laser;

        [Tooltip("Точка лазерного прицела")]
        [SerializeField]
        private Transform _laserPoint;

        [Tooltip("Точка выстрела")]
        [SerializeField]
        private Transform _firePoint;

        [Tooltip("Всполохи пороха")]
        [SerializeField]
        private ParticleSystem _muzzleEffect;

        [Tooltip("Отработанные патроны")]
        [SerializeField]
        private ParticleSystem _bulletEffect;

        [Space]
        [Tooltip("Рендер оружия"), SerializeField]
        private SkinnedMeshRenderer _weaponRenderer;

        [Tooltip("Компонент, отвечающий за интерфейс игрока"), SerializeField]
        private PlayerUIComponent _playerUI;


        /// <summary>
        /// Событие создания снаряда
        /// </summary>
        public event SpawnGameObjectEventHandler OnSpawnProjectileEvent;



        public void ReloadAnimation()
        {
            _animator.SetTrigger("Reload");
        }

        /// <summary>
        /// Получение урона игроком
        /// </summary>
        /// <param name="damage">Полученный урон</param>
        public void SetDamage(ushort damage)
        {
            _playerUI.SetDamage(damage);

        }

        public async void OnStart()
        {
            enabled = true;
            GetComponentInChildren<Camera>().enabled = true;
            GetComponentInChildren<AudioListener>().enabled = true;
            _playerUI.gameObject.SetActive(true);

            //Фикс бага с прицеливанием
            await Task.Yield();
            _laserAxis.localEulerAngles = Vector3.zero;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }

        [Inject]
        private void Construct(PlayerConfiguration stats, WeaponConfiguration weapon)
        {
            //Установка параметров выживаемости игрока
            _playerUI.SetHealth(stats.GetHealth, stats.GetDelayRegenerationInSec, stats.GetHitPointRegenPerSec);
            //Установка прочих параметров персонажа
            _moveSpeed = stats.GetMoveSpeed; _jumpForce = stats.GetJumpForce; _focusSpeed = stats.GetFocusSpeed;

            _weapons = weapon.GetWeaponData;
            //Конфигурирование данных для интерфейса
            var weaponData = new (string, WeaponOverheatData)[_weapons.Length];
            for (int i = 0; i < _weapons.Length; i++) weaponData[i] = (_weapons[i].WeaponName, _weapons[i].WeaponOverheat);
            _playerUI.SetIndicatorParams(weaponData);
        }

        private void Awake()
		{
            _controls = new WarriorControls();
        }

		private void OnValidate()
		{
            //_rigidBody = GetComponentInChildren<Rigidbody>();
            _collider = GetComponentInChildren<ColliderComponent>();
            _animator = GetComponentInChildren<Animator>();
        }

		void Start()
        {
            _collider.OnCollisionEnterEvent += (sender, collider) =>
            {
                _isFalling = false;
                _animator.SetTrigger("Fall");
            };

            _collider.OnCollisionExitEvent += (sender, collider) =>
            {
                _isFalling = true;
            };

            //Расчет рескейлинга для дочерних элементов
            var parent = _laserAxis.parent;
            while(parent != transform)
            {
                _rescaleConst.x *= parent.localScale.x;
                _rescaleConst.y *= parent.localScale.y;
                _rescaleConst.z *= parent.localScale.z;
                parent = parent.parent;
            }

            _rescaleConst.x = 1 / _rescaleConst.x;
            _rescaleConst.y = 1 / _rescaleConst.y;
            _rescaleConst.z = 1 / _rescaleConst.z;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }

        private void OnEnable()
		{
            _controls.WarriorInput.Enable();
            _controls.WarriorInput.Jump.performed += OnJump;
            _controls.WarriorInput.WeaponSwitch.performed += OnWeaponSwitch;
        }

		private void OnDisable()
		{
            _controls.WarriorInput.Disable();
            _controls.WarriorInput.Jump.performed -= OnJump;
            _controls.WarriorInput.WeaponSwitch.performed -= OnWeaponSwitch;
        }

		private void Update()
		{
            OnMove();
            OnFire();
            OnFocus();

            if (Keyboard.current.tabKey.isPressed)
            {
                EditorApplication.isPaused = true;
            }
        }

        //Движение персонажа
        private void OnMove()
        {
            var velocity = transform.position;

            var direction = _controls.WarriorInput.Movement.ReadValue<Vector2>();

            _animator.SetFloat("ForwardMove", direction.y);
            _animator.SetFloat("SideMove", direction.x);

            var move = _moveSpeed * Time.deltaTime;

            if (Keyboard.current[Key.LeftShift].isPressed) move *= 5;
            direction *= move; 

            velocity += transform.forward * direction.y;
            velocity += transform.right * direction.x;
            transform.position = velocity;
        }

        [Tooltip("Сет RigidBody")]
        [SerializeField]
        public Rigidbody _r;
        //Прыжок персонажа
        private async void OnJump(CallbackContext context)
        {
            if (_isFalling) return;

            _animator.SetTrigger("StartJump");

            await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(0.25));
            _r.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
        }

        //Смена оружия персонажа
        private void OnWeaponSwitch(CallbackContext context)
        {
            //Смена оружия и проверка, чтобы индекс оружия не выходил за размеры массива
            _currentWeaponIndex = Mathf.Clamp(min: 0, max: _weapons.Length - 1, 
                value: context.ReadValue<float>() > 0 
                    ? _currentWeaponIndex + 1 
                    : _currentWeaponIndex - 1);

            //Обновление интерфейса
            _playerUI.SwitchWeapon(_currentWeaponIndex);
        }

        //Стрельба из оружия
        private void OnFire()
        {
            if (Mouse.current.leftButton.isPressed)
            {
                //Если оружие в перезарядке - не стреляем
                if (!_playerUI.WeaponIsReady) return;

                if (_weapons[_currentWeaponIndex].ProjectileType != PrefabType.SmallBullet)
                {
                    OnSpawnProjectileEvent?.Invoke(_firePoint.position, _firePoint.rotation, _weapons[_currentWeaponIndex].ProjectileType, data: _weapons[_currentWeaponIndex].Projectile);
                } else
                {
                    for (int i = 0; i < 30; i++)
                    {
                        var random = new Vector3(UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-10f, 10f), 0f);
                        OnSpawnProjectileEvent?.Invoke(_firePoint.position, Quaternion.Euler(_firePoint.eulerAngles + random), _weapons[_currentWeaponIndex].ProjectileType, data: _weapons[_currentWeaponIndex].Projectile);
                    }  
                }

                _muzzleEffect.Play();
                _bulletEffect.Play();
                _playerUI.PlayerShot(_weapons[_currentWeaponIndex].CooldownFire);

                AudioManager.PlayOneShot(AudioType.PlayerShot);
            }
            _animator.SetBool("Fire", Mouse.current.leftButton.isPressed);
        }

        //Вращение прицелом
        private void OnFocus()
        {//todo Перевести на кватернионы
            //Вращение персонажа по курсору мыши
            var velocity = transform.eulerAngles;

            var direction = _controls.WarriorInput.Focus.ReadValue<Vector2>();
            direction *= _focusSpeed * Time.deltaTime;
            //velocity.x += direction.y * -1;
            velocity.y += direction.x;
            transform.eulerAngles = velocity;
            //Наклон оружия по вертикали
            velocity = _laserAxis.localEulerAngles;
            velocity.x = ClampFocusAxis(velocity.x, direction.y);
            _laserAxis.localEulerAngles = velocity;

            //Наведение лазерного прицела
            var distance = 10000f;


            if(Physics.Raycast(_laserAxis.position, _laserAxis.forward, out var hit))
            {
                distance = hit.distance * _rescaleConst.z;
                _laserPoint.gameObject.SetActive(true);
                _laserPoint.position = hit.point;
            }
            else
            {
                _laserPoint.gameObject.SetActive(false);
            }

            _laser.localPosition = new Vector3(0f, 0f, distance / 2);
            _laser.localScale = new Vector3(1f, 1f, distance / 2);
        }

        //Обрезание развертки прицела
        private float ClampFocusAxis(float y, float delta)
        {
            var angle = Mathf.DeltaAngle(y, 0f);
            if (delta == 0f) return y;
            //Ось поднимается вверх приращение положительное
            if(angle > 10f) return -10f;
            //Ось опускается вниз приращение отрицательное
            if(angle < -30f) return 30f;

            return y - delta;
        }
    }
}