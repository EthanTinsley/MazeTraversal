# Maze Traversal

## Description
This project implements a maze traversal algorithm using Depth-First Search (DFS) with backtracking. The algorithm is designed to help a robot navigate through a maze and find the exit.

## Problem State
The [ProblemState](/MazeTraversal/State.cs) class is responsible for monitoring the robot's position within the maze. It provides sensor and actuator functions to the robot, allowing it to gather information about its surroundings and make decisions based on that information.

## Robot's Escape Strategy
The robot's escape strategy is based on the DFS algorithm with backtracking. It explores the maze by traversing as far as possible in one direction before backtracking and exploring other paths. This strategy ensures that the robot exhaustively searches the maze and eventually finds the exit.

## Problem uniqueness
The uniqueness of the problem is that the robot does not actually have access to the maze. That is, the robot 
interacts with the world via 3 sensors and an actuator. It must track its movements in order to construct 
indices and paths. This is different from a basic traversal where the maze indices are known from the robot.


## Usage
To use this project, follow these steps:

1. Clone the repository: `git clone https://github.com/your-username/maze-traversal.git`
2. Navigate to the project directory: `cd maze-traversal`
3. Compile the code: `dotnet build`
4. Run the program: `dotnet run`

## License
This project is licensed under the [MIT License](LICENSE).


