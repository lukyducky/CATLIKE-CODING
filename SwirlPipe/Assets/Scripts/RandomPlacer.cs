using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPlacer : PipeItemGenerator {

    public PipeItem[] itemPrefabs;

    public override void GenerateItems(Pipe pipe){
        float angleStep = pipe.CurveAngle / pipe.CurveSegCount;
        for (int i = 0; i < pipe.CurveSegCount; i+=2){
            PipeItem item = Instantiate<PipeItem>(itemPrefabs[Random.Range(0, itemPrefabs.Length)]);
            float pipeRot = (Random.Range(0, pipe.pipeSegCount) + 0.5f) * 360f / pipe.pipeSegCount;
            item.Position(pipe, i * angleStep, pipeRot);
        }
    }
}
