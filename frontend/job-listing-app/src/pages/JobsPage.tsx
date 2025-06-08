import { useQuery } from "@tanstack/react-query";
import JobCard from "../components/JobCard";
import type { Job } from "../contracts/Job";
import { apiClient } from "../api/ApiClient";

export default function JobsPage() {
  const {
    data: jobs,
    isLoading,
    isError,
    error,
  } = useQuery<Job[]>({
    queryKey: ["jobs"],
    queryFn: fetchJobs,
  });

  if (isLoading) {
    return <div className="p-4">Loading jobs...</div>;
  }
  if (isError) {
    return <div className="p-4 text-red-500">Error: {error.message}</div>;
  }

  return (
    <div className="p-4 space-y-4">
      {jobs?.map((job: Job) => (
        <JobCard key={job.id} job={job} />
      ))}
    </div>
  );
}

async function fetchJobs(): Promise<Job[]> {
  const response = await apiClient.get<Job[]>("/job/job/GetUserInteredJobs?userId=550e8400-e29b-41d4-a716-446655440000");
  return response.data;
}
