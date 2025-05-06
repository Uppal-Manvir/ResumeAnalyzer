import { createContext, useContext, useState, ReactNode } from "react";
interface ResumeContextType {
  resumeFile: File | null;
  jobDescription: string;
  result:{score: number; feedback: string} | null;
  loading: boolean;
  setResume: (file: File | null) => void;
  setJobDescription: (text: string) => void;
  submitResumeJobData: () => Promise<void>;
}

// Create the context with default empty values
const ResumeContext = createContext<ResumeContextType | undefined>(undefined);


//TODO: Add loading fuction dont work yet, comprend and make everything just better

export function useResume() {
  const context = useContext(ResumeContext);
  if (!context) {
    throw new Error("useResume must be used within a ResumeProvider");
  }
  return context;
}

// Context Provider component
export const ResumeProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
  const [resumeFile, setResume] = useState<File | null>(null);
  const [jobDescription, setJobDescription] = useState("");
  const [result, setResult] = useState<{ score: number; feedback: string } | null>(null);
  const [loading, setLoading] = useState(false); 


  const submitResumeJobData = async (): Promise<void> => {
    if (!resumeFile || !jobDescription) {
      console.error("Resume file and job description are required");
      return;
    }
    const formData = new FormData();
    formData.append("resume", resumeFile);
    formData.append("jobDescription", jobDescription);

    try {
      const response = await fetch("https://localhost:5001/api/resume/analyze", {

        method: "POST",
        body: formData,
      });
      //debugger
      if (!response.ok) {
        console.log(response.body)
        throw new Error("Failed to submit resume data: ${response.statusText}");
      }

      const data = await response.json();
      console.log("Response from backend:", data);

      setResult(data);
    } catch (error) {
      console.error("Error submitting resume data:", error);
    }finally{
      setLoading(false);
    }


  }
  return (
    <ResumeContext.Provider value={{ resumeFile, jobDescription, result, setResume,loading, setJobDescription, submitResumeJobData }}>
      {children}
    </ResumeContext.Provider>
  );
}