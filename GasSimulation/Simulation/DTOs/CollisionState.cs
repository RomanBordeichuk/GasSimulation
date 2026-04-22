using GasSimulation.Simulation.DTOs.Interfaces;

namespace GasSimulation.Simulation.DTOs
{
    internal struct CollisionState<T1, T2>
        where T1 : IElemState where T2 : IElemState
    {
        public T1 Obj1 { get; set; }
        public T2 Obj2 { get; set; }
        public int Id1 { get; set; }
        public int Id2 { get; set; }
        public double T { get; set; }
        public double Angle { get; set; }

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
