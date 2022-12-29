using System.IO.MemoryMappedFiles;
using BennyBooks.Models;
using Microsoft.AspNetCore.Mvc;
using YamlDotNet.Serialization;
using System.IO;

namespace BennyBooksWeb.Areas.Customer.Controllers;

[Area("Customer")]
public class PortfolioController : Controller
{
    private readonly IWebHostEnvironment _env;

    public PortfolioController(IWebHostEnvironment env)
    {
        _env = env;
    }
    [HttpGet]
    public IActionResult Index() // right click method and click go to view or create view
    {
        string filePath = Path.Combine(_env.WebRootPath, "yaml", "portfolio.yaml");
        // Open the YAML file in using a FileStream. The using with a parentheses is so the object is disposable, once it has ran it will be disposed of
        using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            // Read the contents of the file using a StreamReader
            using (var reader = new StreamReader(stream))
            {
                // Deserialize the YAML to a list of Portfolio objects
                var deserializer = new DeserializerBuilder().Build();


                var portfolios = deserializer.Deserialize<IEnumerable<Portfolio>>(reader);

                return View(portfolios);
            }
        }
    }
}
