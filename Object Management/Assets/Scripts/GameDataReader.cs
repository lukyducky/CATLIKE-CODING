using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameDataReader {

    BinaryReader reader;

    public GameDataReader(BinaryReader reader)
    {
        this.reader = reader;
    }

    public float ReadFloat(){
        return reader.ReadSingle();
    }

    public int ReadInt()
    {
        return reader.ReadInt32();
    }

    public Quaternion ReadQuaternion()
    {
        Quaternion val;
        val.x = reader.ReadSingle();
        val.y = reader.ReadSingle();
        val.z = reader.ReadSingle();
        val.w = reader.ReadSingle();
        return val;

    }

    public Vector3 ReadVector3()
    {
        Vector3 value;
        value.x = reader.ReadSingle();
        value.y = reader.ReadSingle();
        value.z = reader.ReadSingle();
        return value;
    }

}
