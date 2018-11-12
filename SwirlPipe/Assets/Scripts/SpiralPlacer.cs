using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralPlacer : PipeItemGenerator {
    public PipeItem[] itemPrefabs;

    public override void GenerateItems(Pipe pipe){
        float start = (Random.Range(0, pipe.pipeSegCount) + 0.5f);
        float direction = Random.value < 0.5f ? 1f : -1f; //randomly choses clockwise or ccw

        float angleStep = pipe.CurveAngle / pipe.CurveSegCount;
        for (int i = 0; i< pipe.CurveSegCount; i+=2){
            PipeItem item = Instantiate<PipeItem>(itemPrefabs[Random.Range(0, itemPrefabs.Length)]);
            float pipeRotation = (start + i * direction) * 360f / pipe.pipeSegCount;
            item.Position(pipe, i * angleStep, pipeRotation);
        }
    }
}
