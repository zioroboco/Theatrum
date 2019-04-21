using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace Keplerian {

    public class Kepler : JobComponentSystem {

        [BurstCompile]
        private struct Job : IJobForEach<Translation, Elements> {
            public double dt;
            public double mu;
            public void Execute(
                ref Translation translation, ref Elements elements
            ) {
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
