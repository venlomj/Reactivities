import { AccessTime, Place } from "@mui/icons-material";
import { Avatar, Box, Button, Card, CardContent, CardHeader, Chip, Divider, Typography } from "@mui/material";
import { Link } from "react-router";
import AvatarPopover from "../../../app/shared/components/AvatarPopover";
import formatDate from "../../../lib/util/util";

type Props = {
  activity: Activity;
};

export default function ActivityCard({ activity }: Props) {
  const label = activity.isHost ? "You are hosting" : "You are going";
  const color = activity.isHost ? "secondary" : activity.isGoing ? "warning" : "default";

  return (
    <Card elevation={3} sx={{ borderRadius: 3, maxWidth: "100%" }}>
      <Box display="flex" alignItems="center" justifyContent="space-between" flexWrap="wrap" p={{ xs: 1, sm: 2 }}>
        <CardHeader
          avatar={
            <Avatar
              src={activity.hostImageUrl}
              sx={{ height: { xs: 50, sm: 60, md: 80 }, width: { xs: 50, sm: 60, md: 80 } }}
              alt="image of host"
            />
          }
          title={activity.title}
          titleTypographyProps={{
            fontWeight: "bold",
            fontSize: { xs: 16, sm: 18, md: 20 },
          }}
          subheader={
            <>
              Hosted by {" "}
              <Link to={`/profiles/${activity.hostId}`}>{activity.hostDisplayName}</Link>
            </>
          }
        />
        <Box display="flex" flexDirection="column" gap={1} mr={{ xs: 1, sm: 2 }}>
          {(activity.isHost || activity.isGoing) && (
            <Chip variant="outlined" label={label} color={color} sx={{ borderRadius: 2 }} />
          )}
          {activity.isCancelled && <Chip label="Cancelled" color="error" sx={{ borderRadius: 2 }} />}
        </Box>
      </Box>

      <Divider sx={{ mb: 2 }} />

      <CardContent sx={{ p: { xs: 1, sm: 2 } }}>
        <Box display="flex" alignItems="center" flexWrap="wrap" mb={2}>
          <Box display="flex" alignItems="center" gap={1} flexGrow={1}>
            <AccessTime sx={{ fontSize: { xs: 18, sm: 20 } }} />
            <Typography variant="body2" noWrap>
              {formatDate(activity.date)}
            </Typography>
          </Box>

          <Box display="flex" alignItems="center" gap={1} ml={{ xs: 0, sm: 3 }}>
            <Place sx={{ fontSize: { xs: 18, sm: 20 } }} />
            <Typography variant="body2" noWrap>
              {activity.venue}
            </Typography>
          </Box>
        </Box>
        
        <Divider />

        <Box display="flex" flexWrap="wrap" gap={1} sx={{ backgroundColor: "grey.200", py: 2, px: 2 }}>
          {activity.attendees.map((att) => (
            <AvatarPopover profile={att} key={att.id} />
          ))}
        </Box>
      </CardContent>

      <CardContent sx={{ pb: 2, display: "flex", justifyContent: { xs: "center", sm: "flex-end" } }}>
        <Typography variant="body2" mb={2} flexGrow={1}>
          {activity.description}
        </Typography>
        <Button
          component={Link}
          to={`/activities/${activity.id}`}
          size="medium"
          variant="contained"
          sx={{ width: { xs: "100%", sm: "auto" }, borderRadius: 3 }}
        >
          View
        </Button>
      </CardContent>
    </Card>
  );
}
