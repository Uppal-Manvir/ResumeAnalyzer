import React from 'react';
import logo from './logo.svg';
import './App.css';
import { BrowserRouter as Router, Routes, Route, Navigate } from "react-router-dom";
import UploadPage from "./pages/UploadPage";
import ResultsPage from "./pages/ResultsPage";
import { ResumeProvider } from "./context/ResumeJobContext"; // Import the context provider


function App() {
  return (
    <ResumeProvider>
      <Router>
        <Routes>
        <Route path="/" element={<Navigate to="/upload" />} />
          <Route path="/upload" element={<UploadPage />} />
          <Route path="/results" element={<ResultsPage />} />
        </Routes>
      </Router>
    </ResumeProvider>
  );
}

export default App;
