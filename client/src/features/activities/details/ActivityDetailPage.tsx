import { Grid, Typography } from "@mui/material";
import { useParams } from "react-router";
import { useActivities } from "../../../lib/hooks/useActivities";
import ActivityDetailsHeader from "./ActivityDetailsHeader";
import ActivityDetailsInfo from "./ActivityDetailsInfo";
import ActivityDetailsChat from "./ActivityDetailsChat";
import ActivityDetailsSidebar from "./ActivityDetailsSidebar";

export default function ActivityDetailPage() {
  const { id } = useParams();
  const { activity, isLoadingActivity } = useActivities(id);

  if (isLoadingActivity) return <Typography>Loading...</Typography>;

  if (!activity) return <Typography>Activity not found</Typography>;

  return (
    <Grid container spacing={3}>
      {/* Main Content - Adjusts for all screen sizes */}
      <Grid item xs={12} md={8}>
        <ActivityDetailsHeader activity={activity} />
        <ActivityDetailsInfo activity={activity} />
        <ActivityDetailsChat />
      </Grid>

      {/* Sidebar - Stacks below on small screens */}
      <Grid
        item
        xs={12}
        md={4}
        sx={{
          overflowY: "auto",
          maxHeight: { xs: "none", md: "calc(100vh - 64px)" },
        }}
      >
        <ActivityDetailsSidebar activity={activity} />
      </Grid>
    </Grid>
  );
}
