using UnityEngine;

namespace PositionSystem.GridSystem
{
    public interface IGridModule
    {
        Vector3 GridPositionToWorld(int x, int y);
        (int x, int y) WorldPositionToGrid(Vector3 position);
    }
}