using GasSimulation.GeneralDTOs.Interfaces;

namespace GasSimulation.GeneralDTOs
{
    public struct VectorState : IElemState
    {
        public double X1 { get; }
        public double Y1 { get; }
        public double X2 { get; }
        public double Y2 { get; }
        public double Thickness { get; }

        public VectorState (double x1, double y1, double x2, double y2, double thickness)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
            Thickness = thickness;
        }

        public VectorState (PosState pos1, PosState pos2, double thickness)
        {
            X1 = pos1.X; 
            Y1 = pos1.Y;
            X2 = pos2.X;
            Y2 = pos2.Y;
            Thickness = thickness;
        }
    }
}
