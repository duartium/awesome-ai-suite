using CharacterUniverse.Core.Models;
using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace CharacterUniverse.Infraestructure.Prompts.TodoListAgent.Plugins;

internal class TodoListPlugin
{
	private static List<TodoItem>  myTodoList = new()
	{
		new TodoItem { Task = "Complete coding exercise", Completed = false },
		new TodoItem { Task = "Practice Mandarin", Completed = false },
		new TodoItem { Task = "Buy groceries", Completed = false }
	};

	[KernelFunction, Description("Mark a todo list item as complete")]
	public static string CompleteTask(
		[Description("The task to complete")] string task)
	{
		foreach (var taskItem in myTodoList)
		{
			if (taskItem.Task == task)
			{
				taskItem.Completed = true;
				break;
			}
		}

		return $"Task '{task}' marked as complete.";
	}
}
