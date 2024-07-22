import React from 'react';
import { AppBar, Toolbar, Typography, Button, Box } from '@mui/material';
import MenuIcon from '@mui/icons-material/Menu';
import { useAuth } from './AuthContext';

const Header = () => {
    const { authState } = useAuth();
    return (
        <AppBar position="static" className="header">
            {console.log(authState) }
            <Toolbar>
                <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
                    Poker App
                </Typography>
                <Box sx={{ display: 'flex' }}>
                    {authState.token ? (<><Button color="inherit" href="/profile">{authState.userName}</Button>
                        <Button color="inherit" href="/dashboard">All Games</Button></>) :
                        <Button color="inherit" href="/login">Login</Button>}
                    <Button color="inherit">About</Button>
                </Box>
            </Toolbar>
        </AppBar>
    );
};

export default Header;
