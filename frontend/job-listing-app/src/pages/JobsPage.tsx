import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  TextField,
} from "@mui/material";
import Pagination from "@mui/material/Pagination";
import { useQuery } from "@tanstack/react-query";
import { useState } from "react";
import { apiClient } from "../api/ApiClient";
import JobCard from "../components/JobCard";
import type { Job } from "../contracts/Job";
import type { PagedResult } from "../contracts/PagedResult";
import { Role } from "../contracts/User";

export default function JobsPage() {
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const pageSize = 5;

  const rawRole = localStorage.getItem("role");
  const role = rawRole !== null ? (Number(rawRole) as Role) : null;

  const [open, setOpen] = useState(false);
  const [newJobTitle, setNewJobTitle] = useState("");
  const [newJobDescription, setNewJobDescription] = useState("");

  const {
    data: jobs,
    isLoading,
    isError,
    error,
    refetch,
  } = useQuery<PagedResult<Job>>({
    queryKey: ["jobs", page, pageSize],
    queryFn: () => fetchJobs((page - 1) * pageSize, pageSize),
  });

  const fetchJobs = async (
    skip: number,
    take: number
  ): Promise<PagedResult<Job>> => {
    const response = await apiClient.get<PagedResult<Job>>(
      `/job/job/GetPaged?skip=${skip}&take=${take}`
    );

    setTotalPages(Math.ceil(response.data.totalCount / take));

    return response.data;
  };

  const handlePageChange = (_: React.ChangeEvent<unknown>, value: number) => {
    setPage(value);
  };

  const handleOpen = () => setOpen(true);
  const handleClose = () => {
    setOpen(false);
    setNewJobTitle("");
    setNewJobDescription("");
  };

  const handlePost = async () => {
    const userId = localStorage.getItem("userId");
    try {
      await apiClient.post<Job>("/job/job/CreateJob", {
        title: newJobTitle,
        description: newJobDescription,
        postedByUser: userId,
      } as Job);
      handleClose();
      refetch();
    } catch (error) {
      alert("Job creation failed" + error);
    }
  };

  if (isLoading) {
    return <div className="p-4">Loading jobs...</div>;
  }
  if (isError) {
    return <div className="p-4 text-red-500">Error: {error.message}</div>;
  }

  return (
    <>
      {role === Role.Poster && (
        <div className="flex justify-end px-4 mt-4">
          <Button variant="contained" color="primary" onClick={handleOpen}>
            Post
          </Button>
        </div>
      )}

      <div className="p-4 space-y-4">
        {jobs?.items?.map((job: Job) => (
          <JobCard key={job.id} job={job} role={role} onUpdate={refetch} />
        ))}
      </div>

      <div className="flex justify-center mt-4">
        <Pagination
          count={totalPages}
          page={page}
          onChange={handlePageChange}
          color="primary"
          shape="rounded"
        />
      </div>

      <Dialog open={open} onClose={handleClose} fullWidth>
        <DialogTitle>Post a New Job</DialogTitle>
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
          <Button onClick={handleClose}>Cancel</Button>
          <Button
            onClick={handlePost}
            variant="contained"
            color="primary"
            disabled={!newJobTitle.trim()}
          >
            Post
          </Button>
        </DialogActions>
      </Dialog>
    </>
  );
}
