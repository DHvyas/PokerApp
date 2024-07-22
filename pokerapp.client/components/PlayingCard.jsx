import React from 'react';
import { Box, Typography } from '@mui/material';
import { styled } from '@mui/system';

const suits = {
    H: '♥',
    D: '♦',
    C: '♣',
    S: '♠'
};

const suitColors = {
    H: 'red',
    D: 'red',
    C: 'black',
    S: 'black'
};

const CardContainer = styled(Box)(({ theme }) => ({
    width: '100px',
    height: '150px',
    border: '1px solid #000',
    borderRadius: '8px',
    display: 'flex',
    flexDirection: 'column',
    justifyContent: 'space-between',
    alignItems: 'center',
    padding: theme.spacing(1),
    backgroundColor: '#fff',
    margin: theme.spacing(1)
}));

const CardTop = styled(Box)(({ color }) => ({
    color: color,
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'flex-start',
}));

const CardBottom = styled(Box)(({ color }) => ({
    color: color,
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'flex-end',
}));

const PlayingCard = ({ card }) => {
    const rank = card.slice(0, -1);
    const suit = card.slice(-1);
    const suitSymbol = suits[suit];
    const color = suitColors[suit];

    return (
        <CardContainer>
            <CardTop color={color}>
                <Typography variant="h6">{rank}</Typography>
                <Typography variant="h5">{suitSymbol}</Typography>
            </CardTop>
            <Typography variant="h1" style={{ color: color }}>
                {suitSymbol}
            </Typography>

        </CardContainer>
    );
};

export default PlayingCard;
