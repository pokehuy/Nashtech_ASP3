using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using asp3.Models;

namespace asp3.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    static List<PersonModel> listPersons = new List<PersonModel>
        {
            new PersonModel{
                Id = 1,
                FirstName = "Nguyen",
                LastName = "Nam Phuong",
                Gender = "Male",
                DateOfBirth = 2001,
                PhoneNumber = "0123456778",
                BirthPlace = "Ha noi",
                IsGraduated = false
            },
            new PersonModel{
                Id = 2,
                FirstName = "Phuong",
                LastName = "Viet Hoang",
                Gender = "Male",
                DateOfBirth = 1999,
                PhoneNumber = "01234545667",
                BirthPlace = "Nam dinh",
                IsGraduated = false
            },
            new PersonModel{
                Id = 3,
                FirstName = "Trinh",
                LastName = "Hong Nhung",
                Gender = "Female",
                DateOfBirth = 1999,
                PhoneNumber = "01298332132",
                BirthPlace = "Thanh hoa",
                IsGraduated = true
            }
        };
    

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
        listPersons = listPersons.OrderBy(person => person.Id).ToList();
        return View(listPersons);
    }

    public IActionResult Privacy()
    {
        return View();
    }
 
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(PersonModel per)
    {
            if(per.Id == null) {
                per.Id = listPersons[listPersons.Count - 1].Id + 1;
                listPersons.Add(per);
                return RedirectToAction("Index");
            } else {
                var person = listPersons.Where(p => p.Id == per.Id).FirstOrDefault();
                listPersons.Remove(person);
                listPersons.Add(per);

                return RedirectToAction("Index");
            }
    }

    public IActionResult Delete(int? id){
        var person = listPersons.Where(person => person.Id == id).FirstOrDefault();
        listPersons.Remove(person);
        return RedirectToAction("Index");
    }

    // http get, -> click edit -> check if person is available to edit (return to edit page with person) or the new one (return nothing)
    public IActionResult Edit(int? id){
        var person = listPersons.Where(person => person.Id == id).FirstOrDefault();
        return View(person);       
    }

    //url: localhost:port/NashTech/Home/MalePersons
    public IActionResult MalePersons()
    {
        List<PersonModel> listMalePersons = GetMaleMembers(listPersons);
        return View(listMalePersons);
    }

    //url: localhost:port/NashTech/Home/OldestPerson
    public IActionResult OldestPerson()
    {
        var personModel = GetOldestMember(listPersons);
        return View(personModel);
    }

    //url: localhost:port/NashTech/Home/FullnamePersons
    public IActionResult FullnamePersons()
    {
        var listName = GetFullNameList(listPersons);
        return View(listName);
    }

    //url: localhost:port/NashTech/Home/Get3Lists
    public IActionResult Get3Lists()
    {
        var list3 = List3(listPersons);
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
