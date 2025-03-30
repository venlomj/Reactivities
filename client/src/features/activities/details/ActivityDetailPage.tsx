import { Typography } from "@mui/material";
import Grid from "@mui/material/Grid2";
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
            <Grid size={{ xs: 12, sm: 12, md: 8, lg: 8 }}>
                <ActivityDetailsHeader activity={activity} />
                <ActivityDetailsInfo activity={activity} />
                <ActivityDetailsChat />
            </Grid>
            <Grid size={{ xs: 12, sm: 12, md: 4, lg: 4 }}>
                <ActivityDetailsSidebar activity={activity} />
            </Grid>
        </Grid>
    );
}
