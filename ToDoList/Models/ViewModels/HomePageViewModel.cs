using ToDoList.Models.Domain;

namespace ToDoList.Models.ViewModels;

public class HomePageViewModel
{ 
    public ToDo ToDo { get; set; }
    public Category Category { get; set; }
    public ChangeStorageRequest ChangeStorageRequest { get; set; }
    public List<ToDo>? ToDos { get; set; }
    public List<Category>? Categories { get; set; }
}