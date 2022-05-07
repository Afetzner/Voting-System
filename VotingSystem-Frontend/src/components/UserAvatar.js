import { Avatar } from "@mui/material";

const color = (name) => {
  let hash = 0, color = "#";
  for (let i = 0; i < name.length; i++) {
    hash = name.charCodeAt(i) + ((hash << 5) - hash);
  }
  for (let i = 0; i < 3; i++) {
    const value = (hash >> (i * 8)) & 0xff;
    color += `00${value.toString(16)}`.slice(-2);
  }
  return color;
};

export default function UserAvatar(props) {
  if (props.user === null) {
    return (
      <Avatar />
    );
  } else {
    return (
      <Avatar sx={{bgcolor: color(props.user.firstName + " " + props.user.lastName)}}>
        {`${props.user.firstName[0]}${props.user.lastName[0]}`}
      </Avatar>
    );
  }
}