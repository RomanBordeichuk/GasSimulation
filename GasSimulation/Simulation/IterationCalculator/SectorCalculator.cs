using GasSimulation.Configuration;
using GasSimulation.Debuggers;
using GasSimulation.GeneralDTOs;
using GasSimulation.GeneralDTOs.Atom;
using GasSimulation.GeneralDTOs.Rect;
using GasSimulation.Logs;
using GasSimulation.Simulation.DTOs;
using GasSimulation.Simulation.IterationCalculator.DTOs;
using GasSimulation.Simulation.IterationCalculator.Helpers;

namespace GasSimulation.Simulation.IterationCalculator
{
    public class SectorCalculator
    {
        private readonly Config _config;
        private readonly SectorCalculatorVisualDebugger _debugger;

        public SectorCalculator(Config config,
            SectorCalculatorVisualDebugger debugger)
        {
            _config = config;
            _debugger = debugger;
        }

        public async ValueTask Calculate(AllStates allStates)
        {
            _debugger.ClearAll();
            _debugger.CreateAtoms(allStates.Atoms);
            _debugger.CreateRects(allStates.Rects);
            _debugger.CreateVectors(allStates);
            await _debugger.Stop();

            double remT = 1;

            while (true)
            {
                CollisionState<AtomState, AtomState> atomCollision = 
                    ClosestAtomCollisionHelper.Calculate(_config, ref allStates, remT);

                CollisionState<AtomState, RectState> rectCollision = 
                    ClosestRectCollisionHelper.Calculate(_config, ref allStates, remT);

                if (MathHelper.Equals(atomCollision.T, -1, _config.Simulation.ErrorRate) && 
                    MathHelper.Equals(rectCollision.T, -1, _config.Simulation.ErrorRate))
                {
                    MoveAll(allStates.Atoms, remT);

                    _debugger.ClearAtoms();
                    _debugger.ClearVectors();
                    _debugger.CreateAtoms(allStates.Atoms);
                    await _debugger.Stop();

                    break;
                }

                Logger.Log("Collision!!!");

                if (MathHelper.Equals(rectCollision.T, -1, _config.Simulation.ErrorRate) || 
                    (!MathHelper.Equals(atomCollision.T, -1, _config.Simulation.ErrorRate) && 
                    atomCollision.T < rectCollision.T))
                {
                    _debugger.ClearVectors();
                    _debugger.CreateVectors(allStates, atomCollision.T);
                    _debugger.CreateGhostAtom(atomCollision.Obj1, atomCollision.T);
                    _debugger.CreateGhostAtom(atomCollision.Obj2, atomCollision.T);
                    await _debugger.Stop();

                    MoveAll(allStates.Atoms, atomCollision.T);

                    _debugger.ClearGhostAtoms();
                    _debugger.ClearAtoms();
                    _debugger.ClearVectors();
                    _debugger.CreateAtoms(allStates.Atoms);
                    await _debugger.Stop();

                    (VelocityState newV1, VelocityState newV2) = CollisionCalculator.CalculateVelocities(
                        _config, atomCollision.Obj1, atomCollision.Obj2, atomCollision.Angle);

                    var newPos1 = Move(atomCollision.Obj1, atomCollision.T);
                    var newPos2 = Move(atomCollision.Obj2, atomCollision.T);

                    allStates.Atoms[atomCollision.Id1] = new AtomState(newPos1, newV1);
                    allStates.Atoms[atomCollision.Id2] = new AtomState(newPos2, newV2);

                    _debugger.ClearAtoms();
                    _debugger.CreateAtoms(allStates.Atoms);
                    _debugger.CreateVectors(allStates, remT - atomCollision.T);
                    await _debugger.Stop();

                    remT -= atomCollision.T;
                }
                else
                {
                    _debugger.ClearVectors();
                    _debugger.CreateVectors(allStates, rectCollision.T);
                    _debugger.CreateGhostAtom(rectCollision.Obj1, rectCollision.T);
                    await _debugger.Stop();

                    MoveAll(allStates.Atoms, rectCollision.T);

                    _debugger.ClearGhostAtoms();
                    _debugger.ClearAtoms();
                    _debugger.ClearVectors();
                    _debugger.CreateAtoms(allStates.Atoms);
                    await _debugger.Stop();

                    VelocityState newV = CollisionCalculator.CalculateVelocity(
                        rectCollision.Obj1, rectCollision.Angle);
                    var newPos = Move(rectCollision.Obj1, rectCollision.T);

                    allStates.Atoms[rectCollision.Id1] = new AtomState(newPos, newV);

                    _debugger.ClearAtoms();
                    _debugger.CreateAtoms(allStates.Atoms);
                    _debugger.CreateVectors(allStates, remT - rectCollision.T);
                    await _debugger.Stop();

                    remT -= rectCollision.T;
                }
            }
        }

        private static void MoveAll(List<AtomState> atoms, double t)
        {
            for (int i = 0; i < atoms.Count; i++)
            {
                atoms[i] = new (Move(atoms[i], t), atoms[i].Velocity);
            }
        }

        private static PosState Move(AtomState atom, double t)
        {
            PosState pos = atom.Pos;
            VelocityState v = atom.Velocity;

            double x = pos.X;
            double y = pos.Y;

            x += v.Dx * t;
            y += v.Dy * t;

            return new(x, y);
        }
    }
}
