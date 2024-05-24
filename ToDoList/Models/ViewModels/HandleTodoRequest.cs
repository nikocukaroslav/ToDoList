namespace ToDoList.Models;

public class HandleTodoRequest
{
    public Guid Id { get; set; }
    public bool IsPerformed { get; set; }
}