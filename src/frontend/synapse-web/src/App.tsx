import { useState, useEffect } from "react";
import { HomePage } from "./pages/HomePage";
import { LoginPage } from "./pages/LoginPage";
import { RegisterPage } from "./pages/RegisterPage";
import { getToken } from "./auth/tokenStorage";

type AuthMode = "login" | "register"; 

function App() {
  const [isAuthenticated, setIsAuthenticated] = useState(false);

  const [mode, setMode] = useState<AuthMode>("login");

  useEffect(()=>{
    const token = getToken();
    setIsAuthenticated(!!token);
  }, [])

  if (!isAuthenticated) {
    if (mode === "login") {
      return (
        <LoginPage
          onLoginSuccess={() => setIsAuthenticated(true)}
          onGoToRegister={() => setMode("register")}
        />
      );
    }

    return (
      <RegisterPage
        onRegisterSuccess={() => setIsAuthenticated(true)}
        onGoToLogin={() => setMode("login")}
      />
    );
  }

  return (
    <HomePage
      onLogout={() => setIsAuthenticated(false)}
    />
  );
}

export default App;