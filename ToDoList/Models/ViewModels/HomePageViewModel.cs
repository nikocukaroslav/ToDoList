using ToDoList.Models.Domain;

namespace ToDoList.Models;

public class HomePageViewModel
{
    public AddToDoRequest AddToDoRequest { get; set; }
    public DeleteToDoRequest DeleteToDoRequest { get; set; }
    public List<ToDo> ToDos { get; set; }
    public List<Category> Categories { get; set; }
}