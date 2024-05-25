using System.Diagnostics;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Data;
using ToDoList.Models;
using ToDoList.Models.Domain;
using System.Linq;
using ToDoList.Factory;

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

        _repository.ChangeStorage();
        
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
    public async Task<IActionResult> AddToDo(AddToDoRequest addToDoRequest)
    {
        await _repository.GetToDoListRepository().AddToDo(addToDoRequest);

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> AddCategory(AddCategoryRequest addCategoryRequest)
    {
        await _repository.GetToDoListRepository().AddCategory(addCategoryRequest);

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> PerformToDo(HandleTodoRequest handleTodoRequest)
    {
        await _repository.GetToDoListRepository().PerformToDo(handleTodoRequest);

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> UnperformToDo(HandleTodoRequest handleTodoRequest)
    {
        await _repository.GetToDoListRepository().UnperformToDo(handleTodoRequest);

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteToDo(DeleteToDoRequest deleteToDoRequest)
    {
        await _repository.GetToDoListRepository().DeleteToDo(deleteToDoRequest);

        return RedirectToAction("Index");
    }
}