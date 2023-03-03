using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
public class SavingManager : MonoBehaviour
{
    public static string savePath => $"{Application.persistentDataPath}/save/";

    private void Start()
    {
        CheckPoint.CheckPointHit += SaveAll;
        PauseMenuController.Restarting += LoadAll;
        PlayerManager.PlayerDied += LoadAll;

        SaveAll();
        
    }
    private void OnDestroy()
    {
        CheckPoint.CheckPointHit -= SaveAll;
        PauseMenuController.Restarting -= LoadAll;
        PlayerManager.PlayerDied -= LoadAll;
    }
    private void SaveAll()
    {
        foreach (var saveable in FindObjectsOfType<Saveable>())
        {
            saveable.GetComponent<ISaveable>().Save();
        }

    }
    private void LoadAll()
    {
        foreach (var saveable in FindObjectsOfType<Saveable>())
        {
            saveable.GetComponent<ISaveable>().Load();
        }

    }
    public static void Save<T>(T objectToSave,string key)
    {
        Directory.CreateDirectory($"{savePath}/");
        BinaryFormatter formatter = new BinaryFormatter();
        using (var stream = File.Open($"{savePath}/{key}.txt", FileMode.Create))
        {
            formatter.Serialize(stream, objectToSave);
        }
    }
    public static T Load<T>(string key)
    {
        Directory.CreateDirectory($"{savePath}/");
        BinaryFormatter formatter = new BinaryFormatter();
        T returnValue = default(T);
        using (var stream = File.Open($"{savePath}/{key}.txt", FileMode.Open))
        {
            returnValue = (T)formatter.Deserialize(stream);

        }
        return returnValue;
    }

    public static bool SaveExists(string key)
    {
        return File.Exists($"{savePath}/{key}.txt");
    }

    public static void ClearFile()
    {
        string Path = $"{savePath}/";
        DirectoryInfo directory = new DirectoryInfo(Path);
        directory.Delete(true);
        Directory.CreateDirectory(Path);
    }
}
