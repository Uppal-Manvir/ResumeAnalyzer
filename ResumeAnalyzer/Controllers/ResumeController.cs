using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using System.Text;

[Route("api/resume")]
[ApiController]
public class ResumeController : ControllerBase
{
    private readonly JobMatchService  _jobMatchService;
    public ResumeController(JobMatchService jobMatchService){
        _jobMatchService = jobMatchService;
    }
    [HttpPost("upload")]
    public async Task<IActionResult> UploadResume(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded");

        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", file.FileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var text = ExtractTextFromPDFSaved(filePath);

        return Ok(
            new { 
                message = "File uploaded successfully", 
                fileName = file.FileName,
                text = text
            }
        );
    }
    [HttpPost("analyze")]
    public async Task<IActionResult> AnalyzeResume([FromForm] IFormFile resume, [FromForm] string jobDescription)
    {
        if (resume == null || string.IsNullOrEmpty(jobDescription))
        {
            return BadRequest("Resume and job description are required.");
        }
        string resumeText = ExtractTextFromPDFFile(resume);
    
        if (string.IsNullOrWhiteSpace(resumeText))
        {
            return BadRequest("Could not extract text from resume.");
        }
        double matchScore = await _jobMatchService.CalculateSimilarity(jobDescription, resumeText);

        string aiFeedback = await _jobMatchService.GenerateResumeFeedback(resumeText, jobDescription);


        return Ok(new 
        { 
            message = "Resume processed successfully",
            score = matchScore,
            feedback = aiFeedback
        });
    }
    private string ExtractTextFromPDFFile(IFormFile file)
{
    if (file == null || file.Length == 0)
        return "Unsupported File Type";

    using (var stream = file.OpenReadStream())
    using (PdfReader pdfReader = new PdfReader(stream))
    using (PdfDocument pdfDoc = new PdfDocument(pdfReader))
    {
        StringBuilder text = new StringBuilder();
        for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
        {
            text.Append(PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(i)));
        }
        return text.ToString();
    }
}
    private string ExtractTextFromPDFSaved(string filePath){
        if(Path.GetExtension(filePath).ToLower() == ".pdf"){
            StringBuilder text = new StringBuilder();

            using (PdfReader pdfReader  = new PdfReader(filePath))
            using (PdfDocument pdfDoc = new PdfDocument(pdfReader))
            {
                for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++){
                    text.Append(PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(i)));
                }
            }
            return text.ToString();
        }
        else{
            return "Unsupported File type";
        }
    }
}

