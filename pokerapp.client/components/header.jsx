import React from 'react';
import { AppBar, Toolbar, Typography, Button, Box } from '@mui/material';
import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from './AuthContext';

const Header = () => {
    const { authState, logout } = useAuth();
    const handleLogout = () => {
        logout();
        window.location.reload();
    }
    const navigate = useNavigate();
    useEffect(() => {
        
        //skip if current page is login or signup
        if (window.location.pathname === '/login' || window.location.pathname === '/') {
            return;
        }
        const token = localStorage.getItem('token');
        console.log(token);
        if (token === null) {
            navigate('/');
        }
    }, []);
    return (
        <AppBar position="static" className="header">
            {console.log(authState) }
            <Toolbar>
                <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
                    Poker App
                </Typography>
                <Box sx={{ display: 'flex' }}>
                    {authState.token ? (<><Button color="inherit" href="/profile">{authState.userName}</Button>
                        <Button color="inherit" href="/dashboard">All Games</Button>
                        <Button color="inherit" onClick={() => handleLogout()}>Logout</Button></>) :
                        <Button color="inherit" href="/login">Login</Button>}
                    <Button color="inherit">About</Button>
                </Box>
            </Toolbar>
        </AppBar>
    );
};

export default Header;
