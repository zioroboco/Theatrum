using Space;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Newtonian {

    public class Newton : JobComponentSystem {

        [BurstCompile]
        private struct Job : IJobProcessComponentData<Translation, Vectors> {
            public double dt;
            public double mu;
            public void Execute(
                ref Translation translation, ref Vectors state
            ) {
                var acceleration = mu / math.pow(state.Position.r, 2);

                var nextVelocity = state.Velocity + state.Position.PolarTransform(acceleration * dt);
                var nextPosition = state.Position.Update(nextVelocity * dt);

                state.Position = nextPosition;
                state.Velocity = nextVelocity;

                translation.Value = toFloat3(state.Position.ToWorldVector());
            }
        }

        private static float3 toFloat3(double2 v) {
            return new float3((float) v.x, (float) v.y, 0f);
        }

        protected override JobHandle OnUpdate(JobHandle inputDependencies) {
            var job = new Job {
                dt = Clock.Instance.DeltaTime,
                mu = 1d
            };
            return job.Schedule(this, inputDependencies);
        }
    }

}
