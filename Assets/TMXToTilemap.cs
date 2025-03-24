using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine.Tilemaps;

public class TMXToTilemap : MonoBehaviour
{
	public Tilemap tilemap; 
	public TileBase[] tiles;

	private void Start()
	{
		LoadMap("tilemap.json"); 
	}

	void LoadMap(string filename)
	{
		string path = Path.Combine(Application.streamingAssetsPath, filename);
		if (!File.Exists(path))
		{
			Debug.LogError("Map file not found: " + path);
			return;
		}

		string json = File.ReadAllText(path);
		JObject mapData = JObject.Parse(json);

		int width = (int)mapData["width"];
		int height = (int)mapData["height"];

		JArray layers = (JArray)mapData["layers"];
		foreach (JObject layer in layers)
		{
			if ((string)layer["type"] == "tilelayer") 
			{
				JArray data = (JArray)layer["data"];
				for (int i = 0; i < data.Count; i++)
				{
					int tileID = (int)data[i] - 1; 
					if (tileID >= 0 && tileID < tiles.Length)
					{
						int x = i % width;
						int y = height - (i / width) - 1;
						tilemap.SetTile(new Vector3Int(x, y, 0), tiles[tileID]);
					}
				}
			}
		}
	}
}
