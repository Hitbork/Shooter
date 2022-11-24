using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

namespace Intensive.UI
{
    public class TooltipAssistant : MonoBehaviour
    {
        private ConfigData _config;
        private ModTriggerComponent[] _triggers;

        [SerializeField]
        private Text _headerText;

        [SerializeField]
        private Text _descText;

		private void Start()
		{
            foreach (var trigger in _triggers) trigger.OnTriggerEnterEvent += ShowTooltip;

        }

		public void Construct(string path)
        {
            if (!File.Exists(Application.dataPath + path + _path)) return;

            using (var file = File.OpenRead(Application.dataPath + path + _path))
            {
                var formatter = new BinaryFormatter();
                _config = (ConfigData)formatter.Deserialize(file);
            }
        }

        public async void ShowTooltip(object sender, string args)
        {
            var index = Convert.ToInt32(args);

            var text = new string[] { string.Empty, string.Empty };

            for(int i = 0; i < _config.Data[0].Length; i++)
            {
                text[0] += (char)(_config.Data[0][i] - _path.Length);
			}
            for (int i = 0; i < _config.Data[index].Length; i++)
            {
                text[1] += (char)(_config.Data[index][i] + _path.Length);
            }

            gameObject.SetActive(true);
            _headerText.text = text[0];
            _descText.text = text[1];

            await Task.Delay(TimeSpan.FromSeconds(5f));
            gameObject.SetActive(false);
		}

		private void OnValidate()
		{
            _triggers = FindObjectsOfType<ModTriggerComponent>();
        }

        public void Up()
        {
            foreach (var obj in _triggers) obj.Met();
		}

		[Serializable]
        public struct ConfigData
        {
            public int[][] Data;
        }

        private string _path = "settings.dat";
    }
}