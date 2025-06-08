import { useMutation } from "@tanstack/react-query";
import { useState } from "react";
import { apiClient } from "../api/ApiClient";
import type { AuthenticationRequest } from "../contracts/AuthenticationRequest";
import type { AuthenticationResponse } from "../contracts/AuthenticationResponse";

export default function LoginPage({
  onLoginSuccess,
}: Readonly<{
  onLoginSuccess: (firstName: string) => void;
}>) {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

  const loginMutation = useMutation({
    mutationFn: (data: AuthenticationRequest) =>
      apiClient.post<AuthenticationResponse>("/auth/auth/authenticate", data),
    onSuccess: (response) => {
      if (!response.data.isAuthenticated) {
        alert("Login failed. Check your credentials.");
        return;
      }
      localStorage.setItem("token", response.headers["Authorization"]);
      localStorage.setItem("userId", response.data.userId);
      localStorage.setItem("username", response.data.userName);
      localStorage.setItem("firstName", response.data.firstName);
      localStorage.setItem("lastName", response.data.lastName);
      localStorage.setItem("role", response.data.role);
      onLoginSuccess(response.data.firstName);
    },
    onError: () => {
      alert("Login failed. Check your credentials.");
    },
  });

  const handleLogin = () => {
    loginMutation.mutate({ username, password });
  };

  const handleRegister = () => {
    alert("Registered (dummy)");
  };

  return (
    <>
      <input
        type="text"
        placeholder="Username"
        value={username}
        onChange={(e) => setUsername(e.target.value)}
        className="w-full mb-3 p-2 border border-gray-300 rounded"
      />
      <input
        type="password"
        placeholder="Password"
        value={password}
        onChange={(e) => setPassword(e.target.value)}
        className="w-full mb-4 p-2 border border-gray-300 rounded"
      />
      <div className="flex justify-between">
        <button
          onClick={handleLogin}
          className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600"
        >
          Login
        </button>
        <button
          onClick={handleRegister}
          className="bg-gray-500 text-white px-4 py-2 rounded hover:bg-gray-600"
        >
          Register
        </button>
      </div>
    </>
  );
}
