import { SyntheticEvent, useEffect, useState } from "react";
import {
  Box,
  Card,
  CardContent,
  CardMedia,
  Tab,
  Tabs,
  Typography,
} from "@mui/material";
import Grid from "@mui/material/Grid2";
import { Link, useParams } from "react-router";
import { format } from "date-fns";
import { useProfile } from "../../lib/hooks/useProfile.ts";


export default function ProfileActivities() {
  const [activeTab, setActiveTab] = useState(0);
  const { id } = useParams();
  const { userActivities, setFilter, loadingUserActivities } = useProfile(id);

  useEffect(() => {
    setFilter("future");
  }, [setFilter]);

  const tabs = [
    { menuItem: "Future Events", key: "future" },
    { menuItem: "Past Events", key: "past" },
    { menuItem: "Hosting", key: "hosting" },
  ];

  const handleTabChange = (_: SyntheticEvent, newValue: number) => {
    setActiveTab(newValue);
    setFilter(tabs[newValue].key);
  };

  return (
    <Box>
      <Grid container spacing={2}>
        <Grid size={{ xs: 12, sm: 12, md: 12, lg: 12 }}>
          <Tabs value={activeTab} onChange={handleTabChange}>
            {tabs.map((tab, index) => (
              <Tab label={tab.menuItem} key={index} />
            ))}
          </Tabs>
        </Grid>
      </Grid>
      {(!userActivities || userActivities.length === 0) &&
      !loadingUserActivities ? (
        <Typography mt={2}>No activities to show</Typography>
      ) : null}
      <Grid
        container
        spacing={2}
        sx={{ marginTop: 2, height: 400, overflow: "auto" }}
      >
        {userActivities &&
          userActivities.map((activity: Activity) => (
            <Grid size={{ xs: 12, sm: 6, md: 4, lg: 2 }} key={activity.id}>
              <Link
                to={`/activities/${activity.id}`}
                style={{ textDecoration: "none" }}
              >
                <Card elevation={4}>
                  <CardMedia
                    component="img"
                    height="100"
                    image={`/images/categoryImages/${activity.category}.jpg`}
                    alt={activity.title}
                    sx={{ objectFit: "cover" }}
                  />
                  <CardContent>
                    <Typography variant="h6" textAlign="center" mb={1}>
                      {activity.title}
                    </Typography>
                    <Typography
                      variant="body2"
                      textAlign="center"
                      display="flex"
                      flexDirection="column"
                    >
                      <span>{format(activity.date, "do LLL yyyy")}</span>
                      <span>{format(activity.date, "h:mm a")}</span>
                    </Typography>
                  </CardContent>
                </Card>
              </Link>
            </Grid>
          ))}
      </Grid>
    </Box>
  );
}
