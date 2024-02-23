

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
        // Initialize the start position
        var start = new Tuple<int, int>(x, y);
        
        // Push the start position to the stack
        stack.Push(start);
        
        // Initialize the path for the start node
        previous[start] = new List<Tuple<int, int>> { start };
        
        // Add the start position to the visited set
        visited.Add(start);

        // While there are still nodes to visit
        while (stack.Count > 0) {
            // Pop the current node from the stack
            var current = stack.Pop();
            
            // Move to the current node
            MoveTo(current);

            // If the exit is found
            if (!ReadSensorUp()) {
                // Return the path from the start to the current node
                return previous[current];
            }

            // For each neighbor of the current node
            foreach (var neighbor in GetNeighbors()) {
                // If the neighbor has not been visited yet
                if (!visited.Contains(neighbor)) {
                    // Push the neighbor to the stack
                    stack.Push(neighbor);
                    
                    // Add the neighbor to the visited set
                    visited.Add(neighbor);
                    
                    // Add the current node to the path of the previous node to get the path to the neighbor
                    previous[neighbor] = new List<Tuple<int, int>>(previous[current]) { neighbor };
                }
            }
        }

        // If no path to the exit is found, return an empty list
        return new List<Tuple<int, int>>();
    }

    /// <summary>
    /// Moves the robot to the specified destination.
    /// </summary>
    /// <param name="destination">A tuple representing the destination coordinates (x, y) on the grid.</param>
    /// <exception cref="System.Exception">Throws an exception if the robot fails to move to the destination.</exception>
    public void MoveTo(Tuple<int, int> destination) {
        // Calculate the difference in x and y coordinates between the current position and the destination
        int dx = destination.Item1 - x;
        int dy = destination.Item2 - y;

        // If the destination is not adjacent to the current position
        if (Math.Abs(dx) + Math.Abs(dy) > 1) {
            // Backtrack to the last node that the current path and the destination path have in common
            BacktrackTo(destination);

            // Re-evaluate the difference in x and y coordinates
            dx = destination.Item1 - x;
            dy = destination.Item2 - y;
        }

        // Rotate the robot to face upwards for the next move
        while(Direction != Direction.Up) {
            ExecuteAction(Action.RotateRight);
        }

        // Depending on the difference in x and y coordinates, rotate the robot and move forward
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

        // If the robot did not reach the destination, throw an exception
        if (!CurrentIndex.Equals(destination)) {
            throw new Exception("Failed to move to the destination");
        }
    }

    /// <summary>
    /// Backtracks the robot to the specified destination.
    /// </summary>
    /// <param name="destination">A tuple representing the destination coordinates (x, y) on the grid.</param>
    public void BacktrackTo(Tuple<int, int> destination) {
        // Store the current position
        var startIndex = CurrentIndex;

        // Find the last common node between the current path and the destination path
        var common = previous[destination].FindLast(node => previous[new Tuple<int, int>(x, y)].Contains(node));

        // Start from the end of the current path
        for(int i = previous[CurrentIndex].Count - 1; i >= 0; i--) {
            // If the current node is the common node, stop backtracking
            if (CurrentIndex.Equals(common)) {
                break;
            }

            // Move to the next node in the current path
            MoveTo(previous[startIndex][i]);
        }
    }


    public List<Tuple<int, int>> GetNeighbors() {
        // Use the sensors and current direction to get nodes that are not walls
        // Will need to make one rotation to get the back of the robot and then rotate back to the original direction
        var neighbors = new List<Tuple<int, int>>();

        // rotate until we are facing up
        // this is to make sure we are facing the right direction to check the sensors
        // inrelation to the managed index
        while(Direction != Direction.Up) {
            ExecuteAction(Action.RotateRight);
        }

        // check the sensors and add the neighbors to the list
        if (!ReadSensorLeft()) {
            neighbors.Add(new Tuple<int, int>(x, y-1));
        }

        if (!ReadSensorRight()) {
            neighbors.Add(new Tuple<int, int>(x, y+1));
        }

        if (!ReadSensorForward()) {
            neighbors.Add(new Tuple<int, int>(x-1, y));
        }
        
        // check behind the robot
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
