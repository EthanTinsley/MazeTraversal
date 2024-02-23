

class DepthFirstSearchRobot : BaseMappingRobot {
    private Stack<Tuple<int, int>> stack = new Stack<Tuple<int, int>>();
    private HashSet<Tuple<int, int>> visited = new HashSet<Tuple<int, int>>();
    
    public DepthFirstSearchRobot(Func<bool> readSensorLeft, Func<bool> readSensorRight, Func<bool> readSensorForward, Func<bool> readSensorUp, Func<Action, bool> executeAnAction)
        : base(readSensorLeft, readSensorRight, readSensorForward, readSensorUp, executeAnAction)
    {
    }

    public override List<Tuple<int, int>> Escape() {
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
}