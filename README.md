# AI-Powered Resume Analyzer

An AI-driven web application that analyzes resumes against job descriptions and provides match scores, missing skill feedback, and job recommendations.

## ✨ Features

- **Semantic Resume Scoring** – Uses OpenAI embeddings (`text-embedding-3-small`) to compare resume content with job descriptions.
- **AI Feedback Generator** – GPT-4 identifies missing skills, soft skills, and specific experience gaps based on job postings.
- **Automated Job Scraping** – A Python + Selenium pipeline pulls job listings from the web, scores them, and stores results.
- **Scoring Dashboard** – Backend-integrated job scoring with PostgreSQL storage and .NET Core API endpoints.

## 🧰 Tech Stack

| Frontend | Backend | AI & NLP | Automation | DevOps |
|----------|---------|----------|------------|--------|
| React (TypeScript) | .NET Core Web API | OpenAI API | Python + Selenium | Azure DevOps, Docker |
| TailwindCSS | PostgreSQL | GPT-4 + Embeddings | Job Queue | CI/CD Pipelines |

## 🔍 How It Works

1. User uploads a resume and inputs or scrapes a job description.
2. OpenAI embeddings generate semantic vectors for both texts.
3. A cosine similarity score is calculated and stored in PostgreSQL.
4. GPT-4 generates resume improvement suggestions.
5. Frontend displays results with score breakdown and suggestions.

## 🚀 Status

Phase 1 & 2 complete:  
- Resume-to-job matching using OpenAI embeddings  
- AI-powered feedback with resume improvement suggestions  
- Frontend UI for upload, results, and feedback  
- Job scraping from multiple platforms (Indeed, etc.)  
- Automatic backend scoring of new job posts  
- Matching logic to skip duplicate or old jobs 

Next: Notification system for high-scoring jobs and user authentication and refining.

See [TODO.md](TODO.md) for upcoming features and planned improvements.

## 📂 Project Structure

