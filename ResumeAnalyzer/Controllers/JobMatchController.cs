using System.ClientModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Embeddings;


[Route("api/JobMatch")]
[ApiController]
public class JobMatchController : ControllerBase{
    private string? _openAiKey;

    public JobMatchController (IConfiguration configuration){
        _openAiKey = configuration["OpenAI:ApiKey"];

    }

    [HttpPost("compare")]
    public async Task<IActionResult> CompareJobToResume([FromBody] ResumeJobComparison request){
        if(string.IsNullOrWhiteSpace(request.JobDescription) || string.IsNullOrWhiteSpace(request.ResumeText)){
            return BadRequest("Both resume text and job description are req");
        }

        double matchScore = await CalculateSimilarity(request.JobDescription, request.ResumeText);
        string aiFeedback = await GenerateResumeFeedback(request.ResumeText, request.JobDescription);


        return Ok(
            new{
                message = "Score calculated",
                score = matchScore,
                feedback = aiFeedback
            }
        );

    }

    private async Task<string> GenerateResumeFeedback(string resumeText, string jobDescription)
    {
        ChatClient client = new(model: "gpt-4o", _openAiKey);

        var prompt = $@"
        Analyze the following resume and job description. 
        Identify missing skills, experiences, and suggest specific improvements.
        Respond concisely with key points.
        
        Resume:
        {resumeText}

        Job Description:
        {jobDescription}

        Provide the missing skills and improvements in bullet points.
        ";

        ChatCompletion response = await client.CompleteChatAsync(prompt);

        return response.Content[0].Text ?? "No feedback avalible";
    }

    [HttpPost("testEmbed")]
    public async Task<double> CalculateSimilarity(string jobDescription, string resumeText)
    {
        Console.WriteLine("We in dis bitch");
        //var aiApi = new OpenAIClient(_openAiKey);

        EmbeddingClient embeddingClient = new("text-embedding-3-small", _openAiKey);

        //var embeddingClient =  aiApi.GetEmbeddingClient("text-embedding-3-small");
        
        OpenAIEmbedding resumeEmbed = await embeddingClient.GenerateEmbeddingAsync(resumeText);
        OpenAIEmbedding jobEmbed = await embeddingClient.GenerateEmbeddingAsync(jobDescription);

        //var jobEmbed = await embeddingClient.GenerateEmbeddingAsync(jobDescription);

        ReadOnlyMemory<float> resumeVector = resumeEmbed.ToFloats();
        ReadOnlyMemory<float> jobVector = jobEmbed.ToFloats();


        //var ret1 = resumeEmbed.toFloats;
        //var ret2 = jobEmbed.Value.ToString;

        double similarityScore = CosineSimilarity(resumeVector, jobVector);


        // for (int i = 0; i < vector.Length; i++)
        // {
        //     Console.WriteLine($"  [{i,4}] = {vector.Span[i]}");
        // }

        //Console.WriteLine("STARTTTTT : " + ret2 + " ----ENDDDDD");
        //Console.WriteLine("STARTTTTT : " + vector.Length + " ----ENDDDDD");
        return similarityScore * 100;
        
    }

    private double CosineSimilarity(ReadOnlyMemory<float> resumeEmbed, ReadOnlyMemory<float> jobEmbed)
    {
        double dotProduct = 0.0;
    double magnitude1 = 0.0;
    double magnitude2 = 0.0;

    for (int i = 0; i < resumeEmbed.Length; i++)
    {
        dotProduct += resumeEmbed.Span[i] * jobEmbed.Span[i]; // Multiply corresponding elements
        magnitude1 += Math.Pow(resumeEmbed.Span[i], 2);   // Sum of squares for vector 1
        magnitude2 += Math.Pow(jobEmbed.Span[i], 2);   // Sum of squares for vector 2
    }

    magnitude1 = Math.Sqrt(magnitude1); // Square root to get magnitude
    magnitude2 = Math.Sqrt(magnitude2);

    if (magnitude1 == 0 || magnitude2 == 0) return 0; // Avoid division by zero

    return dotProduct / (magnitude1 * magnitude2); // Cosine similarity formula
    }
}

public class ResumeJobComparison{
    public string ResumeText {get; set; }
    public string JobDescription {get; set;}
}