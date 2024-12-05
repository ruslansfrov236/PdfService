using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Test.Context;
using Test.Helper;
using Test.ViewModels.Person;

using MigraDoc.Rendering;
using MigraDoc.DocumentObjectModel;
using Test.Models;
using Document = MigraDoc.DocumentObjectModel.Document;
using Paragraph = MigraDoc.DocumentObjectModel.Paragraph;
using static QuestPDF.Helpers.Colors;
using System;

namespace Test.Controllers
{
    public class PersonController : Controller
    {
        readonly private IFileService _fileService;
        readonly private AppDbContext _context;

        public PersonController(IFileService fileService, AppDbContext context)
        {
            _fileService = fileService;
            _context = context;
        }

        public IActionResult Create()
            => View(new CreatePersonDto());

        [HttpPost]
        public async Task<IActionResult> Create(CreatePersonDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

           
            var pdf = _fileService.IsPdf(model.FormFile);
            if (!pdf)
            {
                ModelState.AddModelError("model.FormFile", "Daxil olan deyer pdf tipinde deyil!");
                return View(model); 
            }

           
            using (MemoryStream memory = new MemoryStream())
            {
                
                await model.FormFile.CopyToAsync(memory);

              
                Person person = new Person()
                {
                    Name = model.Name,
                    Description = model.Description,
                    FileName = model.FormFile.FileName, 
                    FileData = memory.ToArray() 
                };

                
                await _context.Persons.AddAsync(person);
                await _context.SaveChangesAsync();
            }

            return Redirect("/"); 
        }
        public async Task<IActionResult> PrintAll(int id)
        {
            var person = await _context.Persons.Select(a => new Person()
            {
                Id = a.Id,
                FileData = a.FileData
            }).FirstOrDefaultAsync(a => a.Id == id);




            return View(person);




        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var person = await _context.Persons.FindAsync(id);
            if (person == null) return NotFound();
         

            var data =new UpdatePersonDto()
           {
               Id = person.Id,
               Name = person.Name,
               Description = person.Description,
               FileName = person.FileName,
               FileData = person.FileData,
               

           };
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdatePersonDto model)
        {
          

            var data = await _context.Persons.FindAsync(model.Id);
            if (data == null) return NotFound();

            // Check if file was uploaded
            if (model.FormFile != null)
            {
                var pdf = _fileService.IsPdf(model.FormFile);
                if (!pdf)
                {
                    ModelState.AddModelError("model.FormFile", "Daxil olan deyer pdf tipinde deyil!");
                    return View(model); 
                }

                using (MemoryStream memory = new MemoryStream())
                {
                    await model.FormFile.CopyToAsync(memory);

                  
                    data.FileName = model.FormFile.FileName;
                    data.FileData = memory.ToArray();
                }
            }
           
                data.Id = model.Id;
               
              
                data.Name = model.Name;
                data.Description = model.Description;

            

            await _context.SaveChangesAsync();

      
            return Redirect("/");
        }

        [HttpGet]
        public async Task<IActionResult> GetInvoke(int id)
        {
            var person =  await _context.Persons.FindAsync(id);


            var document = new Document();
            BuildDocument(document, id);
            var pdfRender = new PdfDocumentRenderer();
            pdfRender.Document = document;
            pdfRender.Document = document;
            pdfRender.RenderDocument();


          
            var stream = new MemoryStream();
            
                pdfRender.PdfDocument.Save(stream, false);
                Response.ContentType = "application/pdf";
                Response.Headers.Add("content-length", stream.Length.ToString());
                






            return File(stream.ToArray(), "application/pdf");


        }

        
        private async Task BuildDocument(Document document, int id)
        {
            Section section = document.AddSection();


            var person = await _context.Persons.FindAsync(id);

            var paragraph = new Paragraph();
            paragraph.AddText("Person Details");
            paragraph.AddLineBreak();
            paragraph.AddText($"#: {person.Id}");
            paragraph.AddLineBreak();
            paragraph.AddText($"Name: {person.Name}");
            paragraph.AddLineBreak();
            paragraph.AddText($"Description: {person.Description}");
            paragraph.AddLineBreak();
            paragraph.AddText($"Create Date: {person.CreatedDate}");
            paragraph.Format.SpaceAfter = 20;
           

            var table = document.LastSection.AddTable();
            table.Borders.Width = .5;
            

            table.AddColumn("1cm");
            table.AddColumn("5cm");
            table.AddColumn("5cm");
            table.AddColumn("5cm");


            var row = table.AddRow();
            row.Cells[0].AddParagraph("ID");
            row.Cells[1].AddParagraph("Name");
            row.Cells[2].AddParagraph("Description");
            row.Cells[3].AddParagraph("Created Date");


            row = table.AddRow();
            row.Cells[0].AddParagraph(person.Id.ToString());
            row.Cells[1].AddParagraph(person.Name);
            row.Cells[2].AddParagraph(person.Description);
            row.Cells[3].AddParagraph(person.CreatedDate.ToString("yyyy-MM-dd"));


            paragraph = section.Footers.Primary.AddParagraph();
            paragraph.AddText($"Product details / {person.CreatedDate}");
            paragraph.Format.Alignment = ParagraphAlignment.Center;
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var person = await _context.Persons.FindAsync(id);
            if (person == null) return NotFound();
            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();

            return Redirect("/");



        }


        
    }
}

