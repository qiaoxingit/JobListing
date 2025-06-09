import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  MenuItem,
  Select,
  TextField,
} from "@mui/material";
import { useState } from "react";
import { apiClient } from "../api/ApiClient";
import type { Job } from "../contracts/Job";
import { Role, type User } from "../contracts/User";

export default function UserEditDialog({
  open,
  isEdit,
  onClose,
  onSuccess,
  user,
}: Readonly<{
  open: boolean;
  isEdit: boolean;
  onClose: () => void;
  onSuccess: () => void;
  user?: User | null;
}>) {
  const [newUserFirstName, setNewUserFirstName] = useState(
    user ? user.firstName : ""
  );
  const [newUserLastName, setNewUserLastName] = useState(
    user ? user.lastName : ""
  );
  const [newUserEmail, setNewUserEmail] = useState(user ? user.email : "");
  const [newUserUsername, setNewUserUsername] = useState(
    user ? user.username : ""
  );
  const [newUserPassword, setNewUserPassword] = useState("");
  const [newUserRole, setNewUserRole] = useState(
    user ? user.role : Role.Viewer
  );

  const [message, setMessage] = useState("");

  const handlePost = async () => {
    try {
      await apiClient.post<Job>("/user/user/Register", {
        firstName: newUserFirstName,
        lastName: newUserLastName,
        email: newUserEmail,
        username: newUserUsername,
        password: newUserPassword,
        role: newUserRole,
      });

      setNewUserFirstName("");
      setNewUserLastName("");
      setNewUserEmail("");
      setNewUserUsername("");
      setNewUserPassword("");
      setNewUserRole(Role.Viewer);

      onClose();
      onSuccess();
    } catch {
      setMessage("User Registration failed");
    }
  };

  const handleCancel = () => {
    setNewUserFirstName("");
    setNewUserLastName("");
    setNewUserEmail("");
    setNewUserUsername("");
    setNewUserPassword("");
    setNewUserRole(Role.Viewer);

    onClose();
  };

  return (
    <Dialog open={open} onClose={handleCancel} fullWidth>
      <DialogTitle>User Profile</DialogTitle>
      <DialogContent className="space-y-4 mt-2">
        <TextField
          label="First Name"
          fullWidth
          margin="normal"
          value={newUserFirstName}
          onChange={(e) => {
            setNewUserFirstName(e.target.value);
          }}
        />
        <TextField
          label="Last Name"
          fullWidth
          margin="normal"
          value={newUserLastName}
          onChange={(e) => {
            setNewUserLastName(e.target.value);
          }}
        />
        <TextField
          label="Email"
          fullWidth
          margin="normal"
          value={newUserEmail}
          onChange={(e) => {
            setNewUserEmail(e.target.value);
          }}
        />
        <TextField
          label="Username"
          fullWidth
          margin="normal"
          value={newUserUsername}
          onChange={(e) => {
            setNewUserUsername(e.target.value);
          }}
        />
        <TextField
          label="Password"
          type="password"
          fullWidth
          margin="normal"
          value={newUserPassword}
          onChange={(e) => {
            setNewUserPassword(e.target.value);
          }}
        />
        <Select
          label="Role"
          fullWidth
          style={{ marginTop: "16px" }}
          value={newUserRole}
          onChange={(e) => {
            setNewUserRole(e.target.value);
          }}
        >
          <MenuItem value={Role.Viewer}>Viewer</MenuItem>
          <MenuItem value={Role.Poster}>Poster</MenuItem>
        </Select>
        {message && <div className="text-red-500 mb-4">{message}</div>}
      </DialogContent>
      <DialogActions>
        <Button onClick={handleCancel}>Cancel</Button>
        <Button
          onClick={handlePost}
          variant="contained"
          color="primary"
          disabled={false}
        >
          {isEdit ? "Update" : "Create"}
        </Button>
      </DialogActions>
    </Dialog>
  );
}
