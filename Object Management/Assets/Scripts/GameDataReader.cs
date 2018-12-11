using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameDataReader {

    public int Version { get; }

    BinaryReader reader;
    
    public GameDataReader(BinaryReader reader, int version) {
        this.reader = reader;
        this.Version = version;
    }

    public float ReadFloat() {
        return reader.ReadSingle();
    }

    public int ReadInt() {
        return reader.ReadInt32();
    }

    public Quaternion ReadQuaternion() {
        Quaternion val;
        val.x = reader.ReadSingle();
        val.y = reader.ReadSingle();
        val.z = reader.ReadSingle();
        val.w = reader.ReadSingle();
        return val;

    }

    public Vector3 ReadVector3() {
        Vector3 value;
        value.x = reader.ReadSingle();
        value.y = reader.ReadSingle();
        value.z = reader.ReadSingle();
        return value;
    }

    public Color ReadColor() {
        Color val;
        val.r = reader.ReadSingle();
        val.g = reader.ReadSingle();
        val.b = reader.ReadSingle();
        val.a = reader.ReadSingle();
        return val;
    }

    public Random.State ReadRandomState() {
        return JsonUtility.FromJson<Random.State>(reader.ReadString());
    }
}
