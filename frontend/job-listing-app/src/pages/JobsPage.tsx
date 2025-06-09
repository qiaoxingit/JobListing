import { Button } from "@mui/material";
import Pagination from "@mui/material/Pagination";
import { useQuery } from "@tanstack/react-query";
import { useState } from "react";
import { apiClient } from "../api/ApiClient";
import JobCard from "../components/JobCard";
import JobEditDialog from "../components/JobEdit";
import type { Job } from "../contracts/Job";
import type { PagedResult } from "../contracts/PagedResult";
import { Role } from "../contracts/User";

export default function JobsPage() {
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const pageSize = 5;

  const userId = localStorage.getItem("userId");
  const rawRole = localStorage.getItem("role");
  const role = rawRole !== null ? (Number(rawRole) as Role) : null;

  const [open, setOpen] = useState(false);

  const { data: interestedJobs, refetch: refetchInterestedJobs } = useQuery<
    Job[]
  >({
    queryKey: ["interestedJobs", userId],
    queryFn: () => fetchInterestedJobs(userId),
  });

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

  const refretchAllQueries = () => {
    refetch();
    refetchInterestedJobs();
  };

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

  const fetchInterestedJobs = async (userId: string | null): Promise<Job[]> => {
    const response = await apiClient.get<Job[]>(
      `/job/job/GetUserInteredJobs?userId=${userId}`
    );

    return response.data;
  };

  const handlePageChange = (_: React.ChangeEvent<unknown>, value: number) => {
    setPage(value);
  };

  const handleOpen = () => {
    setOpen(true);
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
          <JobCard
            key={job.id}
            job={job}
            liked={(interestedJobs ?? []).some((j) => j.id === job.id)}
            role={role}
            onUpdate={refretchAllQueries}
          />
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

      <JobEditDialog
        title="Post Job"
        open={open}
        onClose={() => setOpen(false)}
        onSuccess={refretchAllQueries}
        userId={userId}
        isEdit={false}
      />
    </>
  );
}
