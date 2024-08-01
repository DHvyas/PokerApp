import React, { useEffect, useState } from 'react';
import api from '../API';
import { useAuth } from '../components/AuthContext';
import { useNavigate } from 'react-router-dom';
import { Container, Box, Grid, Card, CardContent, CardActions, Button, Typography, Dialog, DialogTitle, DialogContent, DialogActions, TextField } from '@mui/material';

const Dashboard = () => {
    const { authState } = useAuth();
    const navigate = useNavigate();
    const [games, setGames] = useState([]);
    const [joinedGames, setJoinedGames] = useState([]);
    const [initialChips, setInitialChips] = useState(0);
    const [createDialogOpen, setCreateDialogOpen] = useState(false);
    const [open, setOpen] = useState(false);
    const [selectedGame, setSelectedGame] = useState(null);
    const [newGameName, setNewGameName] = useState('');
    const fetchGames = async () => {
        try {
            var userId = authState.userID;
            if (userId === null) {
                userId = localStorage.getItem('userID');
            }
            if (userId === null) {
                navigate('/login');
            }
            console.log(authState);
            const response = await api.get(`/api/Game/getAll?userId=${userId}`);
            setGames(response.data.allGames);
            setJoinedGames(response.data.gamesJoined);
            console.log(response.data); // Log the response data
        } catch (e) {
            console.log(e);
        }
    };
    useEffect(() => {
        fetchGames();
    }, []);
    const handleJoinGameClick = (game) => {
        setSelectedGame(game);
        setOpen(true);
    };
    const handleClose = () => {
        setOpen(false);
        setSelectedGame(null);
        setInitialChips(0);
    };
    const handleCreateGameClick = () => {
        setCreateDialogOpen(true);
    };
    const handleCreateDialogClose = () => {
        setCreateDialogOpen(false);
        setNewGameName('');
    };
    const handleCreateGame = async () => {
        const request = {
            GameName: newGameName
        };
        try {
            const response = await api.post('/api/Game/create', request);
            if (response.status === 200) {
                const joinRequest = {
                    GameId: response.data.gameID,
                    InitialChips: initialChips,
                    UserId: authState.userID
                };
                const joinResponse = await api.post('/api/Game/join', joinRequest);
                if (joinResponse.status === 200)
                    navigate(`/game/${response.data.gameID}`);
            }
        } catch (e) {
            console.log(e);
        }
    };
        const joinGame = (gameId) => {
            const request = {
                GameId: gameId,
                InitialChips: initialChips,
                UserId: authState.userID
            };
            try {
                const response = api.post('/api/Game/join', request);
                    console.log(response.status);
                        navigate(`/game/${gameId}`);
                
                } catch (e) {
                    console.log(e);
                }
        }
    return (
        <Container component="main" maxWidth="md">
            <Box
                sx={{
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center',
                    mt: 4,
                }}
            >
                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'column',
                        alignItems: 'center',
                        mt: 4,
                        width: '100%'
                    }}
                >
                <Typography variant="h4" component="h1" gutterBottom>
                    {games?.length > 0 ? "Open Games" : "No Open Games"}
                </Typography>
                <Grid container spacing={4}>
                    {games?.map((game) => (
                        <Grid item key={game.gameID} xs={12} sm={6} md={4}>
                            <Card sx={{ height: '100%', display: 'flex', flexDirection: 'column' }}>
                                <CardContent sx={{ flexGrow: 1 }}>
                                    <Typography gutterBottom variant="h5" component="h2">
                                        {game.gameName}
                                    </Typography>
                                    <Typography>
                                        {game.status}
                                    </Typography>
                                    <Typography>
                                        {game.currentPlayersCount} Players
                                    </Typography>
                                </CardContent>
                                <CardActions>
                                    <Button size="small" color="primary" onClick={() => handleJoinGameClick(game)}>
                                        Join Game
                                    </Button>
                                </CardActions>
                            </Card>
                        </Grid>
                    ))}
                    </Grid>
                </Box>
                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'column',
                        alignItems: 'center',
                        mt: 4,
                        width: '100%'
                    }}
                >
                <Typography variant="h4" component="h1" gutterBottom>
                    {joinedGames?.length > 0 ? "Joined Games" : "No Joined Games"}
                </Typography>
                <Grid container spacing={4}>
                    {joinedGames?.map((game) => (
                        <Grid item key={game.gameID} xs={12} sm={6} md={4}>
                            <Card sx={{ height: '100%', display: 'flex', flexDirection: 'column' }}>
                                <CardContent sx={{ flexGrow: 1 }}>
                                    <Typography gutterBottom variant="h5" component="h2">
                                        {game.gameName}
                                    </Typography>
                                    <Typography>
                                        {game.status}
                                    </Typography>
                                    <Typography>
                                        {game.currentPlayersCount} Players
                                    </Typography>
                                </CardContent>
                                <CardActions>
                                    <Button size="small" color="primary" onClick={() =>  navigate(`/game/${game.gameID}`)}>
                                        Open Game
                                    </Button>
                                </CardActions>
                            </Card>
                        </Grid>
                    ))}
                    </Grid>
                </Box>
                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'column',
                        alignItems: 'center',
                        mt: 4,
                        width: '100%'
                    }}
                >
                <Button variant="contained" color="primary" onClick={handleCreateGameClick}>
                    Create New Game
                    </Button>
                </Box>
            </Box>

            <Dialog open={open} onClose={handleClose}>
                <DialogTitle>Join {selectedGame?.gameName}</DialogTitle>
                <DialogContent>
                    <TextField
                        autoFocus
                        margin="dense"
                        id="chips"
                        label="Initial Chips"
                        type="number"
                        fullWidth
                        variant="outlined"
                        value={initialChips}
                        onChange={(e) => setInitialChips(e.target.value)}
                    />
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleClose} color="primary">
                        Cancel
                    </Button>
                    <Button onClick={() => joinGame(selectedGame?.gameID)} color="primary">
                        Join Game
                    </Button>
                </DialogActions>
            </Dialog>
            <Dialog open={createDialogOpen} onClose={handleCreateDialogClose}>
                <DialogTitle>Create New Game</DialogTitle>
                <DialogContent>
                    <TextField
                        autoFocus
                        margin="dense"
                        id="game-name"
                        label="Game Name"
                        type="text"
                        fullWidth
                        variant="outlined"
                        value={newGameName}
                        onChange={(e) => setNewGameName(e.target.value)}
                    />
                    <TextField
                        autoFocus
                        margin="dense"
                        id="chips"
                        label="Initial Chips"
                        type="number"
                        fullWidth
                        variant="outlined"
                        value={initialChips}
                        onChange={(e) => setInitialChips(e.target.value)}
                    />
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleCreateDialogClose} color="primary">
                        Cancel
                    </Button>
                    <Button onClick={handleCreateGame} color="primary">
                        Create Game
                    </Button>
                </DialogActions>
            </Dialog>
        </Container>

           /* <div>
                <h1>Dashboard</h1>
                <ul>
                    {games.map(game => (
                        <li key={game.gameID}>
                            <span>Game Name: {game.gameName}</span>
                            <br></br>
                            <span>Staus: {game.status}</span>
                            <br></br>
                            <button onClick={() => setRenderInput(true)}>Enter Chips</button>
                            {renderInput ? <div>
                                <span>Enter Initial Chips:</span>
                                <input type="number" onChange={(e) => setInitialChips(e.target.value)}></input>
                                <button onClick={() => joinGame(game.gameID)}>Join Game</button>
                            </div>:null}
                        </li>
                    ))}
                </ul>

            </div>*/
        );
    };

export default Dashboard;