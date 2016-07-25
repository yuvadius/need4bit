using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public class PersistantDepository : MonoBehaviour {
    public string saveFileName;
    private string savePath;
    private List<Storable> infos;

    void Awake() {
        infos = new List<Storable>();
        savePath = Application.persistentDataPath +  "/" + saveFileName;
        DontDestroyOnLoad(this.gameObject);
        load();
    }

    void OnDestroy() {
        save();
    }

    public List<Storable> getStorage() {
        return infos;
    }

    public void addNewInfo(Storable newInfo) {
        infos.Add(newInfo);
    }

    public void deleteInfoById(string id) {
        Storable doomed = null;
        foreach (Storable storable in infos) {
            if (storable.getId() == id) {
                doomed = storable;
            }
        }
        if (doomed != null) {
            infos.Remove(doomed);
        }
    }


    public Storable getInfoById(string id) {
        foreach (Storable storable in infos) {
            if (storable.getId() == id) {
                return storable;
            }
        }
        return null;
    }

    public void load() {
        if (File.Exists(savePath)) {

            FileStream fs = File.Open(savePath, FileMode.Open);

            try {
                BinaryFormatter bf = new BinaryFormatter();
                infos = (List<Storable>)bf.Deserialize(fs);
                Debug.Log("successfully loaded from: " + savePath);
            } catch (SerializationException e) {
                Debug.LogError("Couldn't deserialize content " + e.Message);
            } finally {
                fs.Close();
            }
        } else {
            infos = new List<Storable>();
        }
    }

    public void save() {
        FileStream fs = File.Create(savePath);

        try {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, infos);
            Debug.Log("successfully saving to: " + savePath);
        } catch (SerializationException e) {
            Debug.Log("failed to serialize " + e.Message);
        } finally {
            fs.Close();
        }
    }

}
