
using System;
using System.Runtime.CompilerServices;

public class Robot
{
    private Func<bool> ReadSensorLeft { get; }
    private Func<bool> ReadSensorRight { get; }
    private Func<bool> ReadSensorForward { get; }
    private Func<bool> ReadSensorUp { get; }
    private Func<Action, bool> ExecuteAnAction { get; }
    private Tuple<int, int> CurrentIndex => new Tuple<int, int>(x, y);
    private Stack<Tuple<int, int>> stack = new Stack<Tuple<int, int>>();
    private HashSet<Tuple<int, int>> visited = new HashSet<Tuple<int, int>>();
    private Dictionary<Tuple<int, int>, List<Tuple<int, int>>> previous = new Dictionary<Tuple<int, int>, List<Tuple<int, int>>>();
    private Direction Direction = Direction.Up;
    private int x = 0;
    private int y = 0;


    public Robot(Func<bool> readSensorLeft, Func<bool> readSensorRight, Func<bool> readSensorForward, Func<bool> readSensorUp, Func<Action, bool> executeAnAction)
    {
        ReadSensorLeft = readSensorLeft;
        ReadSensorRight = readSensorRight;
        ReadSensorForward = readSensorForward;
        ReadSensorUp = readSensorUp;
        ExecuteAnAction = executeAnAction;
    }

    public List<Tuple<int, int>> Escape() {
        var start = new Tuple<int, int>(x, y);
        stack.Push(start);
        previous[start] = new List<Tuple<int, int>> { start }; // Initialize the path for the start node
        visited.Add(start);

        while (stack.Count > 0) {
            var current = stack.Pop();
            MoveTo(current);

            if (!ReadSensorUp()) {
                // Found the exit, return the path
                return previous[current]; // Return the path from the start to the current node
            }

            foreach (var neighbor in GetNeighbors()) {
                if (!visited.Contains(neighbor)) {
                    stack.Push(neighbor);
                    visited.Add(neighbor);
                    // Add the current node to the path of the previous node to get the path to the neighbor
                    previous[neighbor] = new List<Tuple<int, int>>(previous[current]) { neighbor };
                }
            }
        }

        return new List<Tuple<int, int>>(); // No path to the exit
    }

    public void MoveTo(Tuple<int, int> destination) {
        int dx = destination.Item1 - x;
        int dy = destination.Item2 - y;
        // if we require multiple moves to get to the destination we will need to backtrack to the last node that the current path
        // and the destination path have in common
        if (Math.Abs(dx) + Math.Abs(dy) > 1) {
            BacktrackTo(destination);
            // re-evaulate the dx and dy
            dx = destination.Item1 - x;
            dy = destination.Item2 - y;
        }

        // Rotate the robot to face upwards for the next move
        while(Direction != Direction.Up) {
            ExecuteAction(Action.RotateRight);
        }

        if (dy == 1) {
            ExecuteAction(Action.RotateRight);
            ExecuteAction(Action.MoveForward);
        } else if (dy == -1) {
            ExecuteAction(Action.RotateLeft);
            ExecuteAction(Action.MoveForward);
        } else if (dx == 1) {
            ExecuteAction(Action.RotateRight);
            ExecuteAction(Action.RotateRight);
            ExecuteAction(Action.MoveForward);
        } else if (dx == -1) {
            ExecuteAction(Action.MoveForward);
        }

        if (!CurrentIndex.Equals(destination)) {
            throw new Exception("Failed to move to the destination");
        }
    }

    public void BacktrackTo(Tuple<int, int> destination) {
        var startIndex = CurrentIndex;
        var common = previous[destination].FindLast(node => previous[new Tuple<int, int>(x, y)].Contains(node));
        for(int i = previous[CurrentIndex].Count - 1; i >= 0; i--) {
            if (CurrentIndex.Equals(common)) {
                break;
            }
            MoveTo(previous[startIndex][i]);
        }
    }



    public List<Tuple<int, int>> GetNeighbors() {
        // Use the sensors and current direction to get nodes that are not walls
        // Will need to make one rotation to get the back of the robot and then rotate back to the original direction
        var neighbors = new List<Tuple<int, int>>();

        while(Direction != Direction.Up) {
            ExecuteAction(Action.RotateRight);
        }

        if (!ReadSensorLeft()) {
            neighbors.Add(new Tuple<int, int>(x, y-1));
        }

        if (!ReadSensorRight()) {
            neighbors.Add(new Tuple<int, int>(x, y+1));
        }

        if (!ReadSensorForward()) {
            neighbors.Add(new Tuple<int, int>(x-1, y));
        }
        
        // rotate 90 degrees to the right and check right sensor then roate 90 degrees to the left
        ExecuteAction(Action.RotateRight);
        if (!ReadSensorRight()) {
            neighbors.Add(new Tuple<int, int>(x+1, y));
        }
        ExecuteAction(Action.RotateLeft);

        return neighbors;
    }

    private void UpdateDirection(Action action) {
        switch (action) {
            case Action.RotateRight:
                switch (Direction) {
                    case Direction.Up:
                        Direction = Direction.Right;
                        break;
                    case Direction.Right:
                        Direction = Direction.Down;
                        break;
                    case Direction.Down:
                        Direction = Direction.Left;
                        break;
                    case Direction.Left:
                        Direction = Direction.Up;
                        break;
                }
                break;

            case Action.RotateLeft:
                switch (Direction) {
                    case Direction.Up:
                        Direction = Direction.Left;
                        break;
                    case Direction.Left:
                        Direction = Direction.Down;
                        break;
                    case Direction.Down:
                        Direction = Direction.Right;
                        break;
                    case Direction.Right:
                        Direction = Direction.Up;
                        break;
                }
                break;
        }
    }

    private void UpdateCoordinates(Action action) {
        if (action == Action.MoveForward) {
            switch (Direction) {
                case Direction.Up:
                    x--;
                    break;
                case Direction.Left:
                    y--;
                    break;
                case Direction.Right:
                    y++;
                    break;
                case Direction.Down:
                    x++;
                    break;
            }
        }
    }

    private void ExecuteAction(Action action) {
        if (ExecuteAnAction(action)) {
            UpdateDirection(action);
            UpdateCoordinates(action);
        }
    }
}
