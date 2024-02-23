

public abstract class BaseRobot : IRobot {

    public Func<bool> ReadSensorLeft { get; }
    public Func<bool> ReadSensorRight { get; }
    public Func<bool> ReadSensorForward { get; }
    public Func<bool> ReadSensorUp { get; }

    /// <summary>
    /// A function that executes an action.
    /// The action can be to rotate the robot to the left, right, or to move forward.
    /// This affects the state of the maze and the robot's position within the maze.
    /// </summary>
    /// <returns>
    /// True if the action was executed successfully, false otherwise.
    /// </returns>
    private Func<Action, bool> ExecuteAnAction { get; }


    public BaseRobot(Func<bool> readSensorLeft, Func<bool> readSensorRight, Func<bool> readSensorForward, Func<bool> readSensorUp, Func<Action, bool> executeAnAction)
    {
        ReadSensorLeft = readSensorLeft;
        ReadSensorRight = readSensorRight;
        ReadSensorForward = readSensorForward;
        ReadSensorUp = readSensorUp;
        ExecuteAnAction = executeAnAction;
    }

    public abstract List<Tuple<int, int>> Escape();

    /// <summary>
    /// Executes the specified action and updates the state of the robot and the maze.
    /// </summary>
    /// <param name="action"></param>
    protected virtual bool PerformAction(Action action) {
        return ExecuteAnAction(action);
    }
}

/// <summary>
/// Basic Robot that adds mapping functionality to the base robot
/// </summary>
public abstract class BaseMappingRobot : BaseRobot {
    protected Direction Direction {get; set;}
    protected int x {get; set;}
    protected int y {get; set;}
    protected Dictionary<Tuple<int, int>, List<Tuple<int, int>>> previous {get; set;}
    protected Tuple<int, int> CurrentIndex => new Tuple<int, int>(x, y);


    public BaseMappingRobot(Func<bool> readSensorLeft, Func<bool> readSensorRight, Func<bool> readSensorForward, Func<bool> readSensorUp, Func<Action, bool> executeAnAction)
        : base(readSensorLeft, readSensorRight, readSensorForward, readSensorUp, executeAnAction)
    {
        Direction = Direction.Up;
        x = 0;
        y = 0;
        previous = new Dictionary<Tuple<int, int>, List<Tuple<int, int>>>();
    }

    public abstract override List<Tuple<int, int>> Escape();

    /// <summary>
    /// Moves the robot to the specified destination.
    /// </summary>
    /// <param name="destination">A tuple representing the destination coordinates (x, y) on the grid.</param>
    /// <exception cref="System.Exception">Throws an exception if the robot fails to move to the destination.</exception>
    protected void MoveTo(Tuple<int, int> destination) {
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
            PerformAction(Action.RotateRight);
        }

        // Depending on the difference in x and y coordinates, rotate the robot and move forward
        if (dy == 1) {
            PerformAction(Action.RotateRight);
            PerformAction(Action.MoveForward);
        } else if (dy == -1) {
            PerformAction(Action.RotateLeft);
            PerformAction(Action.MoveForward);
        } else if (dx == 1) {
            PerformAction(Action.RotateRight);
            PerformAction(Action.RotateRight);
            PerformAction(Action.MoveForward);
        } else if (dx == -1) {
            PerformAction(Action.MoveForward);
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
    protected void BacktrackTo(Tuple<int, int> destination) {
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


    /// <summary>
    /// Uses the sensors and actuators to get the neighbors of the current node
    /// </summary>
    /// <returns></returns>
    protected List<Tuple<int, int>> GetNeighbors() {
        // Use the sensors and current direction to get nodes that are not walls
        // Will need to make one rotation to get the back of the robot and then rotate back to the original direction
        var neighbors = new List<Tuple<int, int>>();

        // rotate until we are facing up
        // this is to make sure we are facing the right direction to check the sensors
        // inrelation to the managed index
        while(Direction != Direction.Up) {
            PerformAction(Action.RotateRight);
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
        PerformAction(Action.RotateRight);
        if (!ReadSensorRight()) {
            neighbors.Add(new Tuple<int, int>(x+1, y));
        }
        PerformAction(Action.RotateLeft);

        return neighbors;
    }

    /// <summary>
    /// Updates the direction based on the current direction and action.
    /// </summary>
    /// <param name="action"></param>
    protected void UpdateDirection(Action action) {
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

    /// <summary>
    /// Updates the x and y coordinates based on the current direction and action.
    /// </summary>
    /// <param name="action"></param>
    protected void UpdateCoordinates(Action action) {
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

    protected override bool PerformAction(Action action) {
        var result = base.PerformAction(action);
        if (result) {
            UpdateDirection(action);
            UpdateCoordinates(action);
        }
        return result;
    }
}