import React, { createContext, useState, useContext, useEffect } from 'react';

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
    const [authState, setAuthState] = useState({
        userID: null,
        token: null,
        userName: null
    });

    useEffect(() => {
        // Retrieve stored auth state from localStorage on initial render
        const storedAuthState = JSON.parse(localStorage.getItem('authState'));
        if (storedAuthState) {
            setAuthState(storedAuthState);
        }
    }, []);

    const setAuthInfo = ({ userID, token, userName }) => {
        const newAuthState = { userID, token, userName };
        setAuthState(newAuthState);
        localStorage.setItem('authState', JSON.stringify(newAuthState));
    };

    return (
        <AuthContext.Provider value={{ authState, setAuthInfo }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => useContext(AuthContext);
