using ToDoList.Models.Domain;

namespace ToDoList.Repository;

public interface IToDoListRepository
{
    List<ToDo> GetAllToDos();
    List<Category> GetAllCategories();
    Category AddCategory(Category category);
    ToDo AddToDo(ToDo todo);
    ToDo HandlePerformed(ToDo todo);
    void DeleteToDo(ToDo todo);
}