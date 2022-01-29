using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using asp3.Models;

namespace asp3.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private List<PersonModel> GetList(){
        return new List<PersonModel>
        {
            new PersonModel{
                FirstName = "Nguyen",
                LastName = "Nam Phuong",
                Gender = "Male",
                DateOfBirth = 2001,
                PhoneNumber = "0123456778",
                BirthPlace = "Ha noi",
                IsGraduated = false
            },
            new PersonModel{
                FirstName = "Phuong",
                LastName = "Viet Hoang",
                Gender = "Male",
                DateOfBirth = 1999,
                PhoneNumber = "01234545667",
                BirthPlace = "Nam dinh",
                IsGraduated = false
            },
            new PersonModel{
                FirstName = "Trinh",
                LastName = "Hong Nhung",
                Gender = "Female",
                DateOfBirth = 1999,
                PhoneNumber = "01298332132",
                BirthPlace = "Thanh hoa",
                IsGraduated = true
            }
        };
    }

    private List<PersonModel> GetMaleMembers(List<PersonModel> listMember){
            var maleMembers = from member in listMember where member.Gender == "Male" select member;
            return maleMembers.ToList();
        }

    private PersonModel GetOldestMember(List<PersonModel> listMember){
        var oldestMember = from member in listMember orderby member.DateOfBirth ascending select member;
        return oldestMember.FirstOrDefault();
    }

    private List<string> GetFullNameList(List<PersonModel> listMember){
        var fullname = from member in listMember select string.Join(" ", member.FirstName, member.LastName);
        return fullname.ToList();
    }

    private List<List<PersonModel>> List3(List<PersonModel> listMember){
        var under2000 = from member in listMember where (member.DateOfBirth < 2000) select member;
        var is2000 = from member in listMember where (member.DateOfBirth == 2000) select member;
        var over2000 = from member in listMember where (member.DateOfBirth > 2000) select member;

        List<List<PersonModel>> list3 = new List<List<PersonModel>>{under2000.ToList(), is2000.ToList(), over2000.ToList()};
        return list3;
    }

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    //url: localhost:port/NashTech/Home/MalePersons
    public IActionResult MalePersons()
    {
        List<PersonModel> listMalePersons = GetMaleMembers(GetList());
        return View(listMalePersons);
    }

    //url: localhost:port/NashTech/Home/OldestPerson
    public IActionResult OldestPerson()
    {
        var personModel = GetOldestMember(GetList());
        return View(personModel);
    }

    //url: localhost:port/NashTech/Home/FullnamePersons
    public IActionResult FullnamePersons()
    {
        var listName = GetFullNameList(GetList());
        return View(listName);
    }

    //url: localhost:port/NashTech/Home/Get3Lists
    public IActionResult Get3Lists()
    {
        var list3 = List3(GetList());
        return View(list3);
    }

    //url: localhost:port/NashTech/Home/DownloadFile
    public FileResult DownloadFile(){
        return File("Assets/Person.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Person.xlsx");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
