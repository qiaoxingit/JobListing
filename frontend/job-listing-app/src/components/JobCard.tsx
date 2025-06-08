import { Card, CardContent, Typography } from "@mui/material";
import type { Job } from "../contracts/Job";

export default function JobCard({ job }: Readonly<{ job: Job }>) {
  return (
    <Card className="rounded-2xl shadow p-2">
      <CardContent>
        <Typography variant="h6">{job.title}</Typography>
        <Typography variant="body2">{job.description}</Typography>
        <Typography className="text-xs text-gray-500 mt-2">
          Posted on {job.postedDate} | Expires on {job.expireDate}
        </Typography>
      </CardContent>
    </Card>
  );
}
