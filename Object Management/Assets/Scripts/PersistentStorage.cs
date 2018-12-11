using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PersistentStorage : MonoBehaviour {
    //saves and loads a single persistent object.

    string savePath;

    void Awake() {
        savePath = Path.Combine(Application.persistentDataPath, "saveFile"); //from system.io; combine so that you get a file not just folder

    }

    public void Save(PersistableObject obj, int vers) {
        using(
            var writer = new BinaryWriter(File.Open(savePath, FileMode.Create))) {
            writer.Write(-vers);
            obj.Save(new GameDataWriter(writer));
        }
    }

    public void Load(PersistableObject obj) {
        byte[] data = File.ReadAllBytes(savePath);
        var reader = new BinaryReader(new MemoryStream(data));
        obj.Load(new GameDataReader(reader, -reader.ReadInt32()));
    }

}
