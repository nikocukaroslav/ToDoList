using System.Xml;
using ToDoList.Models.Domain;

namespace ToDoList.Data
{
    public class ToDoListXmlStrorage : IToDoListXmlStorage
    {
        public List<ToDo> LoadAllToDosXml(string path)
        {
            XmlDocument document = new XmlDocument();
            document.Load(path);
            XmlNodeList? nodes = document.SelectNodes("/database/todos/todo");

            List<ToDo> todos = new();
            if (nodes != null)
                foreach (XmlNode node in nodes)
                {
                    var id = node.SelectSingleNode("id")?.InnerText;
                    var task = node.SelectSingleNode("task")?.InnerText;
                    var categoryName = node.SelectSingleNode("categoryName")?.InnerText;
                    var isPerformed = node.SelectSingleNode("isPerformed")?.InnerText;
                    var dateToPerform = node.SelectSingleNode("dateToPerform")?.InnerText;
                    if (task != null && categoryName != null && isPerformed != null && id != null)
                    {
                        ToDo todo = new ToDo
                        {
                            Id = Guid.Parse(id),
                            Task = task,
                            CategoryName =categoryName,
                            IsPerformed = bool.Parse(isPerformed),
                            DateToPerform = !string.IsNullOrEmpty(dateToPerform)
                                ? DateTime.Parse(dateToPerform)
                                : (DateTime?)null,
                        };
                        todos.Add(todo);
                    }
                }
            return todos;
        }

        public List<Category> LoadAllCategories(string path)
        {
            XmlDocument document = new XmlDocument();
            document.Load(path);
            XmlNodeList? nodes = document.SelectNodes("/database/categories/category");

            List<Category> categories = new();

            if (nodes != null)
                foreach (XmlNode node in nodes)
                {
                    var name = node.SelectSingleNode("name")?.InnerText;

                    if (name != null)
                    {
                        Category category = new Category
                        {
                            Name = name,
                        };
                        categories.Add(category);
                    }
                }

            return categories;
        }

        public void SaveXml(string path, XmlDocument document)
        {
            document.Save(path);
        }
    }
}