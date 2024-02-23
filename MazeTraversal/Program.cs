namespace MazeTraversal;

class Program
{
    static void Main(string[] args)
    {
        // Create a new maze as a 2D array
        int[,] maze = {
            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
            {1, 0, 0, 0 ,0, 0, 0, 0, 0 ,0 ,0, 1},
            {1, 0, 0, 0 ,0, 0, 0, 0, 0 ,0 ,0, 1},
            {1, 0, 0, 1, 1, 1, 1, 0, 1, 1, 0, 1},
            {1, 0, 0, 1, 1, 1, 1, 0, 1, 1, 1, 1},
            {1, 0, 0, 1, 1, 0, 1, 0, 0, 1, 1, 1},
            {1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1},
            {1, 0, 0, 1, 0, 0, 1, 1, 0, 0, 0, 1},
            {1, 0, 0, 1, 1, 0, 0, 1, 1, 1, 1, 1},
            {1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 1, 1},
            {1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1},
            {1, 0, 0, 1, 1, 0, 1, 1, 1, 1, 1, 1},
            {1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1},
            {1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1},
            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
        };

        // start index 
        var startIndex = new Tuple<int, int>(9, 9);

        // exit index
        var exitIndex = new Tuple<int, int>(7, 10);

        // Create a new Maze State 
        ProblemState state = new ProblemState(maze, startIndex, exitIndex, RobotType.BreadthFirstSearch, true);
        var path = state.RunSimulation();
        state.PrintPath(path);

        int[,] maze2 = {
            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
            {1, 0, 0, 0 ,0, 0, 0, 0, 0 ,0 ,0, 1},
            {1, 0, 0, 0 ,0, 0, 0, 0, 0 ,0 ,0, 1},
            {1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 0, 1},
            {1, 0, 0, 1, 1, 0, 1, 0, 0, 1, 1, 1},
            {1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 1, 1},
            {1, 0, 0, 1, 1, 0, 1, 1, 0, 0, 0, 1},
            {1, 0, 0, 0, 1, 0, 1, 1, 0, 0, 0, 1},
            {1, 0, 0, 1, 1, 0, 0, 0, 0, 0, 1, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 0, 0, 0, 1, 0, 1, 1, 1, 1},
            {1, 0, 0, 1, 1, 0, 1, 0, 1, 0, 0, 1},
            {1, 0, 0, 1, 1, 0, 1, 1, 1, 1, 0, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1},
            {1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 1},
            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
        };

        // start index 
        startIndex = new Tuple<int, int>(2, 3);

        // exit index
        exitIndex = new Tuple<int, int>(7, 10);

        // Create a new Maze State 
        state = new ProblemState(maze2, startIndex, exitIndex, RobotType.BreadthFirstSearch, false);
        path = state.RunSimulation();
        state.PrintPath(path);
    }
}
