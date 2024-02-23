
public class ProblemState {

    public int[,] Maze { get;}
    public Tuple<int, int> RobotStartIndex {get;}
    public Tuple<int, int> ExitIndex {get;}
    public Tuple<int, int> CurrentRobotIndex {get; set;}
    public Direction CurrentRobotDirection {get; set;}
    public Robot Robot {get; set;}
    public bool Verbose {get; set;}


    public ProblemState(int[,] maze, Tuple<int, int> robotStartIndex, Tuple<int, int> exitIndex, bool verbose = false) {
        Maze = maze;
        RobotStartIndex = robotStartIndex;
        CurrentRobotIndex = robotStartIndex;
        CurrentRobotDirection = Direction.Up;
        ExitIndex = exitIndex;

        Robot = new Robot(
            () => ReadSensorLeft(),
            () => ReadSensorRight(),
            () => ReadSensorForward(),
            () => ReadSensorUp(),
            (action) => ExecuteAction(action)
        );
        Verbose = verbose;
    }

    public bool ReadSensorLeft() {
        switch (CurrentRobotDirection) {
            case Direction.Up:
                return Maze[CurrentRobotIndex.Item1, CurrentRobotIndex.Item2-1] != 0;
            case Direction.Left:
                return Maze[CurrentRobotIndex.Item1+1, CurrentRobotIndex.Item2] != 0;
            case Direction.Right:
                return Maze[CurrentRobotIndex.Item1-1, CurrentRobotIndex.Item2] != 0;
            case Direction.Down:
                return Maze[CurrentRobotIndex.Item1, CurrentRobotIndex.Item2+1] != 0;
        }
        return false;
    }

    public bool ReadSensorRight() {
        switch (CurrentRobotDirection) {
            case Direction.Up:
                return Maze[CurrentRobotIndex.Item1, CurrentRobotIndex.Item2+1] != 0;
            case Direction.Left:
                return Maze[CurrentRobotIndex.Item1-1, CurrentRobotIndex.Item2] != 0;
            case Direction.Right:
                return Maze[CurrentRobotIndex.Item1+1, CurrentRobotIndex.Item2] != 0;
            case Direction.Down:
                return Maze[CurrentRobotIndex.Item1, CurrentRobotIndex.Item2-1] != 0;
        }
        return false;
    }

    public bool ReadSensorForward() {
        switch (CurrentRobotDirection) {
            case Direction.Up:
                return Maze[CurrentRobotIndex.Item1-1, CurrentRobotIndex.Item2] != 0;
            case Direction.Left:
                return Maze[CurrentRobotIndex.Item1, CurrentRobotIndex.Item2-1] != 0;
            case Direction.Right:
                return Maze[CurrentRobotIndex.Item1, CurrentRobotIndex.Item2+1] != 0;
            case Direction.Down:
                return Maze[CurrentRobotIndex.Item1+1, CurrentRobotIndex.Item2] != 0;
        }
        return false;
    }

    public bool ReadSensorUp() {
        // chcek if the robot is at the exit
        return !CurrentRobotIndex.Equals(ExitIndex);
    }

    public bool ExecuteAction(Action action) {
        if (Verbose && action == Action.MoveForward) {
            PrintState();
        }

        switch (action) {
            case Action.RotateRight:
                switch (CurrentRobotDirection) {
                    case Direction.Up:
                        CurrentRobotDirection = Direction.Right;
                        return true;
                    case Direction.Left:
                        CurrentRobotDirection = Direction.Up;
                        return true;
                    case Direction.Right:
                        CurrentRobotDirection = Direction.Down;
                        return true;
                    case Direction.Down:
                        CurrentRobotDirection = Direction.Left;
                        return true;
                }
                break;
            case Action.RotateLeft:
                switch (CurrentRobotDirection) {
                    case Direction.Up:
                        CurrentRobotDirection = Direction.Left;
                        return true;
                    case Direction.Left:
                        CurrentRobotDirection = Direction.Down;
                        return true;
                    case Direction.Right:
                        CurrentRobotDirection = Direction.Up;
                        return true;
                    case Direction.Down:
                        CurrentRobotDirection = Direction.Right;
                        return true;
                }
                break;
            case Action.MoveForward:
                switch (CurrentRobotDirection) {
                    case Direction.Up:
                        if (Maze[CurrentRobotIndex.Item1-1, CurrentRobotIndex.Item2] == 0) {
                            CurrentRobotIndex = new Tuple<int, int>(CurrentRobotIndex.Item1-1, CurrentRobotIndex.Item2);
                            return true;
                        }
                        break;
                    case Direction.Left:
                        if (Maze[CurrentRobotIndex.Item1, CurrentRobotIndex.Item2-1] == 0) {
                            CurrentRobotIndex = new Tuple<int, int>(CurrentRobotIndex.Item1, CurrentRobotIndex.Item2-1);
                            return true;
                        }
                        break;
                    case Direction.Right:
                        if (Maze[CurrentRobotIndex.Item1, CurrentRobotIndex.Item2+1] == 0) {
                            CurrentRobotIndex = new Tuple<int, int>(CurrentRobotIndex.Item1, CurrentRobotIndex.Item2+1);
                            return true;
                        }
                        break;
                    case Direction.Down:
                        if (Maze[CurrentRobotIndex.Item1+1, CurrentRobotIndex.Item2] == 0) {
                            CurrentRobotIndex = new Tuple<int, int>(CurrentRobotIndex.Item1+1, CurrentRobotIndex.Item2);
                            return true;
                        }
                        break;
                }
                break;
        }

        return false;
    }

    public void PrintState() {
        Console.WriteLine("--------------------");
        for (int i = 0; i < Maze.GetLength(0); i++) {
            for (int j = 0; j < Maze.GetLength(1); j++) {
                if (CurrentRobotIndex.Item1 == i && CurrentRobotIndex.Item2 == j) {
                    Console.ForegroundColor = ConsoleColor.Red;
                    switch (CurrentRobotDirection) {
                        case Direction.Up:
                            Console.Write("^");
                            break;
                        case Direction.Left:
                            Console.Write("<");
                            break;
                        case Direction.Right:
                            Console.Write(">");
                            break;
                        case Direction.Down:
                            Console.Write("v");
                            break;
                    }
                    Console.ResetColor();
                } else if (ExitIndex.Item1 == i && ExitIndex.Item2 == j) {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("E");
                    Console.ResetColor();
                } else if (Maze[i, j] == 1) {
                    Console.Write("X");
                } else {
                    Console.Write(" ");
                }
            }
            Console.WriteLine();
        }
        Console.WriteLine("--------------------");
    }

    public void PrintPath(List<Tuple<int, int>> path) {
        Console.WriteLine("--------------------");
        for (int i = 0; i < Maze.GetLength(0); i++) {
            for (int j = 0; j < Maze.GetLength(1); j++) {
                if (path.Contains(new Tuple<int, int>(i, j))) {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("P");
                    Console.ResetColor();
                } else if (ExitIndex.Item1 == i && ExitIndex.Item2 == j) {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("E");
                    Console.ResetColor();
                } else if (Maze[i, j] == 1) {
                    Console.Write("X");
                } else {
                    Console.Write(" ");
                }
            }
            Console.WriteLine();
        }
        Console.WriteLine("--------------------");
    }

    public List<Tuple<int,int>> RunSimulation() {
        var path = Robot.Escape();
        var normalizedPath = NormalizePath(RobotStartIndex, path);

        if (Verbose) {
            PrintPath(normalizedPath);
        }
        return normalizedPath;
    }

    public static List<Tuple<int, int>> NormalizePath(Tuple<int, int> normalIndex, List<Tuple<int, int>> path) {
        var normalizedPath = new List<Tuple<int, int>>();
        foreach(var node in path) {
            // need to calculate the relative position of the node to the normal index
            // at node (0,0) should equal the normal index
            var x = normalIndex.Item1 + node.Item1;
            var y = normalIndex.Item2 + node.Item2;
            normalizedPath.Add(new Tuple<int, int>(x, y));
        }
        return normalizedPath;
    }

}