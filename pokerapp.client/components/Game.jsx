import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { useAuth } from './AuthContext';
import api from '../API';
import PlayingCard from './PlayingCard';
import {
    Box,
    Card,
    Typography,
    Button,
    CircularProgress,
    List,
    CardContent,
    Avatar,
    Badge,
    Chip,
    Divider,
    Paper,
    TextField
} from '@mui/material';

const Game = () => {
    const MAX_HAND_CARDS = 2;
    const { gameId } = useParams();
    const { authState } = useAuth();
    const [betAmount, setBetAmount] = useState('');
    const [game, setGame] = useState(null);
    const [coins, setCoins] = useState(0);
    const [users, setUsers] = useState([]);
    const [round, setRound] = useState('');
    const [loading, setLoading] = useState(true);
    const [gameStarted, setGameStarted] = useState(false);
    const [cards, setCards] = useState(null);
    const [communityCards, setCommunityCards] = useState([]);
    const [actionResponse, setActionResponse] = useState(null);
    const [isGameOver, setIsGameOver] = useState(false);
    const [winner, setWinner] = useState(null);

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
            console.log('AuthState', authState);
            const response = await api.get(`/api/game/fetchCards?gameId=${gameId}&userId=${authState.userID}`);
            if (response.status === 200) {
                const { card1, card2, gamePlayer: { chips } } = response.data.hand;
                communityCards[0] = response.data.communityCards?.card1;
                communityCards[1] = response.data.communityCards?.card2;
                communityCards[2] = response.data.communityCards?.card3;
                communityCards[3] = response.data.communityCards?.card4;
                communityCards[4] = response.data.communityCards?.card5;
                setCoins(chips);
                setCards([card1, card2]);
                setLoading(false);
            } else {
                const response = await api.get(`/api/game/fetchCards?gameId=${gameId}&userId=${authState.userID}`);
                if (response.status === 200) {
                    const { card1, card2, gamePlayer: { chips } } = response.data.hand;
                    communityCards[0] = response.data.communityCards.card1;
                    communityCards[1] = response.data.communityCards.card2;
                    communityCards[2] = response.data.communityCards.card3;
                    communityCards[3] = response.data.communityCards.card4;
                    communityCards[4] = response.data.communityCards.card5;
                    setCoins(chips);
                    setCards([card1, card2]);
                    setLoading(false);
                } else console.log('Failed to fetch cards');
            }
        } catch (error) {
            console.error('Failed to fetch cards:', error);
        }
    };

    const performAction = async (actionType, amount) => {
        try {
            const request = { gameId: parseInt(gameId), userId: authState.userID, actionType, amount };
            const response = await api.post('api/game/performAction', request);
            if (response.status === 200) {
                if (response.data.isGameOver === true) {
                    setIsGameOver(true);
                    setWinner(response.data.message);
                }
                getGameStatus();
                fetchCards();
                const data = await response.json();
                setActionResponse(data);
            } else console.error('Failed to perform action');
        } catch (error) {
            console.error('Failed to perform action:', error);
        }
    };
    const handleBetAmountChange = (event) => {
        setBetAmount(event.target.value);
    };

    if (loading) {
        return (
            <Box display="flex" justifyContent="center" alignItems="center" height="100vh">
                <CircularProgress />
            </Box>
        );
    }
        if (isGameOver) {
            return (
                <Box display="flex" justifyContent="center" alignItems="center" height="100vh">
                    <Typography variant="h4" color="#FF6F61">Game Over</Typography>
                    <Typography variant="h6" color="#3D9970">{winner}</Typography>
                </Box>
            );

        }
    return (
        <Box p={2} bgcolor="#f5f5f5" minHeight="100vh" display="flex" justifyContent="center" alignItems="center">
            <Paper elevation={3} sx={{ borderRadius: '16px', overflow: 'hidden', maxWidth: '800px', width: '100%', p: 3 }}>
                <CardContent>
                    <Typography variant="h4" gutterBottom align="center" color="#FF6F61">Game Name: {game?.gameName}</Typography>
                    {!gameStarted ? (
                        <Button variant="contained" color="primary" onClick={startGame} fullWidth sx={{ borderRadius: '9999px', mb: 2, backgroundColor: '#FF6F61' }}>
                            Start Game
                        </Button>
                    ) : (
                        <Box>
                            <Box mt={3} mb={3} display="flex" flex-wrap="wrap" alignItems="center" justifyContent="space-around">
                                <Typography variant="h6" color="#3D9970">Your Coins: <Chip label={coins} sx={{ backgroundColor: '#FF6F61', color: '#fff' }} /></Typography>
                                <Typography variant="h6" color="#3D9970">Game Status: <Chip label={game?.status} sx={{ backgroundColor: '#FF6F61', color: '#fff' }} /></Typography>
                                <Typography variant="h6" color="#3D9970">Game Round: <Chip label={round?.roundName} sx={{ backgroundColor: '#FF6F61', color: '#fff' }} /></Typography>
                                <Typography variant="h6" color="#3D9970">Game Pot: <Chip label={game?.potAmount} sx={{ backgroundColor: '#FF6F61', color: '#fff' }} /></Typography>
                            </Box>
                            <Divider sx={{ marginY: 2 }} />
                            <Box mb={3}>
                                <Typography variant="h6" color="#3D9970">Users:</Typography>
                                <List>
                                    {users.map((user, index) => (
                                        <Badge key={index} badgeContent={user.userID === game.currentTurnUserID ? 'Current Turn' : null} color="primary">
                                            <Card sx={{ display: 'flex', alignItems: 'center', padding: '10px', marginBottom: '10px',marginRight: '10px', borderRadius: '8px', backgroundColor: '#FFEBE6' }}>
                                                <Avatar alt={user.email} src={user.profilePicture} sx={{ marginRight: '10px' }} />
                                                <Typography variant="body1">{user.email}</Typography>
                                            </Card>
                                        </Badge>
                                    ))}
                                </List>
                            </Box>
                            {/*<Button variant="contained" color="secondary" onClick={fetchCards} fullWidth sx={{ borderRadius: '9999px', mb: 3, backgroundColor: '#FF6F61' }}>
                                Fetch Cards
                            </Button>*/}

                            {/*Use <PlayingCard isClosed={true} for unknown cards and <PlayingCard isClosed={false} for known cards*/}


                                {console.log(communityCards)}
                                {console.log(cards) }
                                <Box mb={3}>
                                    <Typography variant="h6" align="center" color="#3D9970">Game Cards:</Typography>
                                    <List sx={{ display: 'flex', justifyContent: 'center' }}>
                                        {Object.values(communityCards)?.map((card, index) => (
                                            <PlayingCard key={index} card={card} isClosed={card?.length === 0 ? true : false} />)
                                        )}
                                    </List>
                                </Box>
                                    <Box mb={3}>
                                        <Typography variant="h6" align="center" color="#3D9970">Your Hand:</Typography>
                                        <List sx={{ display: 'flex', justifyContent: 'center' }}>
                                            {cards?.map((card, index) => (
                                                <PlayingCard key={index} card={card} isClosed={ false} />
                                            ))}
                                            {[...Array(MAX_HAND_CARDS - (cards? cards?.length:0))].map((_, index) => (
                                                <PlayingCard key={`player-closed-${index}`} isClosed={true} />
                                            ))}
                                        </List>
                                    </Box>
                            {authState.userID === game.currentTurnUserID ? (
                                <Box>
                                    <Typography variant="h6" align="center" color="#3D9970">Perform Action</Typography>
                                        <Box display="flex" justifyContent="center" mb={2}>
                                            <TextField
                                                label="Amount"
                                                type="number"
                                                variant="outlined"
                                                value={betAmount}
                                                onChange={handleBetAmountChange}
                                                sx={{ marginRight: 2 }}
                                            />
                                            <Button
                                                variant="contained"
                                                color="primary"
                                                onClick={() => performAction('Bet', betAmount)}
                                                disabled={!betAmount}
                                                sx={{ marginRight: 2, backgroundColor: '#FF6F61' }}
                                            >
                                                Bet { betAmount} 
                                        </Button>
                                        <Button variant="contained" color="secondary" onClick={() => performAction('Fold', 0)} sx={{ marginRight: 2, backgroundColor: '#FF6F61' }}>
                                            Fold
                                        </Button>
                                        <Button variant="contained" color="secondary" onClick={() => performAction('Check', 0)} sx={{ backgroundColor: '#FF6F61' }}>
                                            Check
                                        </Button>
                                    </Box>
                                </Box>
                            ) : (
                                <Typography variant="h6" align="center" color="#3D9970">Waiting for other players to perform action</Typography>
                            )}
                            {actionResponse && (
                                <Box mt={3}>
                                    <Typography variant="h6" color="#3D9970">Action Response:</Typography>
                                    <Typography>{actionResponse.message}</Typography>
                                </Box>
                            )}
                        </Box>
                    )}
                </CardContent>
            </Paper>
        </Box>
    );
};

export default Game;
