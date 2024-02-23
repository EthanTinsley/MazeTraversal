# MazeTraversal Project Structure
The MazeTraversal project is structured as follows:

- MazeTraversal/: This is the root directory of the project.
-- ProblemState.cs: This class represents the state of the problem at any given point. It includes information about the current state of the maze, the position of the robot, and other relevant data.
-- Program.cs: This is the main entry point of the application. When you run this file, you will see visualizations of two implementations of the Depth-First Search (DFS) algorithm.
-- Robots/: This directory contains interfaces and classes related to the robot that is traversing the maze.
--- IRobot.cs: This interface defines the basic functionality that all robots must have, such as moving and reading sensors.
--- BaseRobot.cs: This abstract class provides a basic implementation of the IRobot interface. It can be extended by other classes to create robots with more specific behavior.
--- MappingRobot.cs: This class extends BaseRobot and adds functionality for creating a map of the maze as the robot traverses it.
--- (other robot classes): You can add more classes in this directory to create robots with different behaviors.
To run the program and see the DFS visualizations, you can use the following command in the terminal:

```dotnet run```

This command will compile and run the program. You should see the DFS visualizations in the console output.
