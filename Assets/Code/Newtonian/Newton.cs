using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace Newtonian {

    public class Newton : JobComponentSystem {

        [BurstCompile]
        private struct Job : IJobForEach<Translation, Vectors> {
            public double dt;
            public double mu;
            public void Execute(
                ref Translation translation, ref Vectors state
            ) {
                var acceleration = new double2(0d, mu / math.pow(state.Position.r, 2d));
                var velocity = state.Velocity - acceleration * dt;

                var lastPosition = state.Position;
                var nextPosition = lastPosition.Update(velocity * dt);

                state.Position = nextPosition;
                state.Velocity = nextPosition.PolarTransform(lastPosition.WorldTransform(velocity));

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
