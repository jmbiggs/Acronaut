using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PlayerState : MonoBehaviour {

	public static PlayerState state;

	public PlayerData playerData;

	// Use this for initialization
	void Start () {
		if (state == null) {
			DontDestroyOnLoad(this.gameObject);
			state = this;

		}
		else if (state != this) {
			Destroy(this.gameObject);
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	[System.Serializable]
	public class LevelState {
		public int level;
		public float bestTime;
		public bool levelUnlocked;

		public LevelState(int lev, float time, bool unlocked) {
			level = lev;
			bestTime = time;
			levelUnlocked = unlocked;
		}
	}

	[System.Serializable]
	public class PlayerData {
		public string playerName;
		public LevelState[] levelData;

		public PlayerData() {
			levelData = new LevelState[5];
		}

	}

	public void Save() {
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");
		PlayerData data = playerData;
		bf.Serialize(file, data);
		file.Close();
	}

	public void Load() {
		if (File.Exists(Application.persistentDataPath + "/playerInfo.dat")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open (Application.persistentDataPath + "/playerInfo.data", FileMode.Open);
			PlayerData data = (PlayerData)bf.Deserialize(file);
			playerData = data;
			file.Close();
		}

	}
}
