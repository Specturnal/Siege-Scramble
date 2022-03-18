using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoadHandler {
    public static void Save<T>(T saveData, string file) {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(Application.persistentDataPath + "/" + file, FileMode.Create);

        binaryFormatter.Serialize(fileStream, saveData);

        fileStream.Close();
        Debug.Log("File saved");
    }

    public static bool Load<T>(string file, ref T saveData) {
        if (File.Exists(Application.persistentDataPath + "/" + file)) {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(Application.persistentDataPath + "/" + file, FileMode.Open);
            if (binaryFormatter.Deserialize(fileStream) is T save) {
                saveData = save;
                Debug.Log("File loaded");
                fileStream.Close();
                return true;
            } else {
                Debug.Log("Incompatible file found, default values will be loaded");
                fileStream.Close();
                return false;
            }
        } else {
            Debug.Log("File not found, default values will be loaded");
            return false;
        }
    }

    public static void Delete(string file) {
        if (File.Exists(Application.persistentDataPath + "/" + file)) {
            File.Delete(Application.persistentDataPath + "/" + file);
            Debug.Log("File deleted");
        } else
            Debug.Log("File not found");
    }
}
