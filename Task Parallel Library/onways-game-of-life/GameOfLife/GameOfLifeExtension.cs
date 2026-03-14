namespace GameOfLife;

/// <summary>
/// Provides extension methods for simulating Conway's Game of Life on different versions.
/// </summary>
public static class GameOfLifeExtension
{
    /// <summary>
    /// Simulates the evolution of Conway's Game of Life for a specified number of generations using the sequential version.
    /// The result is written to the provided <see cref="TextWriter"/> using the specified characters for alive and dead cells.
    /// </summary>
    /// <param name="game">The sequential version of the Game of Life.</param>
    /// <param name="generations">The number of generations to simulate.</param>
    /// <param name="writer">The <see cref="TextWriter"/> used to output the simulation result.</param>
    /// <param name="aliveCell">The character representing an alive cell.</param>
    /// <param name="deadCell">The character representing a dead cell.</param>
    /// <exception cref="ArgumentNullException">Thrown when <param name="game"/> parameters is null.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <param name="writer"/> parameters is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <param name="generations"/> is less than or equal to 0.</exception>
    public static void Simulate(this GameOfLifeSequentialVersion? game, int generations, TextWriter? writer, char aliveCell, char deadCell)
    {
        ArgumentNullException.ThrowIfNull(game);
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(generations);

        for (int i = 1; i <= generations; i++)
        {
            game.NextGeneration();
            writer.WriteLine($"Generation {i}");
            WriteGrid(writer, game.CurrentGeneration, aliveCell, deadCell);
        }
    }

    /// <summary>
    /// Asynchronously simulates the evolution of Conway's Game of Life for a specified number of generations using the parallel version.
    /// The result is written to the provided TextWriter using the specified characters for alive and dead cells.
    /// </summary>
    /// <param name="game">The parallel version of the Game of Life.</param>
    /// <param name="generations">The number of generations to simulate.</param>
    /// <param name="writer">The <see cref="TextWriter"/> used to output the simulation result.</param>
    /// <param name="aliveCell">The character representing an alive cell.</param>
    /// <param name="deadCell">The character representing a dead cell.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <param name="game"/> parameters is null.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <param name="writer"/> parameters is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <param name="generations"/> is less than or equal to 0.</exception>
    public static Task SimulateAsync(this GameOfLifeParallelVersion? game, int generations, TextWriter? writer, char aliveCell, char deadCell)
    {
        ArgumentNullException.ThrowIfNull(game);
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(generations);

        return SimulateInternalAsync(game, generations, writer, aliveCell, deadCell);
    }

    private static async Task SimulateInternalAsync(GameOfLifeParallelVersion game, int generations, TextWriter writer, char aliveCell, char deadCell)
    {
        for (int i = 1; i <= generations; i++)
        {
            game.NextGeneration();
            await writer.WriteLineAsync($"Generation {i}");
            await WriteGridAsync(writer, game.CurrentGeneration, aliveCell, deadCell);
        }
    }

    private static void WriteGrid(TextWriter writer, bool[,] grid, char aliveCell, char deadCell)
    {
        int rows = grid.GetLength(0);
        int columns = grid.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                writer.Write(grid[i, j] ? aliveCell : deadCell);
            }

            writer.WriteLine();
        }
    }

    private static async Task WriteGridAsync(TextWriter writer, bool[,] grid, char aliveCell, char deadCell)
    {
        int rows = grid.GetLength(0);
        int columns = grid.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                await writer.WriteAsync(grid[i, j] ? aliveCell : deadCell);
            }

            await writer.WriteLineAsync();
        }
    }
}
