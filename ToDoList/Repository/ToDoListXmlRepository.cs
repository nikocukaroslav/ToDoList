using System.Xml;
using ToDoList.Data;
using ToDoList.Models.Domain;

namespace ToDoList.Repository
{
    public class ToDoListXmlRepository : IToDoListRepository
    {
        private readonly XmlStorageContext _xmlStorageContext;

        public ToDoListXmlRepository(XmlStorageContext xmlStorageContext)
        {
            this._xmlStorageContext = xmlStorageContext;
        }

        public List<ToDo> GetAllToDos()
        {
            XmlDocument document = new XmlDocument();
            document.Load(_xmlStorageContext.XmlStoragePath);
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
                            CategoryName = categoryName,
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

        public List<Category> GetAllCategories()
        {
            XmlDocument document = new XmlDocument();
            document.Load(_xmlStorageContext.XmlStoragePath);
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

        public ToDo AddToDo(ToDo todo)
        {
            XmlDocument document = new XmlDocument();

            document.Load(_xmlStorageContext.XmlStoragePath);

            XmlElement root = (XmlElement)document.SelectSingleNode("/database/todos")!;

            if (root != null)
            {
                XmlElement newToDo = document.CreateElement("todo");
                XmlElement id = document.CreateElement("id");
                XmlElement task = document.CreateElement("task");
                XmlElement isPerformed = document.CreateElement("isPerformed");
                XmlElement categoryName = document.CreateElement("categoryName");

                id.InnerText = todo.Id.ToString();
                task.InnerText = todo.Task;
                isPerformed.InnerText = todo.IsPerformed.ToString();
                categoryName.InnerText = todo.CategoryName;

                newToDo.AppendChild(id);
                newToDo.AppendChild(task);
                newToDo.AppendChild(isPerformed);
                newToDo.AppendChild(categoryName);

                if (todo.DateToPerform != null)
                {
                    XmlElement? dateToPerform = document.CreateElement("dateToPerform");
                    dateToPerform.InnerText = todo.DateToPerform.ToString();
                    newToDo.AppendChild(dateToPerform);
                }

                root.AppendChild(newToDo);
                SaveXml(
                    _xmlStorageContext.XmlStoragePath,
                    document);
            }

            return null;
        }

        public Category AddCategory(Category category)
        {
            XmlDocument document = new XmlDocument();

            document.Load(_xmlStorageContext.XmlStoragePath);

            XmlElement root = (XmlElement)document.SelectSingleNode("/database/categories")!;

            if (root != null)
            {
                XmlElement newCategory = document.CreateElement("category");
                XmlElement id = document.CreateElement("id");
                XmlElement name = document.CreateElement("name");

                id.InnerText = category.Id.ToString();
                name.InnerText = category.Name;

                newCategory.AppendChild(id);
                newCategory.AppendChild(name);

                root.AppendChild(newCategory);

                SaveXml(
                    _xmlStorageContext.XmlStoragePath,
                    document);
            }

            return null;
        }

        public ToDo HandlePerformed(ToDo todo) 
        {
            XmlDocument document = new XmlDocument();
            document.Load(_xmlStorageContext.XmlStoragePath);

            string idString = todo.Id.ToString();

            XmlNode? todoNode = document.SelectSingleNode($"/database/todos/todo[id='{idString}']");

            if (todoNode != null)
            {
                XmlElement isPerformed = (XmlElement)todoNode.SelectSingleNode("isPerformed")!;

                isPerformed.InnerText = (!todo.IsPerformed).ToString();
                document.Save(_xmlStorageContext.XmlStoragePath);
            }

            return null;
        }
        
        public void DeleteToDo(ToDo todo)
        {
            XmlDocument document = new XmlDocument();
            document.Load(_xmlStorageContext.XmlStoragePath);

            string idString = todo.Id.ToString();

            XmlNode? todoToDelete = document.SelectSingleNode($"/database/todos/todo[id='{idString}']");

            if (todoToDelete != null)
            {
                todoToDelete.ParentNode?.RemoveChild(todoToDelete);
                SaveXml(
                    _xmlStorageContext.XmlStoragePath,
                    document);
            }
        }

        public void SaveXml(string path, XmlDocument document)
        {
            document.Save(path);
        }
    }
}