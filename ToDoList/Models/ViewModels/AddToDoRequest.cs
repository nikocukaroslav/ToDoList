using ToDoList.Models.Domain;

namespace ToDoList.Models;

public class AddToDoRequest
{
    public string Task { get; set; }
    public DateTime? DateToPerform { get; set; }
    public Guid CategoryId { get; set; }
}