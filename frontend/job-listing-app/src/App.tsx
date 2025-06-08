import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import "./App.css";
import JobsPage from "./pages/JobsPage";

const queryClient = new QueryClient();

function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <div className="min-h-screen flex flex-col items-center justify-start bg-gray-100 p-4">
        <div className="w-full max-w-4xl bg-white rounded-2xl shadow-lg p-6">
          <h1 className="text-3xl font-bold mb-6 text-center">Jobs</h1>
          <JobsPage />
        </div>
      </div>
    </QueryClientProvider>
  );
}

export default App;
