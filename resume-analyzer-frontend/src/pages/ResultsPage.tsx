import { useNavigate } from "react-router-dom";
import { useResume } from "../context/ResumeJobContext"; 

export default function ResultsPage() {
  const navigate = useNavigate();
  const {resumeFile, jobDescription, result} = useResume();

  if (!result) {
    navigate("/upload");
    return null;
  }

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-100 p-4">
      <div className="bg-white p-6 rounded-lg shadow-lg w-full max-w-2xl">
        <h1 className="text-2xl font-bold mb-4 text-center">Match Results</h1>

        {/* Display Match Score */}
        <div className="mb-4">
          <h2 className="text-lg font-semibold">Match Score:</h2>
          <p className="text-2xl font-bold text-green-500">{result.score}%</p>
        </div>

        {/* Display AI Feedback */}
        <div className="mb-4">
          <h2 className="text-lg font-semibold">AI Feedback:</h2>
          <ul className="list-disc ml-6 mt-2">
          {result.feedback.split("\n").map((line, index) => (
            <li key={index}>{line}</li>
          ))}
          </ul>
        </div>

        {/* Go Back Button */}
        <button
          onClick={() => navigate("/upload")}
          className="bg-gray-500 text-white px-4 py-2 rounded w-full hover:bg-gray-600 transition"
        >
          Upload Another Resume
        </button>
      </div>
    </div>
  );
}
