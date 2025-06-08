import Pagination from "@mui/material/Pagination";
import { useQuery } from "@tanstack/react-query";
import { useState } from "react";
import { apiClient } from "../api/ApiClient";
import JobCard from "../components/JobCard";
import type { Job } from "../contracts/Job";

export default function JobsPage() {
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(10);
  const pageSize = 2;

  const {
    data: jobs,
    isLoading,
    isError,
    error,
  } = useQuery<Job[]>({
    queryKey: ["jobs", page, pageSize],
    queryFn: () => fetchJobs((page - 1) * pageSize, pageSize),
  });

  const fetchJobs = async (skip: number, take: number): Promise<Job[]> => {
    const response = await apiClient.get<Job[]>(
      `/job/job/GetPaged?skip=${skip}&take=${take}`
    );
    setTotalPages(10);
    return response.data;
  };

  const handlePageChange = (_: React.ChangeEvent<unknown>, value: number) => {
    setPage(value);
  };

  if (isLoading) {
    return <div className="p-4">Loading jobs...</div>;
  }
  if (isError) {
    return <div className="p-4 text-red-500">Error: {error.message}</div>;
  }

  return (
    <>
      <div className="p-4 space-y-4">
        {jobs?.map((job: Job) => (
          <JobCard key={job.id} job={job} />
        ))}
      </div>
      <div className="flex justify-center mt-4">
        <Pagination
          count={totalPages}
          page={page}
          onChange={handlePageChange}
          color="primary"
        />
      </div>
    </>
  );
}
