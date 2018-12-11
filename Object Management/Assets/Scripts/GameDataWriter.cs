using System.IO;
using UnityEngine;

public class GameDataWriter {
    //not being attached to gameObject therefore, doesn't need monobehaviour
    //is a wrapper around Binary writer to make life easier for us

    BinaryWriter writer;

    public GameDataWriter(BinaryWriter writer) //custom CTOR; replacing implicit default CTOR
    {
        this.writer = writer;
    }

    public void Write(float value) {
        writer.Write(value);
    }

    public void Write(int val) {
        writer.Write(val);
    }

    public void Write(Quaternion value) {
        writer.Write(value.x);
        writer.Write(value.y);
        writer.Write(value.z);
        writer.Write(value.w);
    }

    public void Write(Vector3 value) {
        writer.Write(value.x);
        writer.Write(value.y);
        writer.Write(value.z);
    }

    public void Write (Color val) {
        writer.Write(val.r);
        writer.Write(val.g);
        writer.Write(val.b);
        writer.Write(val.a);
    }

    public void Write (Random.State value) {
        //random.state is a struct.  it keeps track of where in random sequence we are.  
        //So that we can recreate the randomly created shapes when reloading a scene
        //Stores floats that are not directly accessible, but is serialzable so we use JSON.

        Debug.Log(JsonUtility.ToJson(value));


    }
}
