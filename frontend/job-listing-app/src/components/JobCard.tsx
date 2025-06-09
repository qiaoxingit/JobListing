import {
  AccountCircle,
  Delete,
  Edit,
  Favorite,
  FavoriteBorder,
} from "@mui/icons-material";
import {
  Avatar,
  Card,
  CardContent,
  Divider,
  IconButton,
  List,
  ListItem,
  ListItemAvatar,
  ListItemText,
  Tooltip,
  Typography,
} from "@mui/material";
import { useState, type JSX } from "react";
import { apiClient } from "../api/ApiClient";
import type { Job } from "../contracts/Job";
import { Role, type User } from "../contracts/User";
import JobEditDialog from "./JobEdit";

export default function JobCard({
  job,
  liked,
  role,
  onUpdate,
}: Readonly<{
  job: Job;
  liked: boolean;
  role: Role | null;
  onUpdate: () => void;
}>) {
  const [open, setOpen] = useState(false);
  const [likedUsers, setLikedUsers] = useState<User[]>([]);

  const userId = localStorage.getItem("userId");
  const isMyJob = userId !== null && job.postedByUser === userId;

  const handleLike = async () => {
    liked = !liked;
    await apiClient.get<Job>(
      `/job/job/MarkInterestedJob?userId=${userId}&jobId=${job.id}&like=${liked}`
    );
    onUpdate();
  };

  const handleLikedUsers = async () => {
    const response = await apiClient.get<User[]>(
      `/user/user/GetLikedUsers?jobId=${job.id}`
    );

    setLikedUsers(response.data);
  };

  const handleEdit = () => {
    setOpen(true);
    onUpdate();
  };

  const handleDelete = async () => {
    await apiClient.delete<Job>(`/job/job/DeleteJob?jobId=${job.id}`);
    onUpdate();
  };

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
        <Tooltip title="Liked Users">
          <IconButton onClick={handleLikedUsers} aria-label="view liked users">
            <AccountCircle />
          </IconButton>
        </Tooltip>
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
    <>
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

          {likedUsers?.map((user: User) => (
            <List key={user.id}>
              <ListItem alignItems="flex-start">
                <ListItemAvatar>
                  <Avatar />
                </ListItemAvatar>
                <ListItemText
                  primary={user.firstName + " " + user.lastName}
                  secondary={
                    <Typography component="span" variant="body2">
                      {user.email}
                    </Typography>
                  }
                />
              </ListItem>
              <Divider variant="inset" component="li" />
            </List>
          ))}
        </CardContent>
      </Card>
      <JobEditDialog
        title="Edit Job"
        open={open}
        onClose={() => setOpen(false)}
        onSuccess={onUpdate}
        userId={userId}
        job={job}
        isEdit={true}
      />
    </>
  );
}
