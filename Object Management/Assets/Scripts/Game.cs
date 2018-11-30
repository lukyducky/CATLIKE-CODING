using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : PersistableObject {
    #region Data

    public ShapeFactory shapeFactory;
    public PersistentStorage storage;

    const int saveVersion = 1;

    public KeyCode createKey = KeyCode.C;
    public KeyCode newGameKey = KeyCode.N;
    public KeyCode saveKey = KeyCode.S;
    public KeyCode loadKey = KeyCode.L;


    List<Shape> shapes;

    #endregion


    private void Awake() {
        shapes = new List<Shape>();
    }

    private void Update() {
        if(Input.GetKeyDown(createKey)) {
            CreateShape();
        }
        else if(Input.GetKeyDown(newGameKey)) {
            BeginNewGame();
        }
        else if(Input.GetKeyDown(saveKey)) {
            storage.Save(this, saveVersion);
        }
        else if(Input.GetKeyDown(loadKey)) {
            BeginNewGame();
            storage.Load(this);
        }
    }

    void BeginNewGame() {
        for(int i = 0; i < shapes.Count; i++) {
            Destroy(shapes[i].gameObject);
        }
        shapes.Clear(); //gets rid of references to destroyed objects
    }

    void CreateShape() {
        Shape instance = shapeFactory.GetRandom();
        Transform t = instance.transform;
        t.localPosition = Random.insideUnitSphere * 5f;
        t.localRotation = Random.rotation;
        t.localScale = Vector3.one * Random.Range(0.5f, 1.5f);
        instance.SetColor(Random.ColorHSV(
            hueMin: 0f, hueMax:1f, 
            saturationMin: 0.5f, saturationMax: 1f, 
            valueMin: 0.25f, valueMax: 1f, 
            alphaMin: 1f, alphaMax: 1f));
        shapes.Add(instance);
    }

    public override void Save(GameDataWriter writer) {

        writer.Write(shapes.Count);
        for(int i = 0; i < shapes.Count; i++) {
            writer.Write(shapes[i].ShapeId); //writing shapeId to file before shape saves itself
            writer.Write(shapes[i].MaterialId);
            shapes[i].Save(writer);
        }
    }

    public override void Load(GameDataReader reader) {
        int vers = reader.Version;
        if(vers > saveVersion) {
            Debug.LogError("Unsupported future save version " + vers);
            return;
        }

        int count = vers <= 0 ? -vers : reader.ReadInt(); //checks if old file or new file
        for(int i = 0; i < count; i++) {
            int shapeId = vers > 0 ? reader.ReadInt() : 0; //if old file just read 0/get cubes
            int materialId = vers > 0 ? reader.ReadInt() : 0;
            Shape instance = shapeFactory.Get(shapeId, materialId);
            instance.Load(reader);
            shapes.Add(instance);
        }
    }
}
/*void Save() //old save; replaced by use of persistable obj's and storage
{
    //using (//etc){} -> ensures proper disposal of w/e writer uses; is shorthand for a common try/catch block
    using (
    var writer = new BinaryWriter(File.Open(savePath, FileMode.Create)) //creates binary stream to write data into
    ) {
        writer.Write(objects.Count);

        //store transformation data of each cube
        for (int i = 0; i < objects.Count; i++)
        {
            Transform t = objects[i];
            writer.Write(t.localPosition.x);
            writer.Write(t.localPosition.y);
            writer.Write(t.localPosition.z);
        }
    }
    Debug.Log("Saving data to... " + savePath);

}

void Load()
{
    BeginNewGame();
    using (
        var reader = new BinaryReader(File.Open(savePath, FileMode.Open))
        )
    {
        int count = reader.ReadInt32(); //being super explicit ab what we are reading in
        //readSingle -> floats
        for (int i = 0; i < count; i++)
        {
            Vector3 p;
            p.x = reader.ReadSingle();
            p.y = reader.ReadSingle();
            p.z = reader.ReadSingle();

            //instantiating the cubes that just read data for
            Transform t = Instantiate(prefab);
            t.localPosition = p;
            objects.Add(t);
        }
    }
}
*/


