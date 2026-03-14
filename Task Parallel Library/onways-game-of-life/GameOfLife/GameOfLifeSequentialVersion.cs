namespace GameOfLife;

/// <summary>
/// Represents Conway's Game of Life in a sequential version.
/// The class provides methods to simulate the game's evolution based on simple rules.
/// </summary>
public sealed class GameOfLifeSequentialVersion
{
    private readonly bool[,] initialGrid;
    private readonly int rows;
    private readonly int columns;
    private bool[,] currentGrid;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameOfLifeSequentialVersion"/> class with the specified number of rows and columns. The initial state of the grid is randomly set with alive or dead cells.
    /// </summary>
    /// <param name="rows">The number of rows in the grid.</param>
    /// <param name="columns">The number of columns in the grid.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the number of rows or columns is less than or equal to 0.</exception>
    public GameOfLifeSequentialVersion(int rows, int columns)
    {
        if (rows <= 0 || columns <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(rows), "Rows and columns must be greater than 0.");
        }

        this.rows = rows;
        this.columns = columns;
        this.initialGrid = new bool[rows, columns];
        this.currentGrid = new bool[rows, columns];

        Random random = new Random();
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                this.initialGrid[i, j] = random.Next(2) == 1;
                this.currentGrid[i, j] = this.initialGrid[i, j];
            }
        }

        this.Generation = 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GameOfLifeSequentialVersion"/> class with the given grid.
    /// </summary>
    /// <param name="grid">The 2D array representing the initial state of the grid.</param>
    /// <exception cref="ArgumentNullException">Thrown when the input grid is null.</exception>
    public GameOfLifeSequentialVersion(bool[,] grid)
    {
        ArgumentNullException.ThrowIfNull(grid);

        this.rows = grid.GetLength(0);
        this.columns = grid.GetLength(1);
        this.initialGrid = (bool[,])grid.Clone();
        this.currentGrid = (bool[,])grid.Clone();
        this.Generation = 0;
    }

    /// <summary>
    /// Gets a copy of the current generation grid.
    /// </summary>
    /// <returns>A separate copy of the current generation grid.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability", "S2365:Properties should not copy collections", Justification = "Required by README specification and automated tests.")]
    public bool[,] CurrentGeneration => (bool[,])this.currentGrid.Clone();

    /// <summary>
    /// Gets the current generation number.
    /// </summary>
    public int Generation { get; private set; }

    /// <summary>
    /// Restarts the game by resetting the current grid to the initial state.
    /// </summary>
    public void Restart()
    {
        this.currentGrid = (bool[,])this.initialGrid.Clone();
        this.Generation = 0;
    }

    /// <summary>
    /// Advances the game to the next generation based on the rules of Conway's Game of Life.
    /// </summary>
    public void NextGeneration()
    {
        bool[,] nextGrid = new bool[this.rows, this.columns];

        for (int i = 0; i < this.rows; i++)
        {
            for (int j = 0; j < this.columns; j++)
            {
                int aliveNeighbors = this.CountAliveNeighbors(i, j);
                bool isAlive = this.currentGrid[i, j];

                if (isAlive)
                {
                    nextGrid[i, j] = aliveNeighbors == 2 || aliveNeighbors == 3;
                }
                else
                {
                    nextGrid[i, j] = aliveNeighbors == 3;
                }
            }
        }

        this.currentGrid = nextGrid;
        this.Generation++;
    }

    /// <summary>
    /// Counts the number of alive neighbors for a given cell in the grid.
    /// </summary>
    /// <param name="row">The row index of the cell.</param>
    /// <param name="column">The column index of the cell.</param>
    /// <returns>The number of alive neighbors for the specified cell.</returns>
    private int CountAliveNeighbors(int row, int column)
    {
        int count = 0;
        for (int i = row - 1; i <= row + 1; i++)
        {
            for (int j = column - 1; j <= column + 1; j++)
            {
                if (i == row && j == column)
                {
                    continue;
                }

                if (i >= 0 && i < this.rows && j >= 0 && j < this.columns && this.currentGrid[i, j])
                {
                    count++;
                }
            }
        }

        return count;
    }
}
