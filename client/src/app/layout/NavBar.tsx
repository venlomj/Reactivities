import { Group, Menu as MenuIcon } from "@mui/icons-material";
import {
  Box,
  AppBar,
  Toolbar,
  Typography,
  Container,
  MenuItem,
  CircularProgress,
  useMediaQuery,
  useTheme,
  IconButton,
  Drawer,
  List,
  ListItem,
  ListItemText,
  ListItemButton
} from "@mui/material";
import { NavLink } from "react-router";
import MenuItemLink from "../shared/components/MenuItemLink";
import useStore from "../../lib/hooks/useStore";
import { Observer } from "mobx-react-lite";
import { useAccount } from "../../lib/hooks/useAccount";
import UserMenu from "./UserMenu";
import { useState } from "react";

export default function NavBar() {
  const { uiStore } = useStore();
  const { currentUser } = useAccount();

  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down("sm"));

  const [drawerOpen, setDrawerOpen] = useState(false);

  const toggleDrawer = () => {
    setDrawerOpen(!drawerOpen);
  };

  const menuItems = (
    <Box sx={{ display: "flex" }}>
      <MenuItemLink to="/activities">Activities</MenuItemLink>
      <MenuItemLink to="/counter">Counter</MenuItemLink>
      <MenuItemLink to="/errors">Errors</MenuItemLink>
    </Box>
  );

  const mobileMenuItems = (
    <List>
  <ListItem>
    <ListItemButton component={NavLink} to="/activities">
      <ListItemText primary="Activities" />
    </ListItemButton>
  </ListItem>
  <ListItem>
    <ListItemButton component={NavLink} to="/counter">
      <ListItemText primary="Counter" />
    </ListItemButton>
  </ListItem>
  <ListItem>
    <ListItemButton component={NavLink} to="/errors">
      <ListItemText primary="Errors" />
    </ListItemButton>
  </ListItem>
</List>
  );

  return (
    <Box sx={{ flexGrow: 1 }}>
      <AppBar
        position="fixed"
        sx={{
          backgroundImage:
            "linear-gradient(135deg, #0D1B2A 0%, #1B263B 50%, #415A77 100%)",
        }}
      >
        <Container maxWidth="xl">
          <Toolbar sx={{ display: "flex", justifyContent: "space-between" }}>
            <Box sx={{ display: "flex", alignItems: "center" }}>
              <MenuItem
                component={NavLink}
                to="/"
                sx={{ display: "flex", gap: 2 }}
              >
                <Group fontSize="large" />
                <Typography
                  sx={{ position: "relative" }}
                  variant="h4"
                  fontWeight="bold"
                >
                  Reactivities
                </Typography>
                <Observer>
                  {() =>
                    uiStore.isLoading ? (
                      <CircularProgress
                        size={20}
                        thickness={7}
                        sx={{
                          color: "white",
                          position: "absolute",
                          top: "30%",
                          left: "105%",
                        }}
                      />
                    ) : null
                  }
                </Observer>
              </MenuItem>
            </Box>
            {isMobile ? (
              <Box>
                <IconButton
                  color="inherit"
                  aria-label="menu"
                  onClick={toggleDrawer}
                  edge="end"
                >
                  <MenuIcon />
                </IconButton>
              </Box>
            ) : (
              menuItems
            )}
            <Box display="flex" alignItems="center">
              {currentUser ? (
                <UserMenu />
              ) : (
                <>
                  <MenuItemLink to="/login">Login</MenuItemLink>
                  <MenuItemLink to="/register">Register</MenuItemLink>
                </>
              )}
            </Box>
          </Toolbar>
        </Container>
      </AppBar>

      {/* Drawer for mobile */}
      <Drawer anchor="right" open={drawerOpen} onClose={toggleDrawer}>
        <Box sx={{ width: 250 }} role="presentation" onClick={toggleDrawer}>
          {mobileMenuItems}
        </Box>
      </Drawer>
    </Box>
  );
}
