import Pagination from "@mui/material/Pagination";
import { useQuery } from "@tanstack/react-query";
import { useState } from "react";
import { apiClient } from "../api/ApiClient";
import JobCard from "../components/JobCard";
import type { Job } from "../contracts/Job";
import type { PagedResult } from "../contracts/PagedResult";

export default function JobsPage() {
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const pageSize = 5;

  const {
    data: jobs,
    isLoading,
    isError,
    error,
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

  if (isLoading) {
    return <div className="p-4">Loading jobs...</div>;
  }
  if (isError) {
    return <div className="p-4 text-red-500">Error: {error.message}</div>;
  }

  return (
    <>
      <div className="p-4 space-y-4">
        {jobs?.items?.map((job: Job) => (
          <JobCard key={job.id} job={job} />
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
    </>
  );
}
