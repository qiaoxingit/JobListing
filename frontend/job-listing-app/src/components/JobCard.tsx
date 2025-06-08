import { Delete, Edit, Favorite, FavoriteBorder } from "@mui/icons-material";
import {
  Card,
  CardContent,
  IconButton,
  Tooltip,
  Typography,
} from "@mui/material";
import { useState, type JSX } from "react";
import type { Job } from "../contracts/Job";
import { Role } from "../contracts/User";

export default function JobCard({
  job,
  role,
  onUpdate,
}: Readonly<{ job: Job; role: Role | null; onUpdate: () => void }>) {
  const [liked, setLiked] = useState(false);

  const handleLike = () => {
    setLiked((prev) => !prev);
    onUpdate();
  };

  const handleEdit = () => {
    onUpdate();
  };

  const handleDelete = () => {
    onUpdate();
  };

  const userId = localStorage.getItem("userId");
  const isMyJob = userId !== null && job.postedByUser === userId;

  let actionButtons: JSX.Element | null = null;

  if (role === Role.Viewer) {
    actionButtons = (
      <Tooltip title={liked ? "Unlike" : "Like"}>
        <IconButton onClick={handleLike} aria-label="like job">
          {liked ? <Favorite color="error" /> : <FavoriteBorder />}
        </IconButton>
      </Tooltip>
    );
  } else if (role === Role.Poster && isMyJob) {
    actionButtons = (
      <>
        <Tooltip title="Edit">
          <IconButton onClick={handleEdit} aria-label="edit job">
            <Edit />
          </IconButton>
        </Tooltip>
        <Tooltip title="Delete">
          <IconButton onClick={handleDelete} aria-label="delete job">
            <Delete />
          </IconButton>
        </Tooltip>
      </>
    );
  }

  return (
    <Card className="rounded-2xl shadow p-2">
      <CardContent>
        <div className="flex justify-between items-start">
          <div className="flex-1">
            <Typography variant="h6">{job.title}</Typography>
            <Typography variant="body2">{job.description}</Typography>
            <Typography className="text-xs text-gray-500 mt-2">
              Posted on {job.postedDate} | Expires on {job.expireDate}
            </Typography>
          </div>
          <div className="flex gap-1">{actionButtons}</div>
        </div>
      </CardContent>
    </Card>
  );
}
