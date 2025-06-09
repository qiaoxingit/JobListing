import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  TextField,
} from "@mui/material";
import { useState } from "react";
import { apiClient } from "../api/ApiClient";
import type { Job } from "../contracts/Job";

export default function JobEditDialog({
  title,
  open,
  isEdit,
  onClose,
  onSuccess,
  userId,
  job,
}: Readonly<{
  title: string;
  open: boolean;
  isEdit: boolean;
  onClose: () => void;
  onSuccess: () => void;
  userId: string | null;
  job?: Job | null;
}>) {
  const [newJobTitle, setNewJobTitle] = useState(job ? job.title : "");
  const [newJobDescription, setNewJobDescription] = useState(
    job ? job.description : ""
  );

  const handlePost = async () => {
    try {
      await apiClient.post<Job>("/job/job/CreateJob", {
        title: newJobTitle,
        description: newJobDescription,
        postedByUser: userId,
      });
      setNewJobTitle("");
      setNewJobDescription("");
      onClose();
      onSuccess();
    } catch (error) {
      alert("Job creation failed: " + error);
    }
  };

  const handleEdit = async () => {
    try {
      await apiClient.post<Job>("/job/job/UpdateJob", {
        id: job?.id,
        title: newJobTitle,
        description: newJobDescription,
      });
      setNewJobTitle("");
      setNewJobDescription("");
      onClose();
      onSuccess();
    } catch (error) {
      alert("Job edit failed: " + error);
    }
  };

  const handleCancel = () => {
    setNewJobTitle("");
    setNewJobDescription("");
    onClose();
  };

  return (
    <Dialog open={open} onClose={handleCancel} fullWidth>
      <DialogTitle>{title}</DialogTitle>
      <DialogContent className="space-y-4 mt-2">
        <TextField
          label="Title"
          fullWidth
          margin="normal"
          value={newJobTitle}
          onChange={(e) => setNewJobTitle(e.target.value)}
        />
        <TextField
          label="Description"
          fullWidth
          multiline
          minRows={3}
          margin="normal"
          value={newJobDescription}
          onChange={(e) => setNewJobDescription(e.target.value)}
        />
      </DialogContent>
      <DialogActions>
        <Button onClick={handleCancel}>Cancel</Button>
        <Button
          onClick={isEdit ? handleEdit : handlePost}
          variant="contained"
          color="primary"
          disabled={!newJobTitle.trim()}
        >
          {isEdit ? "Update" : "Post"}
        </Button>
      </DialogActions>
    </Dialog>
  );
}
