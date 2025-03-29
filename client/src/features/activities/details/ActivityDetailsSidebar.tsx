import {
  Paper,
  Typography,
  List,
  ListItem,
  Chip,
  ListItemAvatar,
  Avatar,
  ListItemText,
  Grid,
} from "@mui/material";
import { Link } from "react-router";

type Props = {
  activity: Activity;
};

export default function ActivityDetailsSidebar({ activity }: Props) {
  return (
    <>
      {/* Header Section */}
      <Paper
        sx={{
          textAlign: "center",
          border: "none",
          backgroundColor: "primary.main",
          color: "white",
          p: 2,
        }}
      >
        <Typography variant="h6">
          {activity.attendees.length} people going
        </Typography>
      </Paper>

      {/* Attendees Section */}
      <Paper sx={{ padding: 2 }}>
        {activity.attendees.length === 0 ? (
          <Typography variant="body2">No attendees yet.</Typography>
        ) : (
          activity.attendees.map((attendee) => (
            <Grid
              key={attendee.id}
              container
              alignItems="center"
              spacing={2}
              sx={{ mb: 2 }}
            >
              {/* Avatar & Info */}
              <Grid item xs={8}>
                <List disablePadding>
                  <ListItem component={Link} to={`/profiles/${attendee.id}`}>
                    <ListItemAvatar>
                      <Avatar
                        variant="rounded"
                        alt={`${attendee.displayName} image`}
                        src={attendee.imageUrl}
                        sx={{
                          width: { xs: 50, sm: 75 },
                          height: { xs: 50, sm: 75 },
                          mr: 2,
                        }}
                      />
                    </ListItemAvatar>
                    <ListItemText
                      primary={
                        <Typography variant="h6" noWrap>
                          {attendee.displayName}
                        </Typography>
                      }
                      secondary={
                        attendee.following && (
                          <Typography variant="body2" color="orange">
                            Following
                          </Typography>
                        )
                      }
                    />
                  </ListItem>
                </List>
              </Grid>

              {/* Host Badge */}
              <Grid
                item
                xs={4}
                sx={{
                  display: "flex",
                  justifyContent: "flex-end",
                  alignItems: "center",
                }}
              >
                {activity.hostId === attendee.id && (
                  <Chip
                    label="Host"
                    color="warning"
                    variant="filled"
                    sx={{ borderRadius: 2 }}
                  />
                )}
              </Grid>
            </Grid>
          ))
        )}
      </Paper>
    </>
  );
}
