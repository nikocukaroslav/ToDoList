using ToDoList.Models.Domain;

namespace ToDoList.Models;

public class AddToDoRequest
{
    public string Id { get; set; }
    public string Task { get; set; }
    public DateTime? DateToPerform { get; set; }
    public bool IsPerformed { get; set; } = false;
    public string CategoryName { get; set; }
    public Guid CategoryId { get; set; }
}