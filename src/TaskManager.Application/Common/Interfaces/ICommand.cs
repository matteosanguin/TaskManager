namespace TaskManager.Application.Common.Interfaces;

/// <summary>
/// Marker interface for commands
/// </summary>
public interface ICommand { }

/// <summary>
/// Interface for commands that return a result
/// </summary>
/// <typeparam name="TResult">The type of result returned by the command</typeparam>
public interface ICommand<out TResult> : ICommand { }
