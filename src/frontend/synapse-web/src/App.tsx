import { Routes, Route, Navigate } from "react-router-dom";
import { HomePage } from "./pages/HomePage";
import { LoginPage} from "./pages/LoginPage";
import { RegisterPage } from "./pages/RegisterPage";
import { NoteDetailPage } from "./pages/NoteDetailPage";
import { getToken } from "./auth/tokenStorage";
import { useState } from "react";

function App() {
  const [isAuthenticated, setIsAuthenticated] = useState(!!getToken());

  if (!isAuthenticated) {
    return(
      <Routes>
        <Route path="/login" element={<LoginPage onLogin={()=> setIsAuthenticated(true)}/>} />
        <Route path="/register" element={<RegisterPage onRegister={()=> setIsAuthenticated(true)}/>} />
        <Route path="*" element={<Navigate to="/login" />} />
      </Routes>
    )
  }

  return (
    <Routes>
      <Route path="/" element={<HomePage />} />
      <Route path="/notes/:id" element={<NoteDetailPage />} />
    </Routes>
  );
}

export default App;