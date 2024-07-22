import React from 'react';
import { Box, Container, Grid, Link, Typography } from '@mui/material';

const Footer = () => {
    return (
        <Box
            sx={{
                width: '100%',
                background: 'linear-gradient(45deg, #FE6B8B 30%, #FF8E53 90%)',
                color: 'white',
                mt: 8,
                py: 4,
            }}
        >
            <Container maxWidth="lg">
                <Grid container spacing={4}>
                    <Grid item xs={12} sm={4}>
                        <Typography variant="h6" component="div" gutterBottom>
                            MyApp
                        </Typography>
                        <Typography variant="body2">
                            © {new Date().getFullYear()} MyApp. All rights reserved.
                        </Typography>
                    </Grid>
                    <Grid item xs={12} sm={4}>
                        <Typography variant="h6" component="div" gutterBottom>
                            Quick Links
                        </Typography>
                        <Link href="#" color="inherit" variant="body2" display="block">
                            Home
                        </Link>
                        <Link href="#" color="inherit" variant="body2" display="block">
                            About
                        </Link>
                        <Link href="#" color="inherit" variant="body2" display="block">
                            Contact
                        </Link>
                    </Grid>
                    <Grid item xs={12} sm={4}>
                        <Typography variant="h6" component="div" gutterBottom>
                            Follow Us
                        </Typography>
                        <Link href="#" color="inherit" variant="body2" display="block">
                            Facebook
                        </Link>
                        <Link href="#" color="inherit" variant="body2" display="block">
                            Twitter
                        </Link>
                        <Link href="#" color="inherit" variant="body2" display="block">
                            Instagram
                        </Link>
                    </Grid>
                </Grid>
            </Container>
        </Box>
    );
};

export default Footer;
