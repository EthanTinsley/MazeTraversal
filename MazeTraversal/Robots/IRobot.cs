

public interface IRobot
{

    /// <summary>
    /// A function that reads the sensor in the left direction.
    /// </summary>
    /// <returns>
    /// True if the sensor detects a wall, false otherwise.
    /// </returns>
    public Func<bool> ReadSensorLeft { get; }
    
    /// <summary>
    /// A function that reads the sensor in the right direction.
    /// </summary>
    /// <returns>
    /// True if the sensor detects a wall, false otherwise.
    /// </returns>
    public Func<bool> ReadSensorRight { get; }
    
    /// <summary>
    /// A function that reads the sensor in the forward direction.
    /// </summary>
    /// <returns>
    /// True if the sensor detects a wall, false otherwise.
    /// </returns>
    public Func<bool> ReadSensorForward { get; }

    /// <summary>
    /// A function that reads the sensor in the up direction.
    /// </summary>
    /// <returns> 
    /// True if the sensor detects a wall, false otherwise.
    /// </returns>
    public Func<bool> ReadSensorUp { get; }

    /// <summary>
    /// Tries to escape from the current position by finding a path to the exit.
    /// </summary>
    /// <returns>
    /// A list of tuples representing the path to the exit. Each tuple represents a coordinate (x, y) on the grid.
    /// If no path to the exit is found, returns an empty list.
    /// </returns>
    public List<Tuple<int, int>> Escape();
}