import { Favorite, FavoriteBorder } from "@mui/icons-material";
import {
  Card,
  CardContent,
  IconButton,
  Tooltip,
  Typography,
} from "@mui/material";
import { useState } from "react";
import type { Job } from "../contracts/Job";
import { Role } from "../contracts/User";

export default function JobCard({
  job,
  role,
}: Readonly<{ job: Job; role: Role | null }>) {
  const [liked, setLiked] = useState(false);

  const handleLike = () => {
    setLiked((prev) => !prev);
  };

  const userId = localStorage.getItem("userId");
  const isMyJob = userId !== null && job.postedByUser === userId;
  const cardBackgroundColor = isMyJob ? "bg-yellow-100" : "bg-white";

  return (
    <Card className="rounded-2xl shadow p-2">
      <CardContent>
        <div
          className={`flex justify-between items-start ${cardBackgroundColor}`}
        >
          <div className="flex-1">
            <Typography variant="h6">{job.title}</Typography>
            <Typography variant="body2">{job.description}</Typography>
            <Typography className="text-xs text-gray-500 mt-2">
              Posted on {job.postedDate} | Expires on {job.expireDate}
            </Typography>
          </div>
          {role === Role.Viewer ? (
            <Tooltip title={liked ? "Unlike" : "Like"}>
              <IconButton onClick={handleLike} aria-label="like job">
                {liked ? <Favorite color="error" /> : <FavoriteBorder />}
              </IconButton>
            </Tooltip>
          ) : (
            <></>
          )}
        </div>
      </CardContent>
    </Card>
  );
}
