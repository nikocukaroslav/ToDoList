using System.Diagnostics;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Models;
using ToDoList.Models.Domain;
using System.Linq;

namespace ToDoList.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ToDoListDbContext toDoListDbContext;
    private IToDoListXmlStorage toDoListXmlStorage;

    public HomeController(ILogger<HomeController> logger, ToDoListDbContext toDoListDbContext,
        IToDoListXmlStorage toDoListXmlStorage)
    {
        _logger = logger;
        this.toDoListDbContext = toDoListDbContext;
        this.toDoListXmlStorage = toDoListXmlStorage;
    }

    [HttpGet]
    public async Task<IActionResult> Index(ChangeStorageRequest changeStorageRequest)
    {
        
        
        List<ToDo> todos = await toDoListDbContext.ToDo.ToListAsync();
        List<Category> categories = await toDoListDbContext.Category.ToListAsync();
        List<ToDo> todosXml =
            toDoListXmlStorage.LoadAllToDosXml(
                "C:\\Users\\Ярик\\Desktop\\Bootcamp\\ToDoListMVC\\ToDoList\\Data\\Database.xml");
        List<Category> categoriesXml =
            toDoListXmlStorage.LoadAllCategories(
                "C:\\Users\\Ярик\\Desktop\\Bootcamp\\ToDoListMVC\\ToDoList\\Data\\Database.xml");

        var model = new HomePageViewModel
        {
            Categories = categories,
            CategoriesXml = categoriesXml,
            ToDos = todos,
            ToDosXml = todosXml,
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AddToDo(AddToDoRequest addToDoRequest)
    {
        var todo = new ToDo
        {
            Task = addToDoRequest.Task,
            CategoryName = addToDoRequest.CategoryName,
            DateToPerform = addToDoRequest.DateToPerform,
        };
        await toDoListDbContext.ToDo.AddAsync(todo);
        await toDoListDbContext.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> AddCategory(AddCategoryRequest addCategoryRequest)
    {
        var category = new Category
        {
            Name = addCategoryRequest.Name,
        };
        await toDoListDbContext.Category.AddAsync(category);
        await toDoListDbContext.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> PerformToDo(PerformTodoRequest performTodoRequest)
    {
        var todoToPerform = await toDoListDbContext.ToDo.FindAsync(performTodoRequest.Id);

        if (todoToPerform != null)
        {
            todoToPerform.IsPerformed = true;
            await toDoListDbContext.SaveChangesAsync();
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> UnperformToDo(PerformTodoRequest performTodoRequest)
    {
        var todoToUnperform = await toDoListDbContext.ToDo.FindAsync(performTodoRequest.Id);

        if (todoToUnperform != null)
        {
            todoToUnperform.IsPerformed = false;
            await toDoListDbContext.SaveChangesAsync();
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteToDo(DeleteToDoRequest deleteToDoRequest)
    {
        var todoToDelete = await toDoListDbContext.ToDo.FindAsync(deleteToDoRequest.Id);

        if (todoToDelete != null)
        {
            toDoListDbContext.Remove(todoToDelete);
            await toDoListDbContext.SaveChangesAsync();
        }

        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpPost]
    public IActionResult AddToDoXml(AddToDoRequest addToDoRequest)
    {
        XmlDocument document = new XmlDocument();

        document.Load("C:\\Users\\Ярик\\Desktop\\Bootcamp\\ToDoListMVC\\ToDoList\\Data\\Database.xml");

        XmlElement root = (XmlElement)document.SelectSingleNode("/database/todos")!;

        if (root != null)
        {
            XmlElement todo = document.CreateElement("todo");
            XmlElement id = document.CreateElement("id");
            XmlElement task = document.CreateElement("task");
            XmlElement isPerformed = document.CreateElement("isPerformed");
            XmlElement categoryName = document.CreateElement("categoryName");

            id.InnerText = addToDoRequest.Id.ToString();
            task.InnerText = addToDoRequest.Task;
            isPerformed.InnerText = addToDoRequest.IsPerformed.ToString();
            categoryName.InnerText = addToDoRequest.CategoryName;

            todo.AppendChild(id);
            todo.AppendChild(task);
            todo.AppendChild(isPerformed);
            todo.AppendChild(categoryName);

            if (addToDoRequest.DateToPerform != null)
            {
                XmlElement? dateToPerform = document.CreateElement("dateToPerform");
                dateToPerform.InnerText = addToDoRequest.DateToPerform.ToString();
                todo.AppendChild(dateToPerform);
            }

            root.AppendChild(todo);
            toDoListXmlStorage.SaveXml("C:\\Users\\Ярик\\Desktop\\Bootcamp\\ToDoListMVC\\ToDoList\\Data\\Database.xml",
                document);
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult AddCategoryXml(AddCategoryRequest addCategoryRequest)
    {
        XmlDocument document = new XmlDocument();

        document.Load("C:\\Users\\Ярик\\Desktop\\Bootcamp\\ToDoListMVC\\ToDoList\\Data\\Database.xml");

        XmlElement root = (XmlElement)document.SelectSingleNode("/database/categories")!;

        if (root != null)
        {
            XmlElement category = document.CreateElement("category");
            XmlElement id = document.CreateElement("id");
            XmlElement name = document.CreateElement("name");

            id.InnerText = addCategoryRequest.Id;
            name.InnerText = addCategoryRequest.Name;

            category.AppendChild(id);
            category.AppendChild(name);

            root.AppendChild(category);

            toDoListXmlStorage.SaveXml("C:\\Users\\Ярик\\Desktop\\Bootcamp\\ToDoListMVC\\ToDoList\\Data\\Database.xml",
                document);
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult PerformToDoXml(PerformTodoRequest performTodoRequest)
    {
        XmlDocument document = new XmlDocument();
        document.Load("C:\\Users\\Ярик\\Desktop\\Bootcamp\\ToDoListMVC\\ToDoList\\Data\\Database.xml");

        string idString = performTodoRequest.Id.ToString();

        // Find the 'todo' node with the specified ID attribute
        XmlNode todoNode = document.SelectSingleNode($"/database/todos/todo[id='{idString}']")!;

        if (todoNode != null)
        {
            XmlElement isPerformed = (XmlElement)todoNode.SelectSingleNode("isPerformed")!;
            if (isPerformed != null)
            {
                isPerformed.InnerText = true.ToString();
                document.Save("C:\\Users\\Ярик\\Desktop\\Bootcamp\\ToDoListMVC\\ToDoList\\Data\\Database.xml");
            }
        }

        return RedirectToAction("Index");
    }


    [HttpPost]
    public IActionResult UnPerformToDoXml(PerformTodoRequest performTodoRequest)
    {
        XmlDocument document = new XmlDocument();
        document.Load("C:\\Users\\Ярик\\Desktop\\Bootcamp\\ToDoListMVC\\ToDoList\\Data\\Database.xml");

        string idString = performTodoRequest.Id.ToString();

        XmlNode todoNode = document.SelectSingleNode($"/database/todos/todo[id='{idString}']")!;

        if (todoNode != null)
        {
            XmlElement isPerformed = (XmlElement)todoNode.SelectSingleNode("isPerformed")!;
            if (isPerformed != null)
            {
                isPerformed.InnerText = false.ToString();
                document.Save("C:\\Users\\Ярик\\Desktop\\Bootcamp\\ToDoListMVC\\ToDoList\\Data\\Database.xml");
            }
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult DeleteToDoXml(DeleteToDoRequest deleteToDoRequest)
    {
        XmlDocument document = new XmlDocument();
        document.Load("C:\\Users\\Ярик\\Desktop\\Bootcamp\\ToDoListMVC\\ToDoList\\Data\\Database.xml");

        string idString = deleteToDoRequest.Id.ToString();

        XmlNode? todoToDelete = document.SelectSingleNode($"/database/todos/todo[id='{idString}']");

        if (todoToDelete != null)
        {
            todoToDelete.ParentNode?.RemoveChild(todoToDelete);
            toDoListXmlStorage.SaveXml("C:\\Users\\Ярик\\Desktop\\Bootcamp\\ToDoListMVC\\ToDoList\\Data\\Database.xml",
                document);
        }

        return RedirectToAction("Index");
    }
}