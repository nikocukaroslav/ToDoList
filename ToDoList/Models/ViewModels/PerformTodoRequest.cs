namespace ToDoList.Models;

public class PerformTodoRequest
{
    public Guid Id { get; set; }
    public bool IsPerformed { get; set; }
}