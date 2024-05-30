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
    public async Task<IActionResult> Index()
    {
        var toDoListRepository = _repository.GetToDoListRepository();

        var model = new HomePageViewModel
        {
            Categories = await toDoListRepository.GetAllCategories(),
            ToDos = await toDoListRepository.GetAllToDos(),
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AddToDo(ToDo todo)
    {
        await _repository.GetToDoListRepository().AddToDo(todo);

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> AddCategory(Category category)
    {
        await _repository.GetToDoListRepository().AddCategory(category);

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> HandlePerformed(ToDo todo)
    {
        await _repository.GetToDoListRepository().HandlePerformed(todo);

        return RedirectToAction("Index");
    }
    

    [HttpPost]
    public async Task<IActionResult> DeleteToDo(ToDo todo)
    {
        await _repository.GetToDoListRepository().DeleteToDo(todo);

        return RedirectToAction("Index");
    }
}