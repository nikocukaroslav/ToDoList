using System.Xml;
using ToDoList.Models.Domain;

namespace ToDoList.Data;

public interface IToDoListXmlStorage
{
    List<ToDo> LoadAllToDosXml(string path);
    List<Category> LoadAllCategories(string path);
    void SaveXml(string path, XmlDocument document);
}