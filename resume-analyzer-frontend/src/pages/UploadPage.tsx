import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useResume } from "../context/ResumeJobContext"

export default function UploadPage() {
  const { setResume, setJobDescription ,submitResumeJobData, loading} = useResume();
  const navigate = useNavigate();

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files){
        setResume(e.target.files[0]);
    }
  };

  const handleTextChange = (e: React.ChangeEvent<HTMLTextAreaElement>) => {
    setJobDescription(e.target.value);
  }

  const handleSubmit = async (event: React.FormEvent) => {
    event.preventDefault();
    await submitResumeJobData();
    navigate("/results");
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-100 p-4">
      <div className="bg-white p-6 rounded-lg shadow-lg w-full max-w-2xl">
        <h1 className="text-2xl font-bold mb-4 text-center">Upload Your Resume</h1>
        <form onSubmit={handleSubmit}>
            {/* Resume Upload */}
            <div className="mb-4">
            <label className="block text-gray-700 font-semibold mb-2">Upload Resume (PDF)</label>
            <input
                type="file"
                accept=".pdf"
                onChange={handleFileChange}
                className="border border-gray-300 p-2 rounded w-full"
                disabled={loading}
            />
            </div>

            {/* Job Description Input */}
            <div className="mb-4">
            <label className="block text-gray-700 font-semibold mb-2">Paste Job Description</label>
            <textarea
                rows={5}
                placeholder="Paste job description here..."
                onChange={handleTextChange}
                className="border border-gray-300 p-2 rounded w-full resize-none"
                disabled={loading}
            />
            </div>

            {/* Analyze Button */}
            <button
                type="submit" 
                className="bg-blue-500 text-white px-4 py-2 rounded w-full hover:bg-blue-600 transition"
            >
            Analyze
            </button>
        </form>
      </div>
    </div>
  );
}
