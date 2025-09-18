using FitnessBookingApp.Controllers;
using FitnessBookingApp.Models;
using FitnessBookingApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
public class TrainingController : BaseController
{
    private readonly DataService _dataService;


    public TrainingController(DataService dataService) : base(dataService)
    {
        _dataService = dataService;
    }       

    public IActionResult Index()
    {
        var trainings = _dataService.GetTrainings();
        return View(trainings);
    }

    [HttpPost]
    public IActionResult Create(Training training)
    {
        training.Id = Guid.NewGuid(); // správně, typ Guid
        training.TrainingRegistrations = new List<TrainingRegistration>(); // DB-friendly
        _dataService.AddTraining(training);
        return RedirectToAction("Index");
    }


    [HttpPost]
    public IActionResult Edit(Training training)
    {
        _dataService.UpdateTraining(training);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Delete(Guid id)
    {
        _dataService.DeleteTraining(id);
        return RedirectToAction("Index");
    }
    [HttpPost]
    public IActionResult GenerateNextMonth()
    {
        _dataService.GenerateNextMonth();
        return RedirectToAction("Index", "Home");
    }
}
