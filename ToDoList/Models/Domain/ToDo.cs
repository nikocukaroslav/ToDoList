using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models.Domain;

public class ToDo
{
    public Guid Id { get; set; }
    public string Task { get; set; }
    public bool IsPerformed { get; set; } = false;
    public DateTime? DateToPerform { get; set; }
    public Guid CategoryId { get; set; }
    public Category Category { get; set; }
}