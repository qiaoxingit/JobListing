import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { useEffect, useState } from "react";
import "./App.css";
import JobsPage from "./pages/JobsPage";
import LoginPage from "./pages/LoginPage";

const queryClient = new QueryClient();

function App() {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [firstName, setFirstName] = useState("");

  useEffect(() => {
    const token = localStorage.getItem("token");
    const userId = localStorage.getItem("userId");
    setFirstName(localStorage.getItem("firstName") ?? "");
    if (token && userId) {
      setIsLoggedIn(true);
    }
  }, []);

  const handleLoginSuccess = (firstName: string) => {
    setIsLoggedIn(true);
    setFirstName(firstName);
  };

  const handleLogout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("userId");
    localStorage.removeItem("username");
    localStorage.removeItem("firstName");
    localStorage.removeItem("lastName");
    localStorage.removeItem("role");
    setIsLoggedIn(false);
    setFirstName("");
  };

  return (
    <QueryClientProvider client={queryClient}>
      <div className="min-h-screen flex flex-col items-center justify-start bg-gray-100 p-4">
        <div className="w-full max-w-4xl bg-white rounded-2xl shadow-lg p-6">
          <h1 className="text-3xl font-bold mb-6 text-center">Jobs</h1>
          <div className="flex justify-between items-center mb-6">
            {isLoggedIn && (
              <>
                <span className="text-lg font-semibold">
                  Welcome, {firstName}!
                </span>
                <button
                  onClick={handleLogout}
                  className="ml-4 px-4 py-2 bg-red-500 text-white rounded hover:bg-red-600"
                >
                  Logout
                </button>
              </>
            )}
          </div>
          {isLoggedIn ? (
            <JobsPage />
          ) : (
            <LoginPage onLoginSuccess={handleLoginSuccess} />
          )}
        </div>
      </div>
    </QueryClientProvider>
  );
}

export default App;
