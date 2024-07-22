import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { useAuth } from './AuthContext';
import api from '../API';
import PlayingCard from './PlayingCard';
import { Box, Typography, Button, CircularProgress, List, Grid, Card, CardContent, Avatar } from '@mui/material';

const Game = () => {
    const { gameId } = useParams();
    const { authState } = useAuth();
    const [game, setGame] = useState(null);
    const [coins, setCoins] = useState(0);
    const [users, setUsers] = useState([]);
    const [round, setRound] = useState('');
    const [loading, setLoading] = useState(true);
    const [gameStarted, setGameStarted] = useState(false);
    const [cards, setCards] = useState(null);
    const [communityCards, setCommunityCards] = useState(null);
    const [actionResponse, setActionResponse] = useState(null);

    useEffect(() => {
        getGameStatus();
        fetchCards();
    }, []);

    const getGameStatus = async () => {
        try {
            const response = await api.get(`/api/game/get?gameId=${gameId}`);
            if (response.data.game.status === "Started") setGameStarted(true);
            setGame(response.data.game);
            setRound(response.data.round);
            setUsers(response.data.users);
            setLoading(false);
        } catch (error) {
            console.error('Failed to get game status:', error);
        }
    };

    const startGame = async () => {
        try {
            const request = { gameId: parseInt(gameId), userId: authState.userID };
            const response = await api.post('/api/Game/start', request);
            if (response.status !== 404) setGameStarted(true);
            else console.error('Failed to start game');
        } catch (error) {
            console.error('Failed to start game:', error);
        }
    };

    const fetchCards = async () => {
        try {
            const response = await api.get(`/api/game/fetchCards?gameId=${gameId}&userId=${authState.userID}`);
            if (response.status === 200) {
                const { card1, card2, gamePlayer: { chips } } = response.data.hand;
                setCommunityCards(response.data.communityCards);
                setCoins(chips);
                setCards([card1, card2]);
            } else console.error('Failed to fetch cards');
        } catch (error) {
            console.error('Failed to fetch cards:', error);
        }
    };

    const performAction = async (actionType, amount) => {
        try {
            const request = { gameId: parseInt(gameId), userId: authState.userID, actionType, amount };
            const response = await api.post('api/game/performAction', request);
            if (response.status === 200) {
                getGameStatus();
                fetchCards();
                const data = await response.json();
                setActionResponse(data);
            } else console.error('Failed to perform action');
        } catch (error) {
            console.error('Failed to perform action:', error);
        }
    };

    if (loading) {
        return (
            <Box display="flex" justifyContent="center" alignItems="center" height="100vh">
                <CircularProgress />
            </Box>
        );
    }

    return (
        <Box p={3}>
            <Card>
                <CardContent>
                    <Typography variant="h4" gutterBottom>Game Name: {game?.gameName}</Typography>
                    {!gameStarted ? (
                        <Button variant="contained" color="primary" onClick={startGame}>Start Game</Button>
                    ) : (
                        <Box>
                            <Box mt={3} mb={3} display="flex" justifyContent="space-between" alignItems="center">
                                <Typography variant="h6">Your Coins: {coins}</Typography>
                                <Typography variant="h6">Game Status: {game?.status}</Typography>
                                <Typography variant="h6">Game Round: {round?.roundName}</Typography>
                                <Typography variant="h6">Game Pot: {game?.potAmount}</Typography>
                            </Box>
                            <Box mb={3}>
                                <Typography variant="h6">Users:</Typography>
                                <List>
                                    {users.map((user, index) => (
                                        <Grid container spacing={2} key={index}>
                                            <Grid item>
                                                <Avatar alt={user.email} src={user.profilePicture} />
                                            </Grid>
                                            <Grid item>
                                                <Typography>{user.email}</Typography>
                                                {user.userID === game.currentTurnUserID && <Typography color="primary">Current Turn</Typography>}
                                            </Grid>
                                        </Grid>
                                    ))}
                                </List>
                            </Box>
                            <Button variant="contained" color="secondary" onClick={fetchCards}>Fetch Cards</Button>
                            {cards && (
                                <Box mt={3} mb={3}>
                                    <Typography variant="h6">Your Cards:</Typography>
                                    <List className="flex">
                                        {cards.map((card, index) => (
                                                <PlayingCard key={index} card={card} />
                                        ))}
                                    </List>
                                </Box>
                            )}
                            {communityCards && (
                                <Box mb={3}>
                                    <Typography variant="h6">Community Cards:</Typography>
                                    <List className="flex">
                                            {Object.values(communityCards).map((card, index) => (
                                                card?.length > 0 && (<PlayingCard key={ index} card={card} />)
                                        ))}
                                    </List>
                                </Box>
                                )}
                                {authState.userID === game.currentTurnUserID ? (
                            <Box>
                                <Typography variant="h6">Perform Action</Typography>
                                <Button variant="contained" color="primary" onClick={() => performAction('Bet', 100)}>Bet 100</Button>
                                <Button variant="contained" color="secondary" onClick={() => performAction('Fold', 0)} sx={{ ml: 2 }}>Fold</Button>
                                <Button variant="contained" color="secondary" onClick={() => performAction('Check', 0)} sx={{ ml: 2 }}>Check</Button>
                                    </Box>) : (
                                <Typography variant="h6">Waiting for other players to perform action</Typography>
                            )}
                            {actionResponse && (
                                <Box mt={3}>
                                    <Typography variant="h6">Action Response:</Typography>
                                    <Typography>{actionResponse.message}</Typography>
                                </Box>
                            )}
                        </Box>
                    )}
                </CardContent>
            </Card>
        </Box>
    );
};

export default Game;
