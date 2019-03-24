using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class Newton : JobComponentSystem {

    [BurstCompile]
    struct Job : IJobProcessComponentData<Translation, Vectors> {
        public double dt;
        public double mu;
        public void Execute(
            ref Translation translation, ref Vectors state
        ) {
            state.Velocity += accelerationDueToGravity(state.Position, mu, dt);
            state.Position += state.Velocity * dt;
            translation.Value = toFloat3(state.Position);
        }
    }

    static double2 fromFloat3(float3 v) {
        return new double2(v.x, v.y);
    }

    static float3 toFloat3(double2 v) {
        return new float3((float) v.x, (float) v.y, 0f);
    }

    static double2 accelerationDueToGravity(double2 r, double mu, double dt) {
        return (mu / math.dot(r, r)) * math.normalize(-r) * dt;
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies) {
        var job = new Job() {
            dt = Clock.Instance.DeltaTime,
            mu = 1d
        };
        return job.Schedule(this, inputDependencies);
    }
}
