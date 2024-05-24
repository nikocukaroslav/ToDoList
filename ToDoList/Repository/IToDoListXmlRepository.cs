using System.Xml;
using ToDoList.Models;
using ToDoList.Models.Domain;

namespace ToDoList.Data;

public interface IToDoListXmlRepository
{
    List<ToDo> LoadAllToDosXml(string path);
    List<Category> LoadAllCategories(string path);
    void AddToDo(AddToDoRequest addToDoRequest);
    void AddCategory(AddCategoryRequest addCategoryRequest);
    void PerformToDo(HandleTodoRequest handleTodoRequest);
    void UnperformToDo(HandleTodoRequest handleTodoRequest);
    void DeleteToDo(DeleteToDoRequest deleteToDoRequest);
    void SaveXml(string path, XmlDocument document);
}