using GasSimulation.GeneralDTOs.Interfaces;

namespace GasSimulation.Simulation.IterationCalculator.DTOs
{
    public struct CollisionState<T1, T2>
        where T1 : IElemState where T2 : IElemState
    {
        public T1 Obj1 { get; }
        public T2 Obj2 { get; }
        public int Id1 { get; }
        public int Id2 { get; }
        public double T { get; }
        public double Angle { get; }

        public CollisionState(
            int id1, int id2, T1 obj1, T2 obj2, double t, double angle)
            : this(obj1, obj2, t, angle)
        {
            Id1 = id1;
            Id2 = id2;
        }

        public CollisionState(T1 obj1, T2 obj2, double t, double angle)
        {
            Obj1 = obj1; 
            Obj2 = obj2;
            T = t;
            Angle = angle;
        }
    }
}
