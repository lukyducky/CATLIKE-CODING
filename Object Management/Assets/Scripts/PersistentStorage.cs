﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PersistentStorage : MonoBehaviour {
    //saves and loads a single persistent object.

    string savePath;

	void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "saveFile"); //from system.io; combine so that you get a file not just folder

    }

    public void Save (PersistableObject obj)
    {
        using (
            var writer = new BinaryWriter(File.Open(savePath, FileMode.Create)))
        {
            obj.Save(new GameDataWriter(writer));
        }
    }

    public void Load (PersistableObject obj)
    {
        using ( 
            var reader = new BinaryReader(File.Open(savePath, FileMode.Open)))
        {
            obj.Load(new GameDataReader(reader));
        }
    }

}