import React from 'react';
import { Box, Typography } from '@mui/material';
import { styled } from '@mui/system';
import PropTypes from 'prop-types';

const suits = {
    H: '♥',
    D: '♦',
    C: '♣',
    S: '♠'
};

const suitColors = {
    H: '#FF6F61',
    D: '#FF6F61',
    C: '#333',
    S: '#333'
};

const CardContainer = styled(Box)(({isClosed}) => ({
    width: '120px',
    height: '180px',
    border: '1px solid #000',
    borderRadius: '16px',
    display: 'flex',
    flexDirection: 'column',
    justifyContent: 'space-between',
    alignItems: 'center',
    padding: '8px',
    background: isClosed ? 'linear-gradient(45deg, #FE6B8B 30%, #FF8E53 90%)' : '#fff',
    boxShadow: '0 4px 8px rgba(0,0,0,0.1)',
    margin: '8px',
    position: 'relative',
    overflow: 'hidden'
}));

const CardTop = styled(Box)(({ color }) => ({
    color: color,
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'flex-start',
    position: 'absolute',
    top: '8px',
    left: '8px',
}));

const CardBottom = styled(Box)(({ color }) => ({
    color: color,
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'flex-end',
    position: 'absolute',
    bottom: '8px',
    right: '8px',
    transform: 'rotate(180deg)',
}));

const MainSymbol = styled(Typography)(({ color }) => ({
    color: color,
    fontSize: '48px',
    lineHeight: 1,
    position: 'absolute',
    top: '50%',
    left: '50%',
    transform: 'translate(-50%, -50%)'
}));


const PlayingCard = ({ card, isClosed }) => {
    if (isClosed || card === null || card === undefined) {
        return (<CardContainer isClosed={true}>
                
        </CardContainer>);
    }
    console.log('card: ', card);
    const rank = card.slice(0, -1);
    const suit = card.slice(-1);
    const suitSymbol = suits[suit];
    const color = suitColors[suit];

    return (
        <CardContainer isClosed={false}>
            <CardTop color={color}>
                <Typography variant="h6" fontWeight="bold">{rank}</Typography>
                <Typography variant="h5">{suitSymbol}</Typography>
            </CardTop>
            <MainSymbol color={color}>
                {suitSymbol}
            </MainSymbol>
            <CardBottom color={color}>
                <Typography variant="h6" fontWeight="bold">{rank}</Typography>
                <Typography variant="h5">{suitSymbol}</Typography>
            </CardBottom>
        </CardContainer>
    );
};
PlayingCard.propTypes = {
    card: PropTypes.string.IsRequired,
    isClosed: PropTypes.bool.IsRequired
};

export default PlayingCard;
