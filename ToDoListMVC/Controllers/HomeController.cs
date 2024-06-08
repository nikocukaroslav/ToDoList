using Microsoft.AspNetCore.Mvc;
using ToDoList.Data;
using ToDoList.Models;
using ToDoList.Factory;
using ToDoList.Models.Domain;
using ToDoList.Models.ViewModels;

namespace ToDoList.Controllers;

public class HomeController : Controller
{
    private readonly StorageChanger _repository;

    public HomeController(StorageChanger storageChanger)
    {
        _repository = storageChanger;
    }

    public IActionResult ChangeStorage(ChangeStorageRequest changeStorageRequest)
    {
        if (changeStorageRequest.StorageName != null)
            HttpContext.Session.SetString("StorageName", changeStorageRequest.StorageName);
        
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Index()
    {
        var toDoListRepository = _repository.GetToDoListRepository();

        var model = new HomePageViewModel
        {
            Categories = toDoListRepository.GetAllCategories(),
            ToDos =  toDoListRepository.GetAllToDos(),
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult AddToDo(ToDo todo)
    {
        _repository.GetToDoListRepository().AddToDo(todo);

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult AddCategory(Category category)
    {
         _repository.GetToDoListRepository().AddCategory(category);

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult HandlePerformed(ToDo todo)
    {
         _repository.GetToDoListRepository().HandlePerformed(todo);

        return RedirectToAction("Index");
    }
    

    [HttpPost]
    public IActionResult DeleteToDo(ToDo todo)
    {
        _repository.GetToDoListRepository().DeleteToDo(todo);

        return RedirectToAction("Index");
    }
}