using System.IO;
using UnityEngine;

public class GameDataWriter  {
    //not being attached to gameObject therefore, doesn't need monobehaviour
    //is a wrapper around Binary writer to make life easier for us

    BinaryWriter writer;

    public GameDataWriter(BinaryWriter writer) //custom CTOR; replacing implicit default CTOR
    {
        this.writer = writer;
    }

    public void Write (float value)
    {
        writer.Write(value);
    }

    public void Write (int val)
    {
        writer.Write(val);
    }

    public void Write (Quaternion value)
    {
        writer.Write(value.x);
        writer.Write(value.y);
        writer.Write(value.z);
        writer.Write(value.w);
    }

    public void Write (Vector3 value)
    {
        writer.Write(value.x);
        writer.Write(value.y);
        writer.Write(value.z);
    }
}
